using Microsoft.EntityFrameworkCore;
using PruebaTostao.Database;
using PruebaTostao.Entities.DocumentEntity;

namespace PruebaTostao.Repository
{
    public class DocumentRepositoryImp : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepositoryImp(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
        }

        public void Delete(Document document)
        {
            _context.Documents.Remove(document);
        }

        public async Task<IEnumerable<Document>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Documents
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(Guid idDocument)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == idDocument)!;
        }

        public void Update(Document document)
        {
            _context.Documents.Update(document);
        }

        public async Task<IEnumerable<Document>> SearchAsync(string? autor, string? tipo, string? estado)
        {
            var query = _context.Documents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(autor))
            {
                query = query.Where(d => d.Autor.Contains(autor));
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                query = query.Where(d => d.Tipo == tipo);
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                query = query.Where(d => d.Estado == estado);
            }

            return await query.ToListAsync();
        }
    }
}
