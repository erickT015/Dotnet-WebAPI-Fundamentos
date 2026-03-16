namespace PrimerCrudWebAPI.DTOs.Productos
{
    public class ProductoQueryDto
    {
        public string? Search { get; set; }

        public int? CategoriaId { get; set; }

        public decimal? PrecioMin { get; set; }

        public decimal? PrecioMax { get; set; }

        public string? SortBy { get; set; }

        public string? SortDirection { get; set; } = "asc";

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}
