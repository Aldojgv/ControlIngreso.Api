using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ControlIngresoApp.Interfaces;
namespace ControlIngresoApp.API
{
    [Route("[controller]")]
    public class ParametroController : ControllerBase
    {
        private readonly ILogger<ParametroController> _logger;
        private readonly IParametro _parametroRepository;
        public ParametroController(ILogger<ParametroController> logger, IParametro parametroRepository)
        {
            _parametroRepository = parametroRepository ?? throw new ArgumentNullException(nameof(parametroRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));



        }

        [HttpGet("{codParametro}")]
        public async Task<IActionResult> ObtenerValorParametroAsync(string codParametro)
        {
            try
            {
                // Aquí se llamaría al repositorio para obtener el valor del parámetro
                var valor = await _parametroRepository.ObtenerValorParametroAsync(codParametro);
                if (valor == null)
                {
                    return NotFound($"Parámetro con código '{codParametro}' no encontrado.");
                }
                return Ok(valor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el valor del parámetro");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}