using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PruebaTostao.Entities.DocumentEntity;

namespace PruebaTostao.Database.Configuration
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documentos");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Titulo)
                .IsRequired() 
                .HasMaxLength(255);

            builder.Property(d => d.Autor)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(d => d.Tipo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Estado)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.FechaRegistro)
                .IsRequired();
        }
    }
}
