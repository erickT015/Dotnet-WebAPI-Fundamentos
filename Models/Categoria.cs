using System.ComponentModel.DataAnnotations;

namespace PrimerCrudWebAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
        public string? Descripcion { get; set; }

        public List<Producto>? Productos { get; set; }
    }
}
