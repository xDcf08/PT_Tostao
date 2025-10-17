namespace PruebaTostao.Entities.Abstractions.Errors
{
    public class DocumentError
    {
        public static Error NotFound => new("DocumentError.NotFound", "Document not found by id");
    }
}
