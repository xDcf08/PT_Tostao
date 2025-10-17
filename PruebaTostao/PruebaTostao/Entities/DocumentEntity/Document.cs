namespace PruebaTostao.Entities.DocumentEntity
{
    public class Document
    {
        private Document(
            Guid id,
            string titulo,
            string autor,
            string tipo,
            string estado,
            DateTime fechaRegistro
            )
        {
            Id = id;
            Titulo = titulo;
            Autor = autor;
            Tipo = tipo;
            Estado = estado;
            FechaRegistro = fechaRegistro;
        }

        public Guid Id { get; private set; }
        public string? Titulo { get; private set; }
        public string? Autor { get; private set; }
        public string? Tipo { get; private set; }
        public string? Estado { get; private set; }
        public DateTime FechaRegistro { get; private set; }

        public static Document Create(Guid id,
            string titulo,
            string autor,
            string tipo,
            string estado,
            DateTime fechaRegistro)
        {
            var document = new Document(id, titulo, autor, tipo, estado, fechaRegistro);
            return document;
        }

        public void Update(string titulo, string autor, string tipo, string estado)
        {
            Titulo = titulo;
            Autor = autor;
            Tipo = tipo;
            Estado = estado;
        }
    }
}
