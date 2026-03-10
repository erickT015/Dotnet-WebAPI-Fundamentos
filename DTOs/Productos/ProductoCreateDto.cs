using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimerCrudWebAPI.DTOs.Productos
{
    public class ProductoCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres.")]
        public string Descripcion { get; set; }


        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Precio { get; set; }


        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int? CategoriaId { get; set; }
    }
}
