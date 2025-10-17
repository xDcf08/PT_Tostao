
using PruebaTostao.Entities.Abstractions;
using PruebaTostao.Repository;

namespace PruebaTostao.BackgroundServices
{
    public class ArchiveOldDocumentsService : BackgroundService
    {
        private readonly ILogger<ArchiveOldDocumentsService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ArchiveOldDocumentsService(ILogger<ArchiveOldDocumentsService> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de archivo automático iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var documentRepository = scope.ServiceProvider.GetRequiredService<IDocumentRepository>();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        await ArchiveDocuments(documentRepository, unitOfWork);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
            }
        }

        private async Task ArchiveDocuments(IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
        {
            _logger.LogInformation("Ejecutando tarea de archivado de documentos antiguos...");
            var ninetyDaysAgo = DateTime.UtcNow.AddDays(-90);

            var documentsToArchive = await documentRepository.SearchAsync(autor: null, tipo: null, estado: "PENDIENTE");

            var oldDocuments = documentsToArchive.Where(d => d.FechaRegistro <= ninetyDaysAgo).ToList();

            if (!oldDocuments.Any())
            {
                _logger.LogInformation("No se encontraron documentos para archivar");
                return;
            }

            foreach (var document in oldDocuments) {
                document.Update(document.Titulo!, document.Autor!, document.Tipo!, "ARCHIVADO");
                documentRepository.Update(document);
                _logger.LogInformation("Documento con ID {DocumentId} preparado para ser archivado.", document.Id);
            }

            await unitOfWork.SaveChangesAsync();
            _logger.LogInformation("{Count} documentos fueron archivados exitosamente.", oldDocuments.Count);
        }
    }
}
