using PruebaTostao.DTOs;
using PruebaTostao.Entities.Abstractions;
using PruebaTostao.Entities.Abstractions.Errors;
using PruebaTostao.Entities.DocumentEntity;
using PruebaTostao.Repository;

namespace PruebaTostao.Services.Documents
{
    public class DocumentServiceImp : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentServiceImp(IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
        {
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetDocumentDTO>> CreateDocumentAsync(CreateDocumentDTO createDocument)
        {
            var document = Document.Create(Guid.NewGuid(),
                createDocument.Titulo,
                createDocument.Autor,
                createDocument.Tipo,
                "REGISTRADO",
                DateTime.UtcNow);

            await _documentRepository.CreateAsync(document);
            await _unitOfWork.SaveChangesAsync();

            var dto = new GetDocumentDTO(document.Id, document.Titulo!, document.Autor!, document.Tipo!, document.Estado!);

            return Result.Success(dto);
        }

        public async Task<Result> DeleteDocumentAsync(Guid id)
        {
            var existingDoc = await _documentRepository.GetByIdAsync(id);
            if (existingDoc is null)
            {
                return Result.Failure(DocumentError.NotFound);
            }

            _documentRepository.Delete(existingDoc);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<GetDocumentDTO>>> GetAllDocumentsAsync(int pageNumber, int pageSize)
        {
            var documents = await _documentRepository.GetAllAsync(pageNumber, pageSize);

            var res = documents.Select(d => new GetDocumentDTO(
                d.Id,
                d.Titulo!,
                d.Autor!,
                d.Tipo!,
                d.Estado!
            )).ToList();

            return Result.Success<IEnumerable<GetDocumentDTO>>(res);
        }

        public async Task<Result<GetDocumentDTO?>> GetDocumentByIdAsync(Guid id)
        {
            var document = await _documentRepository.GetByIdAsync(id);

            if (document is null)
            {
                return Result.Success<GetDocumentDTO?>(null);
            }

            var res = new GetDocumentDTO(document!.Id, document.Titulo!, document.Autor!, document.Tipo!, document.Estado!);
            return Result.Success<GetDocumentDTO?>(res);
        }

        public async Task<Result> UpdateDocumentAsync(Guid id, UpdateDocumentDTO document)
        {
            var existingDoc = await _documentRepository.GetByIdAsync(id);

            if (existingDoc is null)
            {
                return Result.Failure(DocumentError.NotFound);
            }

            existingDoc.Update(document.Titulo!, document.Autor!, document.Tipo!, document.Estado!);

            _documentRepository.Update(existingDoc);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<GetDocumentDTO>>> SearchDocumentsAsync(string? autor, string? tipo, string? estado)
        {
            var documents = await _documentRepository.SearchAsync(autor, tipo, estado);

            var res = documents.Select(d => new GetDocumentDTO(
                d.Id,
                d.Titulo!,
                d.Autor!,
                d.Tipo!,
                d.Estado!
            )).ToList();

            return Result.Success<IEnumerable<GetDocumentDTO>>(res);
        }
    }
}
