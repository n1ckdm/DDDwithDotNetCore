using Marketplace.Domain;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure
{
    public class ClassifiedAdRepository : IClassifiedAdRepository
    {
        private readonly IAsyncDocumentSession _session;

        public ClassifiedAdRepository(IAsyncDocumentSession session) => _session = session;

        public Task<bool> Exists(ClassifiedAdId id)
            => _session.Advanced.ExistsAsync(EntityId(id));

        public Task<ClassifiedAd> Load(ClassifiedAdId id)
            => _session.LoadAsync<ClassifiedAd>(EntityId(id));

        public async Task Add(ClassifiedAd entity)
        {
            await _session.StoreAsync(entity, EntityId(entity.Id));
        }
        
        private static string EntityId(ClassifiedAdId id) => $"ClassifiedAd/{id}";
    }
}