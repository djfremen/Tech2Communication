using System;

namespace Tech2Communication
{
    public class SeedToKey
    {
        /// <summary>
        /// Calculates the key from the seed for the specified access level
        /// </summary>
        public byte[] CalculateKey(byte[] seed, AccessLevel level)
        {
            ushort seedValue = (ushort)((seed[0] << 8) | seed[1]);
            ushort key = 0;

            // First common transformation
            key = ConvertSeed(seedValue);

            // Apply level-specific transformations
            switch (level)
            {
                case AccessLevel.AccessLevelFD:
                    key /= 3;
                    key ^= 0x8749;
                    key += 0x0ACF;
                    key ^= 0x81BF;
                    break;

                case AccessLevel.AccessLevelFB:
                    key ^= 0x8749;
                    key += 0x06D3;
                    key ^= 0xCFDF;
                    break;

                case AccessLevel.AccessLevel01:
                    // Specific transformations for Level 01 if needed
                    // May need to be determined by analyzing the TIS2Web code
                    break;
            }

            // Return as byte array
            byte[] keyBytes = new byte[2];
            keyBytes[0] = (byte)((key >> 8) & 0xFF);
            keyBytes[1] = (byte)(key & 0xFF);
            return keyBytes;
        }

        private ushort ConvertSeed(ushort seed)
        {
            // Standard T8 algorithm from the original code
            ushort key = (ushort)((seed >> 5) | (seed << 11));
            return (ushort)((key + 0xB988) & 0xFFFF);
        }

        // For CIM (Control Integration Module) if needed
        public byte[] CalculateKeyForCIM(byte[] seed)
        {
            ushort seedValue = (ushort)((seed[0] << 8) | seed[1]);
            ushort key = ConvertSeedCIM(seedValue);

            byte[] keyBytes = new byte[2];
            keyBytes[0] = (byte)((key >> 8) & 0xFF);
            keyBytes[1] = (byte)(key & 0xFF);
            return keyBytes;
        }

        private ushort ConvertSeedCIM(ushort seed)
        {
            ushort key = (ushort)((seed + 0x9130) & 0xFFFF);
            key = (ushort)((key >> 8) | (key << 8));
            return (ushort)((0x3FC7 - key) & 0xFFFF);
        }

        // For ME96 if needed
        public byte[] CalculateKeyForME96(byte[] seed)
        {
            ushort seedValue = (ushort)((seed[0] << 8) | seed[1]);
            ushort key = RetSeed(seedValue);

            byte[] keyBytes = new byte[2];
            keyBytes[0] = (byte)((key >> 8) & 0xFF);
            keyBytes[1] = (byte)(key & 0xFF);
            return keyBytes;
        }

        private ushort RetSeed(ushort seed)
        {
            // ME96 algorithm from the original code
            int component2 = (0xEB + seed) & 0xFF;

            // Handle anomalies
            if (seed >= 0x3808 && seed < 0xA408)
                component2 -= 1;

            return (ushort)(((component2 << 9) | ((((0x5BF8 + seed) >> 8) & 0xFF) << 1) | ((component2 >> 7) & 1)) & 0xFFFF);
        }
    }
}