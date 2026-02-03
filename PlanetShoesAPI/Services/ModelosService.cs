using PlanetShoesAPI.Data;
using PlanetShoesAPI.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace PlanetShoesAPI.Services
{
    public interface IModelosService
    {
        Task<APIResponse<List<ModeloWebDTO>>> GetModelosAsync(string? estilo);
    }

    public class ModelosService : IModelosService
    {
        private readonly ApplicationDbContext _context;

        public ModelosService(ApplicationDbContext context) => _context = context;

        public async Task<APIResponse<List<ModeloWebDTO>>> GetModelosAsync(string? estilo)
        {
            var res = new APIResponse<List<ModeloWebDTO>>();
            try
            {
                var query = _context.ModelosWeb.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(estilo))
                {
                    query = query.Where(m => m.Estilo != null && m.Estilo.Equals(estilo));
                }

                var datos = await query
                    .Select(d => new ModeloWebDTO
                    {
                        Id = d.Id ?? "",
                        Talla = d.Talla!.Trim() ?? "",
                        Existencia = Convert.ToDecimal(d.Existencia),
                        Descripcion = d.Descripcion!.Trim() ?? "",
                        Precio1 = Convert.ToDecimal(d.Precio1),
                        Estilo = d.Estilo!.Trim() ?? "",
                        Rango = d.Rango!.Trim() ?? "",
                        Foto = d.Foto ?? "",
                        Marca = d.Marca!.Trim() ?? "",
                        Color = d.Color!.Trim() ?? "",
                        Material = d.Material!.Trim() ?? "",
                        Genero = d.Genero!.Trim() ?? ""
                    })
                    .ToListAsync(); 

                res.Data = datos;
                res.Success = true;
                res.Message = $"Consulta exitosa. Se encontraron {datos.Count} registros.";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error al obtener los modelos: " + ex.Message;
            }

            return res;
        }
    }
}
