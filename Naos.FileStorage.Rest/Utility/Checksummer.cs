namespace Naos.FileStorage.Rest.Utility
{
    using System;
    using System.Security.Cryptography;

    public class Checksummer
    {
        public static string ComputeChecksum(byte[] bytesToChecksum)
        {
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(bytesToChecksum));
            }
        }

        public static bool ChecksumsMatch(string originalChecksum, byte[] payloadToCompare)
        {
            var checksumOfCurrentFile = ComputeChecksum(payloadToCompare);
            return originalChecksum.Equals(checksumOfCurrentFile);
        }
    }
}