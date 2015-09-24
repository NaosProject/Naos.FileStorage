namespace Naos.FileStorageService.Web.Controllers
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Extensions;
    using log4net;
    using Model;
    using Repositories;
    using Repositories.Model;
    using Storage;
    using Utility;

    /// <summary>
    /// Entry point for file storage.
    /// </summary>
    public class FileController : ApiController
    {
        private readonly IFileStorageProvider fileStorageProvider;

        private readonly IFileMetaDataRepository metaDataRepository;

        private readonly string baseFileStoragePath;

        private readonly ILog logger;
        
        public FileController(IFileStorageProvider fileStorageProvider, IFileMetaDataRepository metaDataRepository)
        {
            this.fileStorageProvider = fileStorageProvider;
            this.metaDataRepository = metaDataRepository;
            
            baseFileStoragePath = ConfigurationManager.AppSettings["BaseFileStoragePath"];
            logger = LogManager.GetLogger(GetType());
        }

        [HttpPost]
        [Route("api/File/")]
        public async Task<HttpResponseMessage> SaveFileAsync(FileWrapper file)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            // todo: consider rolling this into a validation attribute.  
            if (!string.IsNullOrEmpty(file.Sha256Checksum))
            {
                if (!Checksummer.ChecksumsMatch(file.Sha256Checksum, file.Payload))
                {
                    logger.Info(string.Format("Checksum mismatch for file with name: {0} from application: {1}", file.FileName, file.ApplicationName));
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The checksum submitted did not match the server computed SHA256 checksum.");
                }
            }

            try
            {
                var uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
                var filePath = BuildFilePathAndAppendFileName(baseFileStoragePath, file.ApplicationName, uniqueFileName);
                await fileStorageProvider.StoreFile(filePath, file.Payload);

                var metaData = file.ToMetaData();
                metaData.StoragePath = filePath;
                var fileId = await metaDataRepository.SaveFileMetaDataAsync(metaData);

                var response = Request.CreateResponse(HttpStatusCode.Created, file);
                response.Headers.Location = new Uri(Url.Link("GetFileById", new { id = fileId}));

                return response;
            }
            catch (Exception e)
            {
                logger.Error(string.Format("error saving file with name: {0} for application: {1}", file.FileName, file.ApplicationName), e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "File Storage API experienced an error.  This has been reported.  Please try again shortly.");
            }
        }

        [HttpGet]
        [Route("api/File/{id}", Name="GetFileById")]
        public async Task<FileWrapper> GetFileAsync(int id)
        {
            FileDetails fileDetails;
            try
            {
                fileDetails = await metaDataRepository.GetDetailsForFileById(id);
            }
            catch (Exception e)
            {
                logger.Error(string.Format("error getting file details for file with id: {0}", id), e);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            if (fileDetails == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            try
            {
                var payload = await fileStorageProvider.GetFile(fileDetails.StoragePath);
                var fileWrapper = fileDetails.ToFileWrapper(payload);
                return fileWrapper;
            }
            catch (Exception e)
            {
                logger.Error(string.Format("error retrieving file with id {0}", id), e);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        private static string BuildFilePathAndAppendFileName(string basepath, string applicationName, string uniqueFileName)
        {
            if (!basepath.EndsWith(@"\"))
            {
                basepath = basepath + @"\";
            }

            // this will render the path {BasePath}\{ApplicationName}\20150831\
            // this will keep paths from getting so many files in them that we see latency from enumerating files on access.
            basepath = basepath + applicationName + @"\" + string.Format("{0:yyyyMMdd}", DateTime.UtcNow) + @"\" + uniqueFileName;

            return basepath;
        }
    }
}
