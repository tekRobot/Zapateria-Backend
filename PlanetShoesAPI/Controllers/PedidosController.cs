using Microsoft.AspNetCore.Mvc;
using PlanetShoesAPI.Models.DTOS;
using PlanetShoesAPI.Services;

namespace PlanetShoesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosService _service;

        public PedidosController(IPedidosService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoPartidaDTO pedidoPar)
        {
            if (pedidoPar.Cantidad <= 0)
                return BadRequest(new APIResponse<string> { Success = false, Message = "La cantidad debe ser mayor a 0" });

            var result = await _service.CrearPedidoPartidaAsync(pedidoPar);
            if (!result.Success) return StatusCode(500, result);

            return CreatedAtAction(nameof(Create), new { id = result.Data }, result);
        }

        [HttpPatch("{id}/surtir")]
        public async Task<IActionResult> Surtir(int id)
        {
            var result = await _service.SurtirPedidoAsync(id);
            if (!result.Success) return result.Data == false && result.Message.Contains("No se encontró")
                ? NotFound(result)
                : StatusCode(500, result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetalle()
        {
            var result = await _service.GetDetallesPedidosAsync();
            return result.Success ? Ok(result) : StatusCode(500, result);
        }
    }
}
