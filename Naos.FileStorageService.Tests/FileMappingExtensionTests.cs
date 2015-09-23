namespace Naos.FileStorageService.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using Web.Extensions;
    using Web.Utility;

    [TestClass]
    public class FileMappingExtensionTests
    {
        [TestMethod]
        public void FileWrapper_ToMetaData_Returns_Properly_Hydrated()
        {
            var fileWrapper = new FileWrapper();
            fileWrapper.ApplicationName = "application";
            fileWrapper.FileName = "file.txt";
            fileWrapper.MimeType = "mimetype";
            fileWrapper.Payload = new byte []{1, 0, 1};

            var metaData = fileWrapper.ToMetaData();

            Assert.AreEqual(metaData.ApplicationName, fileWrapper.ApplicationName);
            Assert.AreEqual(metaData.FileName, fileWrapper.FileName);
            Assert.AreEqual(metaData.MimeType, fileWrapper.MimeType);
            Assert.AreEqual(metaData.Checksum, Checksummer.ComputeChecksum(fileWrapper.Payload));
        }
    }
}
