using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ControlIngresoApp.Interfaces;
using ControlIngresoApp.Model;
using ControlIngresoApp.Shared.DTOs;
namespace ControlIngresoApp.API
{
    [Route("[controller]")]
    public class RegistroController : Controller
    {
        private readonly ILogger<RegistroController> _logger;
        private readonly IRegistro _registroRepository;

        public RegistroController(ILogger<RegistroController> logger, IRegistro registroRepository)
        {
            _registroRepository = registroRepository ?? throw new ArgumentNullException(nameof(registroRepository));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }
        [HttpGet("{fechaIngreso}")]
        public async Task<IActionResult> ObtenerRegistrosPorFechaAsync(string fechaIngreso)
        {
            try
            {
                var registros = await _registroRepository.ObtenerRegistrosByFecha(fechaIngreso);
                return Ok(registros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los registros por fecha");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CrearRegistroAsync([FromBody] RegistroCreateDTO registro)
        {
            if (registro == null)
            {
                return BadRequest("El registro no puede ser nulo");
            }

            try
            {
                var nuevoRegistro = await _registroRepository.CrearRegistroAsync(registro);
                if (nuevoRegistro <= 0)
                {
                    return BadRequest("No se pudo crear el registro");
                }
                else
                {
                    // Aquí podrías retornar el registro creado o simplemente un mensaje de éxito
                    return nuevoRegistro > 0
                        ? Ok(nuevoRegistro)
                        : BadRequest("No se pudo crear el registro");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarRegistroAsync([FromBody] RegistroUpdateDTO registro)
        {
            if (registro == null)
            {
                return BadRequest("El registro no puede ser nulo");
            }
            try
            {
                var resultado = await _registroRepository.ActualizarRegistroAsync(registro);
                if (resultado <= 0)
                {
                    return BadRequest("No se pudo actualizar el registro");
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("estado")]
        public async Task<IActionResult> ActualizarEstadoRegistroAsync([FromBody] RegistroUpdateEstadoDTO registro)
        {
            if (registro == null)
            {
                return BadRequest("El registro no puede ser nulo");
            }
            try
            {
                var resultado = await _registroRepository.ActualizarEstadoRegistroAsync(registro);
                if (resultado <= 0)
                {
                    return BadRequest("No se pudo actualizar el estado del registro");
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado del registro");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}

