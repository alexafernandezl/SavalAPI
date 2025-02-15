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
    public class FormularioEncuestadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FormularioEncuestadoController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Obtener TODOS los formularios encuestados
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<FormularioEncuestado>>> GetTodosLosFormulariosEncuestados()
        {
            try
            {
                var formularios = await _context.FormularioEncuestados
                    .Include(fe => fe.Encuestado)
                    .Include(fe => fe.Formulario)
                    .ToListAsync();

                return Ok(formularios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Obtener SOLO los formularios encuestados ACTIVOS
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<FormularioEncuestado>>> GetFormulariosEncuestadosActivos()
        {
            try
            {
                var formularios = await _context.FormularioEncuestados
                    .Include(fe => fe.Encuestado)
                    .Include(fe => fe.Formulario)
                    .Where(fe => fe.Habilitado) // 🔹 Filtra solo los habilitados
                    .ToListAsync();

                return Ok(formularios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Obtener un FormularioEncuestado por su clave primaria compuesta
        [HttpGet("{idFormulario}/{idEncuestado}/{fecha}")]
        public async Task<ActionResult<FormularioEncuestado>> GetFormularioEncuestado(int idFormulario, string idEncuestado, DateTime fecha)
        {
            try
            {
                var formularioEncuestado = await _context.FormularioEncuestados
                    .Include(fe => fe.Encuestado)
                    .Include(fe => fe.Formulario)
                    .FirstOrDefaultAsync(fe => fe.IdFormulario == idFormulario && fe.IdEncuestado == idEncuestado && fe.Fecha == fecha);

                if (formularioEncuestado == null)
                {
                    return NotFound(new { message = "FormularioEncuestado no encontrado." });
                }

                return Ok(formularioEncuestado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Crear un nuevo FormularioEncuestado
        [HttpPost]
        public async Task<ActionResult<FormularioEncuestado>> PostFormularioEncuestado(FormularioEncuestado formularioEncuestado)
        {
            try
            {
                // Validar si el formulario y encuestado existen
                var formularioExistente = await _context.Formularios.FindAsync(formularioEncuestado.IdFormulario);
                if (formularioExistente == null)
                    return BadRequest(new { message = "El formulario especificado no existe." });

                var encuestadoExistente = await _context.Encuestados.FindAsync(formularioEncuestado.IdEncuestado);
                if (encuestadoExistente == null)
                    return BadRequest(new { message = "El encuestado especificado no existe." });

                // Calcular IMC antes de guardar
                if (formularioEncuestado.Altura > 0)
                {
                    formularioEncuestado.IMC = formularioEncuestado.Peso / (formularioEncuestado.Altura * formularioEncuestado.Altura);
                }
                else
                {
                    return BadRequest(new { message = "La altura debe ser mayor que 0." });
                }

                _context.FormularioEncuestados.Add(formularioEncuestado);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFormularioEncuestado),
                    new { idFormulario = formularioEncuestado.IdFormulario, idEncuestado = formularioEncuestado.IdEncuestado, fecha = formularioEncuestado.Fecha },
                    formularioEncuestado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Editar un FormularioEncuestado
        [HttpPut("{idFormulario}/{idEncuestado}/{fecha}")]
        public async Task<IActionResult> PutFormularioEncuestado(int idFormulario, string idEncuestado, DateTime fecha, FormularioEncuestado formularioEncuestado)
        {
            try
            {
                if (idFormulario != formularioEncuestado.IdFormulario || idEncuestado != formularioEncuestado.IdEncuestado || fecha != formularioEncuestado.Fecha)
                {
                    return BadRequest(new { message = "Los identificadores no coinciden." });
                }

                // Recalcular el IMC antes de guardar
                if (formularioEncuestado.Altura > 0)
                {
                    formularioEncuestado.IMC = formularioEncuestado.Peso / (formularioEncuestado.Altura * formularioEncuestado.Altura);
                }
                else
                {
                    return BadRequest(new { message = "La altura debe ser mayor que 0." });
                }

                _context.Entry(formularioEncuestado).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { message = "FormularioEncuestado actualizado con éxito." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FormularioEncuestados.Any(fe => fe.IdFormulario == idFormulario && fe.IdEncuestado == idEncuestado && fe.Fecha == fecha))
                {
                    return NotFound(new { message = "FormularioEncuestado no encontrado." });
                }
                return StatusCode(500, new { message = "Error de concurrencia en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // ✅ Soft Delete (Deshabilitar FormularioEncuestado)
        [HttpDelete("{idFormulario}/{idEncuestado}/{fecha}")]
        public async Task<IActionResult> SoftDeleteFormularioEncuestado(int idFormulario, string idEncuestado, DateTime fecha)
        {
            try
            {
                var formularioEncuestado = await _context.FormularioEncuestados
                    .FirstOrDefaultAsync(fe => fe.IdFormulario == idFormulario && fe.IdEncuestado == idEncuestado && fe.Fecha == fecha);

                if (formularioEncuestado == null)
                    return NotFound(new { message = "FormularioEncuestado no encontrado." });

                formularioEncuestado.Habilitado = false; // 🔹 Soft delete
                await _context.SaveChangesAsync();

                return Ok(new { message = "FormularioEncuestado deshabilitado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
