using System.ComponentModel.DataAnnotations;

namespace PrimerCrudWebAPI.DTOs.Categorias
{
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
        public string? Descripcion { get; set; }
    }
}
