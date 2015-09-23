namespace Naos.FileStorageService.Web.Storage
{
    using System.IO;
    using System.Threading.Tasks;

    public class UfsFileStorageProvider : IFileStorageProvider
    {
        private const int BUFFER_SIZE = 4096;

        public async Task StoreFile(string pathWithFileName, byte[] payload)
        {
            FileStream fileStream = null;

            var directoryToWriteTo = Path.GetDirectoryName(pathWithFileName);

            if (!Directory.Exists(directoryToWriteTo))
            {
                Directory.CreateDirectory(directoryToWriteTo);
            }

            try
            {
                fileStream = new FileStream(pathWithFileName, FileMode.Append, FileAccess.Write, FileShare.None, BUFFER_SIZE, true);
                await fileStream.WriteAsync(payload, 0, payload.Length);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public async Task<byte[]> GetFile(string storagePath)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(storagePath, FileMode.Open, FileAccess.Read, FileShare.Read, BUFFER_SIZE, true);
                var file = new byte[fileStream.Length];
                await fileStream.ReadAsync(file, 0, (int) fileStream.Length);

                return file;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

        }
    }
}