namespace Naos.FileStorageService.Client
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Model;

    public class FileStorageServiceClient
    {
        private readonly string storageApiUrl;

        public FileStorageServiceClient(string storageApiUrl)
        {
            this.storageApiUrl = storageApiUrl;
        }

        /// <summary>
        /// Saves a file via the file storage Web API
        /// </summary>
        /// <param name="file">the file to save.</param>
        /// <returns>a storage result indicating the success of the attempt.  If successful the FileUrl property will be populated with a Url to retrieve the resource as needed.</returns>
        public async Task<FileStorageResult> SaveFileAsync(FileWrapper file)
        {
            var result = new FileStorageResult();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(storageApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.PostAsJsonAsync("api/File", file);

                    result.Success = response.IsSuccessStatusCode;

                    if (!result.Success)
                    {
                        result.Errors.Add(response.ReasonPhrase);
                    }
                    else
                    {
                        result.FileUrl = response.Headers.Location.AbsoluteUri;
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.StorageException = e;
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a file from the file storage API. 
        /// </summary>
        /// <param name="fileUrl">the absolute Url to the file resource</param>
        /// <returns>the file located at the Url or null if it isn't found.</returns>
        public async Task<FileWrapper> GetFileAsync(string fileUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(fileUrl);

                if (response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsAsync<FileWrapper>();

                    return file;
                }

                return null;
            }
        }
    }
}
