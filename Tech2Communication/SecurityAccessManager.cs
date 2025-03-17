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

        public SecurityAccessManager(SerialCommunication serialComm)
        {
            this.serialComm = serialComm;
            this.seedCalculator = new SeedToKey();
        }

        public bool RequestSecurityAccess(AccessLevel level, out string extractedVIN)
        {
            extractedVIN = string.Empty;

            // Format security access request based on access level
            byte[] requestCmd = FormatSecurityAccessRequest(level);

            // Send the request and receive seed
            byte[] response = serialComm.SendAndReceive(requestCmd, 500);
            if (response == null || response.Length < 4)
            {
                Console.WriteLine("Failed to receive seed response");
                return false;
            }

            // Extract seed from response
            byte[] seed = new byte[2];
            // Typically seed is in bytes 3-4 of the response
            seed[0] = response[3];
            seed[1] = response[4];

            // Check if seed is 0x0000, which means security access is already granted
            if (seed[0] == 0x00 && seed[1] == 0x00)
            {
                return true;
            }

            // Extract VIN from the seed response if available
            extractedVIN = ExtractVINFromSeed(response);

            // Calculate key based on seed
            byte[] key = seedCalculator.CalculateKey(seed, level);

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
                    return true;
                }
                else if (keyResponse[1] == 0x7F && keyResponse[2] == 0x27)
                {
                    Console.WriteLine($"Security access error: {TranslateErrorCode(keyResponse[3])}");
                }
            }

            return false;
        }

        private byte[] FormatSecurityAccessRequest(AccessLevel level)
        {
            // Format based on the TIS2Web protocol
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

        private string ExtractVINFromSeed(byte[] response)
        {
            // This is where you'd implement the VIN extraction algorithm
            // The exact algorithm will depend on how the VIN is encoded in the seed response

            // Placeholder for the actual implementation
            // In the original TIS2Web, this might involve specific decoding of the seed value
            // or additional data fields in the response

            try
            {
                // Example implementation - this is simplified and needs to be adjusted
                // In reality, you'd need to analyze the TIS2Web code to see how it extracts the VIN

                // Check if the response contains enough data
                if (response.Length < 10) return string.Empty;

                // The VIN might be encoded in bytes after the seed
                // or might require a separate request after security access

                // Placeholder VIN extraction - replace with actual algorithm
                StringBuilder vin = new StringBuilder();
                for (int i = 5; i < Math.Min(response.Length, 22); i++)
                {
                    // Filter for valid VIN characters
                    if ((response[i] >= 'A' && response[i] <= 'Z') ||
                        (response[i] >= '0' && response[i] <= '9'))
                    {
                        vin.Append((char)response[i]);
                    }
                }

                // Standard VIN is 17 characters
                if (vin.Length == 17)
                {
                    return vin.ToString();
                }

                // If we can't extract the VIN from the seed response,
                // it might need a separate ReadDataByIdentifier request
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting VIN: {ex.Message}");
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
            // serialComm.Receive(100);
        }
    }
}