
using System.Threading.Tasks;

namespace TaskWPFExperiment.DataAccess.Common
{
    public interface IStorage
    {
        Task<T?> GetData<T>(string filePath);
        Task SetData<T>(string filePath, T data);
    }
}
