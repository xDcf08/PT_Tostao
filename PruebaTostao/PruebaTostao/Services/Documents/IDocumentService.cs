using PruebaTostao.DTOs;
using PruebaTostao.Entities.Abstractions;

namespace PruebaTostao.Services.Documents
{
    public interface IDocumentService
    {
        Task<Result<IEnumerable<GetDocumentDTO>>> GetAllDocumentsAsync(int pageNumber, int pageSize);
        Task<Result<GetDocumentDTO?>> GetDocumentByIdAsync(Guid id);
        Task<Result<GetDocumentDTO>> CreateDocumentAsync(CreateDocumentDTO createDocument);
        Task<Result> UpdateDocumentAsync(Guid id, UpdateDocumentDTO document);
        Task<Result> DeleteDocumentAsync(Guid id);
        Task<Result<IEnumerable<GetDocumentDTO>>> SearchDocumentsAsync(string? autor, string? tipo, string? estado);
    }
}
