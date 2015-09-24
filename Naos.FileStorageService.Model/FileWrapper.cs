namespace Naos.FileStorage.Contract
{
    using System.ComponentModel.DataAnnotations;
    using Validation;

    public class FileWrapper
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public string MimeType { get; set; }

        [NotNullOrEmptyByteArray]
        public byte[] Payload { get; set; }

        [Required]
        public string ApplicationName { get; set; }
        
        public string Sha256Checksum { get; set; }}
}
