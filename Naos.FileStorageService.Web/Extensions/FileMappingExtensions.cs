namespace Naos.FileStorageService.Web.Extensions
{
    using Model;
    using Noas.FileStorage.Repositories.Model;
    using Utility;

    public static class FileMappingExtensions
    {
        public static FileMetaDataToStore ToMetaData(this FileWrapper fileWrapper)
        {
            var metaData = new FileMetaDataToStore();
            metaData.ApplicationName = fileWrapper.ApplicationName;
            metaData.FileName = fileWrapper.FileName;

            // if they don't send in a checksum then we are going to say ours is the master.
            if (string.IsNullOrEmpty(fileWrapper.Sha256Checksum))
            {
                metaData.Checksum = Checksummer.ComputeChecksum(fileWrapper.Payload);
            }
            else
            {
                metaData.Checksum = Checksummer.ComputeChecksum(fileWrapper.Payload);
            }

            metaData.MimeType = fileWrapper.MimeType;

            return metaData;
        }

        public static FileWrapper ToFileWrapper(this FileDetails fileDetails, byte[] payload)
        {
            var fileWrapper = new FileWrapper();

            fileWrapper.FileName = fileDetails.FileName;
            fileWrapper.ApplicationName = fileDetails.ApplicationName;
            fileWrapper.Sha256Checksum = fileDetails.Checksum;
            fileWrapper.MimeType = fileDetails.MimeType;
            fileWrapper.Payload = payload;

            return fileWrapper;
        }
    }
}