using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ControlIngresoApp.Interfaces;
using ControlIngresoApp.Model;
namespace ControlIngresoApp.API
{
    [Route("[controller]")]
    public class PaisController : Controller
    {
        private readonly ILogger<PaisController> _logger;

        private readonly IPais _paisRepository;  
        public PaisController(ILogger<PaisController> logger , IPais paisRepository)
        {
            _paisRepository = paisRepository ?? throw new ArgumentNullException(nameof(paisRepository));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        [HttpGet("paises")]
        public async Task<IActionResult> ObtenerPaisesAsync()
        {
            try
            {
                var paises = await _paisRepository.ObtenerPaisesAsync();
                return Ok(paises);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los países");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}