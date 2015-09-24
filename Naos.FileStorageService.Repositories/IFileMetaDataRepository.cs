namespace Naos.FileStorage.Repositories
{
    using System.Threading.Tasks;
    using Model;

    /// <summary>
    /// Provides a blueprint for file meta data retrieval
    /// </summary>
    public interface IFileMetaDataRepository
    {
        /// <summary>
        /// Saves the meta data surrounding a file
        /// </summary>
        /// <param name="metaData">the meta data of the file</param>
        /// <returns>the id associated with the file for retrieval</returns>
        Task<int> SaveFileMetaDataAsync(FileMetaDataToStore metaData);

        /// <summary>
        /// Gets the meta data for a file by the Id
        /// </summary>
        /// <param name="id">the id of the file</param>
        /// <returns>meta data for a file associated with the given Id</returns>
        Task<FileDetails> GetDetailsForFileById(int id);
    }
}