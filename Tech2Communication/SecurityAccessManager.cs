using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tech2Communication
{
    public enum AccessLevel
    {
        AccessLevel01,
        AccessLevelFB,
        AccessLevelFD // Highest access level
    }

    public class SecurityAccessManager
    {
        private SerialCommunication serialComm;
        private SeedToKey seedCalculator;
        private Tech2DataParser tech2DataParser;

        public SecurityAccessManager(SerialCommunication serialComm)
        {
            this.serialComm = serialComm;
            this.seedCalculator = new SeedToKey();
            this.tech2DataParser = new Tech2DataParser();
        }

        public bool RequestSecurityAccess(AccessLevel level, out string extractedVIN)
        {
            extractedVIN = string.Empty;

            try
            {
                // Format security access request based on access level
                byte[] requestCmd = FormatSecurityAccessRequest(level);

                // Send the request and receive seed
                byte[] response = serialComm.SendAndReceive(requestCmd, 500);
                if (response == null || response.Length < 5)
                {
                    Console.WriteLine("Failed to receive seed response");
                    return false;
                }

                // Extract seed from response
                byte[] seed = new byte[2];
                // Typically seed is in bytes 3-4 of the response
                seed[0] = response[3];
                seed[1] = response[4];

                // Log the seed for debugging
                Console.WriteLine($"Received seed: {BitConverter.ToString(seed)}");

                // Check if seed is 0x0000, which means security access is already granted
                if (seed[0] == 0x00 && seed[1] == 0x00)
                {
                    Console.WriteLine("Security access already granted (seed 0000)");

                    // Even if security is already granted, try to extract VIN
                    extractedVIN = ReadVehicleVIN();
                    return true;
                }

                // Try to extract VIN from the response data
                // First try Tech2 data parsing approach
                extractedVIN = tech2DataParser.ExtractVINFromTech2Data(response);

                // If that didn't work, we'll try direct VIN request after security access

                // Calculate key based on seed
                byte[] key = seedCalculator.CalculateKey(seed, level);
                Console.WriteLine($"Calculated key: {BitConverter.ToString(key)}");

                // Wait a bit before sending key (some ECUs require this)
                Thread.Sleep(100);

                // Format and send the key response
                byte[] keyCmd = FormatKeyResponse(key, level);
                byte[] keyResponse = serialComm.SendAndReceive(keyCmd, 500);

                // Check if access was granted
                if (keyResponse != null && keyResponse.Length >= 3)
                {
                    // Response should be something like: 0x01, 0x67, 0xFE (for AccessLevelFD)
                    if (keyResponse[1] == 0x67 &&
                       (keyResponse[2] == 0xFE || keyResponse[2] == 0xFC || keyResponse[2] == 0x02))
                    {
                        Console.WriteLine("Security access granted");

                        // If we didn't get VIN from Tech2 data parsing, try direct request
                        if (string.IsNullOrEmpty(extractedVIN))
                        {
                            extractedVIN = ReadVehicleVIN();
                        }

                        return true;
                    }
                    else if (keyResponse[1] == 0x7F && keyResponse[2] == 0x27)
                    {
                        Console.WriteLine($"Security access error: {TranslateErrorCode(keyResponse[3])}");
                    }
                    else
                    {
                        Console.WriteLine($"Unexpected response: {BitConverter.ToString(keyResponse)}");
                    }
                }
                else
                {
                    Console.WriteLine("No valid response received to key request");
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during security access: {ex.Message}");
                return false;
            }
        }

        private byte[] FormatSecurityAccessRequest(AccessLevel level)
        {
            // Format based on the Tech2 protocol
            byte[] cmd = new byte[]{
                0x07, // Length
                0xE0, // ID high byte (0x7E0 is standard diagnostic address)
                0x00, // ID low byte
                0x03, // Data length
                0x27, // Service ID: Security Access
                0x00, // Access level placeholder
                0x00  // Padding
            };

            // Set access level based on the parameter
            switch (level)
            {
                case AccessLevel.AccessLevel01:
                    cmd[5] = 0x01;
                    break;
                case AccessLevel.AccessLevelFB:
                    cmd[5] = 0xFB;
                    break;
                case AccessLevel.AccessLevelFD:
                    cmd[5] = 0xFD;
                    break;
            }

            return cmd;
        }

        private byte[] FormatKeyResponse(byte[] key, AccessLevel level)
        {
            // Format the key response
            byte[] cmd = new byte[]{
                0x09, // Length
                0xE0, // ID high byte
                0x00, // ID low byte
                0x05, // Data length
                0x27, // Service ID: Security Access
                0x00, // Response type placeholder
                key[0], // Key high byte
                key[1], // Key low byte
                0x00  // Padding
            };

            // Set response type based on the access level
            switch (level)
            {
                case AccessLevel.AccessLevel01:
                    cmd[5] = 0x02;
                    break;
                case AccessLevel.AccessLevelFB:
                    cmd[5] = 0xFC;
                    break;
                case AccessLevel.AccessLevelFD:
                    cmd[5] = 0xFE;
                    break;
            }

            return cmd;
        }

        private string ReadVehicleVIN()
        {
            try
            {
                // Format ReadDataByIdentifier request with VIN identifier
                byte[] vinRequest = new byte[]{
                    0x07, // Length
                    0xE0, // ID high byte (0x7E0)
                    0x00, // ID low byte
                    0x03, // Data length
                    0x22, // Service ID: ReadDataByIdentifier
                    0xF1, // VIN identifier high byte
                    0x90  // VIN identifier low byte
                };

                Console.WriteLine("Sending ReadDataByIdentifier for VIN");

                // Send request and receive response
                byte[] response = serialComm.SendAndReceive(vinRequest, 500);

                if (response == null || response.Length < 5)
                {
                    Console.WriteLine("No valid response for VIN request");
                    return string.Empty;
                }

                // Check for positive response
                if (response[4] != 0x62) // 0x62 is positive response to 0x22
                {
                    Console.WriteLine($"Negative response to VIN request: {BitConverter.ToString(response)}");
                    return string.Empty;
                }

                // Extract VIN from response (typically starts at offset 6)
                StringBuilder vin = new StringBuilder();
                for (int i = 6; i < response.Length && vin.Length < 17; i++)
                {
                    // Filter for valid VIN characters (A-Z, 0-9)
                    char c = (char)response[i];
                    if ((c >= 'A' && c <= 'Z' && c != 'I' && c != 'O' && c != 'Q') ||
                        (c >= '0' && c <= '9'))
                    {
                        vin.Append(c);
                    }
                }

                string vinString = vin.ToString();
                Console.WriteLine($"Extracted VIN: {vinString}");
                return vinString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception reading VIN: {ex.Message}");
                return string.Empty;
            }
        }

        private string TranslateErrorCode(byte code)
        {
            switch (code)
            {
                case 0x10: return "General reject";
                case 0x11: return "Service not supported";
                case 0x12: return "Sub-function not supported";
                case 0x22: return "Conditions not correct";
                case 0x31: return "Request out of range";
                case 0x33: return "Security access denied";
                case 0x35: return "Invalid key supplied";
                case 0x36: return "Exceeded number of attempts";
                case 0x37: return "Required time delay not expired";
                case 0x78: return "Response pending";
                default: return $"Unknown error code: 0x{code:X2}";
            }
        }

        public void SendKeepAlive()
        {
            try
            {
                // Send tester present message
                byte[] cmd = new byte[]{
                    0x06, // Length
                    0xE0, // ID high byte
                    0x00, // ID low byte
                    0x02, // Data length
                    0x3E, // Service ID: Tester Present
                    0x00  // Subfunction: Default
                };

                serialComm.Send(cmd);
                // Optionally wait for response
                byte[] response = serialComm.Receive(100);

                // Don't log this to avoid cluttering the console
                // if (response != null && response.Length > 0)
                //     Console.WriteLine("Keep-alive response received");
            }
            catch (Exception ex)
            {
                // Just log but don't throw - keep-alive failures shouldn't interrupt workflow
                Console.WriteLine($"Failed to send keep-alive: {ex.Message}");
            }
        }
    }
}