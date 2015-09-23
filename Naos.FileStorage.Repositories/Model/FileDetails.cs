namespace Noas.FileStorage.Repositories.Model
{
    using System;

    public class FileDetails
    {
        public int FileId { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public string ApplicationName { get; set; }

        public string StoragePath { get; set; }

        public string Checksum { get; set; }

        public DateTime DateStoredUtc { get; set; }
    }
}
