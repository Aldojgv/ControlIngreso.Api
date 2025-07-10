using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ControlIngresoApp.Model;
using ControlIngresoApp.Interfaces;
using ControlIngresoApp.Shared.DTOs;
namespace ControlIngresoApp.API
{
    [Route("[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly ILogger<PersonaController> _logger;
        private readonly IPersona _personaRepository;
        public PersonaController(ILogger<PersonaController> logger, IPersona personaRepository)
        {
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }
        [HttpGet("personas")]
        public async Task<IActionResult> ObtenerPersonasAsync([FromQuery] string documento, [FromQuery] string nombre, [FromQuery] string apellido)
        {
            try
            {
                var personas = await _personaRepository.ObtenerPersonasDTOByDocumentoNombreApellidoAsync(documento, nombre, apellido);
                return Ok(personas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las personas");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPost("crear")]
        public async Task<IActionResult> CrearPersonaAsync([FromBody] PersonaCreateDTO personaCreateDTO)
        {
            if (personaCreateDTO == null)
            {
                return BadRequest("El objeto persona no puede ser nulo");
            }
            try
            {
                var id = await _personaRepository.CrearPersonaAsync(personaCreateDTO);
                return CreatedAtAction(nameof(ObtenerPersonasAsync), new { id }, personaCreateDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la persona");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarPersonaAsync([FromBody] PersonaUpdateDTO personaUpdateDTO)
        {
            if (personaUpdateDTO == null || personaUpdateDTO.Id <= 0)
            {
                return BadRequest("El objeto persona o su ID no pueden ser nulos o cero");
            }
            try
            {
                var result = await _personaRepository.ActualizarPersonaAsync(personaUpdateDTO);
                if (result > 0)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound("Persona no encontrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la persona");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPut("actualizarEstado")]
        public async Task<IActionResult> ActualizarEstadoPersonaAsync([FromBody] PersonaUpdateEstadoDTO personaUpdateEstadoDTO)
        {
            if (personaUpdateEstadoDTO == null || personaUpdateEstadoDTO.Id <= 0)
            {
                return BadRequest("El objeto persona o su ID no pueden ser nulos o cero");
            }
            try
            {
                var result = await _personaRepository.ActualizarEstadoPersonaAsync(personaUpdateEstadoDTO);
                if (result > 0)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound("Persona no encontrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado de la persona");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        

    }
}