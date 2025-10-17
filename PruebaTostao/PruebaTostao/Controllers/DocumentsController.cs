using Microsoft.AspNetCore.Mvc;
using PruebaTostao.DTOs;
using PruebaTostao.Services.Documents;

namespace PruebaTostao.Controllers
{
    [Route("api/v1/documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentDTO createDocument)
        {
            var result = await _documentService.CreateDocumentAsync(createDocument);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _documentService.GetAllDocumentsAsync(pageNumber, pageSize);
            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _documentService.GetDocumentByIdAsync(id);
            if (result.Value is null)
            {
                return NotFound();
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateDocumentDTO updateDto)
        {
            var result = await _documentService.UpdateDocumentAsync(id, updateDto);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return NoContent();
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Search(
            [FromQuery] string? autor,
            [FromQuery] string? tipo,
            [FromQuery] string? estado)
        {
            var result = await _documentService.SearchDocumentsAsync(autor, tipo, estado);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}
