using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskWPFExperiment.DataAccess.Common
{
    public class FileStorage : IStorage
    {
        public async Task<T?> GetData<T>(string filePath)
        {
            CreateIfNotExists(filePath);
            string dataRaw = await File.ReadAllTextAsync(filePath);
            if(dataRaw == string.Empty)
            {
                return default;
            }
            var data = JsonSerializer.Deserialize<T>(dataRaw);

            return data;
        }

        public async Task SetData<T>(string filePath, T data)
        {
            CreateIfNotExists(filePath);
            string dataRaw = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(filePath, dataRaw);
        }

        private void CreateIfNotExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                using var fs = File.Create(filePath);
            }
        }
    }
}
