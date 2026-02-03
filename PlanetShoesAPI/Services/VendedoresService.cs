using PlanetShoesAPI.Data;
using PlanetShoesAPI.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace PlanetShoesAPI.Services
{
    public interface IVendedoresService
    {
        Task<APIResponse<List<VendedorDTO>>> GetVendedoresAsync();
    }

    public class VendedoresService : IVendedoresService
    {
        private readonly ApplicationDbContext _context;

        public VendedoresService(ApplicationDbContext context) => _context = context;

        public async Task<APIResponse<List<VendedorDTO>>> GetVendedoresAsync()
        {
            var res = new APIResponse<List<VendedorDTO>>();
            try
            {
                var datos = await _context.Vendedores.ToListAsync();
                res.Data = datos.Select(d => new VendedorDTO
                {
                    Id = d.Id,
                    Nombre = d.Nombre
                }).ToList();
                res.Success = true;
                res.Message = "Consulta exitosa";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error en servidor";
            }
            return res;
        }
    }
}
