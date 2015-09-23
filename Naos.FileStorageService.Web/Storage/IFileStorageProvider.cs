namespace Naos.FileStorageService.Web.Storage
{
    using System.Threading.Tasks;

    public interface IFileStorageProvider
    {
        Task StoreFile(string path, byte[] payload);

        Task<byte[]> GetFile(string storagePath);
    }
}