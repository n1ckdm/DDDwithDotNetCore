using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public interface IEntityStore
    {
        Task<T> Load<T>(string entityId) where T : Entity;
        Task Save<T>(T Entity) where T : Entity;
        Task<bool> Exists<T>(string entityId);
    }
}