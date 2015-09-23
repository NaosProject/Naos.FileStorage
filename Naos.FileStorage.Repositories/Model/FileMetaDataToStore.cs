namespace Noas.FileStorage.Repositories.Model
{
    public class FileMetaDataToStore
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public string Checksum { get; set; }

        public string ApplicationName { get; set; }

        public string StoragePath { get; set; }
    }
}
