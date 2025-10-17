using PruebaTostao.Entities.DocumentEntity;

namespace PruebaTostao.Repository
{
    public interface IDocumentRepository
    {
        Task CreateAsync(Document document);
        Task<Document?> GetByIdAsync(Guid idDocument);
        Task<IEnumerable<Document>> GetAllAsync(int pageNumber, int pageSize);
        void Update(Document document);
        void Delete(Document document);
        Task<IEnumerable<Document>> SearchAsync(string? autor, string? tipo, string? estado);
    }
}
    