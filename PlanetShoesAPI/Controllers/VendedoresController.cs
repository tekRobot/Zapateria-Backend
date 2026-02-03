using Microsoft.AspNetCore.Mvc;
using PlanetShoesAPI.Services;

namespace PlanetShoesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendedoresController : ControllerBase
    {
        private readonly IVendedoresService _service;
        public VendedoresController(IVendedoresService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetVendedoresAsync();
            return result.Success ? Ok(result) : StatusCode(500, result);
        }
    }
}
