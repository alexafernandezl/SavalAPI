using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavalAPI.Data;
using SavalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SavalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncuestadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EncuestadoController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Obtener TODOS los encuestados (activos e inactivos)
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<Encuestado>>> GetTodosEncuestados()
        {
            try
            {
                var encuestados = await _context.Encuestados.ToListAsync();
                return Ok(encuestados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Obtener SOLO los encuestados ACTIVOS
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Encuestado>>> GetEncuestadosActivos()
        {
            try
            {
                var encuestados = await _context.Encuestados
                    .Where(e => e.Habilitado) // 🔹 Filtra solo habilitados
                    .ToListAsync();

                return Ok(encuestados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Obtener un encuestado por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Encuestado>> GetEncuestado(string id)
        {
            try
            {
                var encuestado = await _context.Encuestados
                    .FirstOrDefaultAsync(e => e.Identificacion == id);

                if (encuestado == null)
                    return NotFound(new { message = "Encuestado no encontrado." });

                return Ok(encuestado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Crear un nuevo encuestado
        [HttpPost]
        public async Task<ActionResult<Encuestado>> PostEncuestado(Encuestado encuestado)
        {
            try
            {
                _context.Encuestados.Add(encuestado);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEncuestado), new { id = encuestado.Identificacion }, encuestado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Editar un encuestado
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEncuestado(string id, Encuestado encuestado)
        {
            try
            {
                if (id != encuestado.Identificacion)
                    return BadRequest(new { message = "El ID no coincide." });

                _context.Entry(encuestado).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Encuestado actualizado con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Encuestados.Any(e => e.Identificacion == id))
                    return NotFound(new { message = "Encuestado no encontrado." });

                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Soft Delete (Deshabilitar Encuestado)
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteEncuestado(string id)
        {
            try
            {
                var encuestado = await _context.Encuestados.FindAsync(id);

                if (encuestado == null)
                    return NotFound(new { message = "Encuestado no encontrado." });

                encuestado.Habilitado = false; // 🔹 Soft delete
                await _context.SaveChangesAsync();

                return Ok(new { message = "Encuestado deshabilitado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
