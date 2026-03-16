using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PrimerCrudWebAPI.Data;
using PrimerCrudWebAPI.DTOs.Categorias;
using PrimerCrudWebAPI.Models;
using PrimerCrudWebAPI.Services.Interfaces;

namespace PrimerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet] //OBTENER TODAS LAS CATEGORIAS
        public async Task<IActionResult> ObtenerCategorias()
        {
            var categorias = await _service.ObtenerCategorias();
            return Ok(categorias);
        }


        [HttpGet("{id}")] //OBTENER UNA CATEGORIA POR ID
        public async Task<IActionResult> ObtenerCategoria(int id)
        {
            var categoria = await _service.ObtenerCategoria(id);
            if (categoria == null)
                return NotFound();
            return Ok(categoria);
        }


        [HttpPost] //CREAR UNA NUEVA CATEGORIA
        public async Task<IActionResult> CrearCategoria( [FromBody] CategoriaCreateDto dto)
        {
            var categoria = await _service.CrearCategoria(dto);
            return CreatedAtAction(nameof(ObtenerCategoria), new { id = categoria.Id }, categoria);
        }


        [HttpPut("{id}")] //EDITAR UN PRODUCTO POR ID
        public async Task<IActionResult> EditarCategoria(int id, [FromBody] CategoriaCreateDto dto)
        {
            var actualizado = await _service.EditarCategoria(id, dto);
            if (!actualizado)
                return NotFound();
            return NoContent();
        }


        [HttpDelete("{id}")] //ELIMINAR UN PRODUCTO POR ID
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var eliminado = await _service.EliminarCategoria(id);
            if (!eliminado)
                return NotFound();
            return NoContent();
        }
    }
}
