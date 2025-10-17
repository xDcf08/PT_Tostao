namespace PruebaTostao.DTOs
{
    public record GetDocumentDTO(Guid Id, string Titulo, string Autor, string Tipo, string Estado);
}
