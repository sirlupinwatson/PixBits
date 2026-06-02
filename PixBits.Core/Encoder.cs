using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace PixBits.Core {
    public static class Encoder {
        public static readonly BigInteger P = BigInteger.Parse("340282366920938463463374607431768211455");
        public static readonly BigInteger K = BigInteger.Parse("314159265358979323846264338327950288419");

        // Step 1: Just pack the data into a 128-bit number
        public static BigInteger PackOnly(double value, uint id, uint salt) {
            long fixedValue = (long)Math.Round(value * 10000); // 4 decimal places
            return (new BigInteger(fixedValue) << 64) | (new BigInteger(id) << 32) | salt;
        }

        // Step 2: Pack AND Scramble (The full process)
        public static BigInteger PackAndScramble(double value, uint id, uint salt) {
            BigInteger packed = PackOnly(value, id, salt);
            return packed * K % P;
        }

        // New: Generates a commitment (hash) of the private data
        public static BigInteger GenerateCommitment(string privateAddress) {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(privateAddress));
            // Ensure it's positive and fits within uint256 range
            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true) % P;
        }

        public static string GetHorizontalScheme(string hex)
        {
            // Ensure we have the last 16 chars like your terminal logic
            string displayHex = hex.PadLeft(16, '0')[^16..];
            var sb = new System.Text.StringBuilder();
            
            // We wrap each "block" in a span with a background color
            foreach (char c in displayHex)
            {
                int val = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
                string cssColor = GetCssColor(val); // Helper for CSS colors
                sb.Append($"<span style='background-color:{cssColor}; width:20px; height:20px; display:inline-block;'></span>");
            }
            return sb.ToString();
        }

        // Map your terminal logic to Web Colors
        private static string GetCssColor(int val)
        {
            return val switch
            {
                0 => "#000000", // Black
                1 => "#0000AA", // Blue
                2 => "#00AA00", // Green
                3 => "#00AAAA", // Cyan
                4 => "#AA0000", // Red
                5 => "#AA00AA", // Magenta
                6 => "#AA5500", // Brown/Orange
                7 => "#AAAAAA", // Gray
                8 => "#555555", // Dark Gray
                9 => "#5555FF", // Bright Blue
                10 => "#55FF55", // Bright Green
                11 => "#55FFFF", // Bright Cyan
                12 => "#FF5555", // Bright Red
                13 => "#FF55FF", // Bright Magenta
                14 => "#FFFF55", // Yellow
                _ => "#FFFFFF"  // White
            };
        }
    }
}
