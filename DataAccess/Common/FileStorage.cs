using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskWPFExperiment.DataAccess.Common
{
    public class FileStorage : IStorage
    {
        public async Task<T> GetData<T>(string filePath)
        {
            string dataRaw = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<T>(dataRaw) ?? Activator.CreateInstance<T>();

            return data;
        }

        public async Task SetData<T>(string filePath, T data)
        {
            string dataRaw = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(filePath, dataRaw);
        }
    }
}
