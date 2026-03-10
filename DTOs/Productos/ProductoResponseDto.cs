namespace PrimerCrudWebAPI.DTOs.Productos
{
    public class ProductoResponseDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        public string? CategoriaNombre { get; set; }
    }
}
