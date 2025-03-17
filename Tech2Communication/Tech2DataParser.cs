using System;
using System.IO;
using System.Text;

namespace Tech2Communication
{
    public class Tech2DataParser
    {
        public string ExtractVINFromTech2Data(byte[] tech2Data)
        {
            try
            {
                // Locate the VIN record
                int vinRecordOffset = FindVINRecord(tech2Data);
                if (vinRecordOffset == -1)
                    return string.Empty;

                // Extract metadata needed for decoding key
                byte vehicleModel = 0x00;
                byte manufactureYear = 0x00;

                // Try to extract metadata if available in the data
                if (tech2Data.Length > 0x30)
                {
                    // Standard offsets for vehicle metadata
                    vehicleModel = tech2Data[0x24];
                    manufactureYear = tech2Data[0x30];
                }

                // Extract raw encoded VIN data (adding safety check)
                if (vinRecordOffset + 23 > tech2Data.Length)
                    return string.Empty;

                byte[] encodedVIN = new byte[17];
                Array.Copy(tech2Data, vinRecordOffset + 6, encodedVIN, 0, 17); // +6 for header/length

                // Generate decoding key
                byte[] decodingKey = GenerateVINDecodingKey(vehicleModel, manufactureYear);

                // Apply decoding algorithm
                char[] decodedVIN = new char[17];
                for (int i = 0; i < 17; i++)
                {
                    byte currentByte = encodedVIN[i];

                    // Apply key XOR - the key byte used depends on position
                    currentByte ^= decodingKey[i % decodingKey.Length];

                    // Apply bit rotation - different for different positions
                    if (i < 9) // Different algorithm for first 9 characters
                    {
                        // Rotate left by 2 for WMI and VDS sections
                        currentByte = (byte)((currentByte << 2) | (currentByte >> 6));
                    }
                    else
                    {
                        // Rotate right by 1 for VIS section
                        currentByte = (byte)((currentByte >> 1) | (currentByte << 7));
                    }

                    // Character substitution and validation
                    char vinChar = DecodeVINCharacter(currentByte, i);
                    decodedVIN[i] = vinChar;
                }

                string vin = new string(decodedVIN);

                // Final validation - ensure it's a valid-looking VIN
                if (IsValidVIN(vin))
                    return vin;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting VIN: " + ex.Message);
                return string.Empty;
            }
        }

        private int FindVINRecord(byte[] data)
        {
            // Search for VIN record signature
            byte[] vinSignature = new byte[] { 0x56, 0x49, 0x4E, 0x90 }; // "VIN" + record type

            for (int i = 0; i < data.Length - vinSignature.Length; i++)
            {
                bool signatureMatch = true;
                for (int j = 0; j < vinSignature.Length; j++)
                {
                    if (data[i + j] != vinSignature[j])
                    {
                        signatureMatch = false;
                        break;
                    }
                }

                if (signatureMatch)
                    return i;
            }

            // Alternative signature used by some Tech2 versions
            byte[] altSignature = new byte[] { 0x56, 0x45, 0x48, 0x91 }; // "VEH" + alt record type

            for (int i = 0; i < data.Length - altSignature.Length; i++)
            {
                bool signatureMatch = true;
                for (int j = 0; j < altSignature.Length; j++)
                {
                    if (data[i + j] != altSignature[j])
                    {
                        signatureMatch = false;
                        break;
                    }
                }

                if (signatureMatch)
                    return i;
            }

            return -1; // Not found
        }

        private byte[] GenerateVINDecodingKey(byte vehicleModel, byte year)
        {
            // Base key seed found in the code
            byte[] baseKey = new byte[] { 0x4D, 0x37, 0x6F, 0x29, 0x5A, 0xE1, 0xB3, 0x49 };

            // Apply vehicle-specific modifications
            for (int i = 0; i < baseKey.Length; i++)
            {
                if (i % 2 == 0)
                    baseKey[i] ^= vehicleModel;
                else
                    baseKey[i] ^= year;

                // Additional scrambling
                baseKey[i] = (byte)((baseKey[i] * 0x0F + 0x71) % 0xFF);
            }

            return baseKey;
        }

        private char DecodeVINCharacter(byte encoded, int position)
        {
            char c = (char)encoded;

            // Handle specific position constraints
            if (position == 9) // Check digit - must be 0-9 or X
            {
                // Ensure it's a valid check digit
                if (c >= '0' && c <= '9')
                    return c;
                else
                    return 'X'; // Common default for check digit
            }
            else if (position >= 10) // VIS section - typically more numeric
            {
                // Ensure numeric characters for certain positions
                if (c >= '0' && c <= '9')
                    return c;

                // Map out-of-range characters to valid ones
                return MapToValidVINCharacter(c);
            }
            else // WMI and VDS sections - more alphabetic
            {
                // Filter out invalid VIN characters
                if ((c >= 'A' && c <= 'Z' && c != 'I' && c != 'O' && c != 'Q') ||
                    (c >= '0' && c <= '9'))
                    return c;

                // Map invalid characters to valid ones
                return MapToValidVINCharacter(c);
            }
        }

        private char MapToValidVINCharacter(char c)
        {
            // Substitution table for non-standard characters
            switch (c)
            {
                case 'I': return '1';
                case 'O': return '0';
                case 'Q': return '9';
                default:
                    // For any other invalid character, map to a valid range
                    if (c < '0')
                        return (char)('0' + (c % 10));
                    else if (c > 'Z')
                        return (char)('A' + (c % 26));
                    else
                        return c;
            }
        }

        private bool IsValidVIN(string vin)
        {
            // Basic VIN validation
            if (vin.Length != 17)
                return false;

            // Check for valid characters
            foreach (char c in vin)
            {
                if (!((c >= 'A' && c <= 'Z' && c != 'I' && c != 'O' && c != 'Q') ||
                      (c >= '0' && c <= '9')))
                {
                    return false;
                }
            }

            // Additional validation could include the VIN check digit calculation

            return true;
        }
    }
}