using Microsoft.AspNetCore.Mvc;
using PlanetShoesAPI.Services;

namespace PlanetShoesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelosController : ControllerBase
    {
        private readonly IModelosService _service;
        public ModelosController(IModelosService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? estilo = null)
        {
            var result = await _service.GetModelosAsync(estilo);
            return result.Success ? Ok(result) : StatusCode(500, result);
        }
    }
}
