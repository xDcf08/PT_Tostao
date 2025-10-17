namespace PruebaTostao.Exceptions
{
    public record ValidationError(
        string PropertyName,
        string ErrorMessage
        );
}
