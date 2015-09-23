namespace Naos.FileStorageService.Tests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Web.Storage;
    using Web.Utility;

    [TestClass]
    public class UfsStorageProviderTests
    {
        private const string SANDBOX_FILE_STORAGE_PATH = @"\\zcloud.com\dvufs\Scratch\Architecture\FileStorageService\";

        private IFileStorageProvider storageProvider;

        [TestInitialize]
        public void Initialize()
        {
            storageProvider = new UfsFileStorageProvider();
        }

        [TestMethod]
        [DeploymentItem("Test.txt")]
        public void Valid_Path_And_Payload_Store_Correctly()
        {
            var file = File.ReadAllBytes("Test.txt");
            
            var uniqueId = Guid.NewGuid().ToString();
            var fullUniqueName = uniqueId + "_" + "Test.txt";

            var pathToWriteTo = SANDBOX_FILE_STORAGE_PATH + fullUniqueName;
            var task = storageProvider.StoreFile(pathToWriteTo, file).GetAwaiter();

            while (!task.IsCompleted)
            {
                /* no op, just need to make sure its ready to ride. */
            }

            // make sure the got the file.
            Assert.IsTrue(File.Exists(pathToWriteTo));
            
            // make sure the file is the same as the one we were supposed to write.
            var checkSumBeforeWriting = Checksummer.ComputeChecksum(file);
            var bytesWrittenToDisk = File.ReadAllBytes(pathToWriteTo);
            var checkSumAfterWriting = Checksummer.ComputeChecksum(bytesWrittenToDisk);

            Assert.AreEqual(checkSumBeforeWriting, checkSumAfterWriting);

            // do some cleanup.
            File.Delete(pathToWriteTo);
        }

        [TestMethod]
        [DeploymentItem("Test.txt")]
        public void Non_Existent_Path_Is_Created()
        {
            var file = File.ReadAllBytes("Test.txt");
            var uniqueId = Guid.NewGuid().ToString();
            var fullUniqueName = uniqueId + "_" + "Test.txt";

            var filePathThatNeedsCreating = SANDBOX_FILE_STORAGE_PATH + "Some_Missing_Path\\" + fullUniqueName;
            var task = storageProvider.StoreFile(filePathThatNeedsCreating, file).GetAwaiter();

            while (!task.IsCompleted)
            {

            }

            Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(filePathThatNeedsCreating)));
            Assert.IsTrue(File.Exists(filePathThatNeedsCreating));

            File.Delete(filePathThatNeedsCreating);
            Directory.Delete(Path.GetDirectoryName(filePathThatNeedsCreating));
        }
    }
}
