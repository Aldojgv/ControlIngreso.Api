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
    public class EmpresaController : ControllerBase
    {
        private readonly ILogger<EmpresaController> _logger;
        private readonly IEmpresa _empresaRepository;

        public EmpresaController(ILogger<EmpresaController> logger, IEmpresa empresaRepository)
        {
            _empresaRepository = empresaRepository ?? throw new ArgumentNullException(nameof(empresaRepository));
            _logger = logger;
        }
        [HttpGet("empresas")]
        public async Task<IActionResult> GetAllEmpresasAsync()
        {
            try
            {
                var empresas = await _empresaRepository.ObtenerEmpresaAsync();
                return Ok(empresas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las empresas");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPost("crear")]
        public async Task<IActionResult> CrearEmpresaAsync([FromBody] EmpresaCreateDTO empresa)
        {
            if (empresa == null)
            {
                return BadRequest("La empresa no puede ser nula");
            }
            try
            {
                var resultado = await _empresaRepository.CrearEmpresaAsync(empresa);
                if (resultado > 0)
                {
                    return Ok("Empresa creada exitosamente");
                }
                return BadRequest("No se pudo crear la empresa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la empresa");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarEmpresaAsync([FromBody] EmpresaUpdateDTO empresa)
        {
            if (empresa == null)
            {
                return BadRequest("La empresa no puede ser nula");
            }
            try
            {
                var resultado = await _empresaRepository.ActualizarEmpresaAsync(empresa);
                if (resultado > 0)
                {
                    return Ok("Empresa actualizada exitosamente");
                }
                return BadRequest("No se pudo actualizar la empresa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la empresa");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPut("estado")]
        public async Task<IActionResult> ActualizarEstadoEmpresaAsync([FromBody] EmpresaUpdateEstadoDTO empresa)
        {
            if (empresa == null)
            {
                return BadRequest("La empresa no puede ser nula");
            }
            try
            {
                var resultado = await _empresaRepository.ActualizarEstadoEmpresaAsync(empresa);
                if (resultado > 0)
                {
                    return Ok("Estado de la empresa actualizado exitosamente");
                }
                return BadRequest("No se pudo actualizar el estado de la empresa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado de la empresa");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
