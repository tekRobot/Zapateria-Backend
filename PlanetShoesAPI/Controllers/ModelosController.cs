using Microsoft.AspNetCore.Mvc;
using PlanetShoesAPI.Services;

namespace PlanetShoesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelosController : ControllerBase
    {
        private readonly IModelosService _service;
        private readonly IModelosSugeridosService _sugeridosService;

        public ModelosController(IModelosService service, IModelosSugeridosService sugeridosService)
        {
            _service = service;
            _sugeridosService = sugeridosService;
        }

        [HttpGet("estilo/{estilo}")]
        public async Task<IActionResult> GetByEstilo(string estilo)
        {
            var result = await _service.GetModelosAsync(estilo);
            return result.Success ? Ok(result) : StatusCode(500, result);
        }

        [HttpGet("estilo/{estilo}/sugeridos")]
        public async Task<IActionResult> GetSugeridosByEstilo(string estilo)
        {
            var result = await _sugeridosService.GetSugeridosByEstiloAsync(estilo);
            return result.Success ? Ok(result) : StatusCode(500, result);
        }
    }
}
