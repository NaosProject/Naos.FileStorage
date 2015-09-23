namespace Noas.FileStorage.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Model;

    public class FileMetaDataRepository : IFileMetaDataRepository
    {
        private readonly string connectionString;

        public FileMetaDataRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <inheritdoc/>
        public async Task<int> SaveFileMetaDataAsync(FileMetaDataToStore metaData)
        {
            int fileId;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("InsertFileMetaData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FileName", metaData.FileName);
                    command.Parameters.AddWithValue("@MimeType", metaData.MimeType);
                    command.Parameters.AddWithValue("@ApplicationName", metaData.ApplicationName);
                    command.Parameters.AddWithValue("@InternalStoragePath", metaData.StoragePath);
                    command.Parameters.AddWithValue("@Checksum", metaData.Checksum);

                    await connection.OpenAsync();
                    fileId = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }

            return fileId;
        }

        /// <inheritdoc/>
        public async Task<FileDetails> GetDetailsForFileById(int id)
        {
            FileDetails details = null;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetFileMetaDatabyId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("Id", id);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            details = new FileDetails();
                            details.ApplicationName = reader["ApplicationName"].ToString();
                            details.FileName = reader["FileName"].ToString();
                            details.FileId = (int)reader["Id"];
                            details.MimeType = reader["MimeType"].ToString();
                            details.StoragePath = reader["InternalStoragePath"].ToString();
                            details.Checksum = reader["Checksum"].ToString();
                            details.DateStoredUtc = (DateTime)reader["DateStoredUtc"];
                        }
                    }
                }
            }

            return details;
        }
    }
}
