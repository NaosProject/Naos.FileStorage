namespace Naos.FileStorageService.Client
{
    using System;
    using System.Collections.Generic;

    public class FileStorageResult
    {
        public FileStorageResult()
        {
            Errors = new List<string>();
        }

        public bool Success { get; set; }

        public string FileUrl { get; set; }

        public List<string> Errors { get; set; }

        public Exception StorageException { get; set; }
    }
}