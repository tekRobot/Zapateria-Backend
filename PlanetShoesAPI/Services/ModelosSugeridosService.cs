using PlanetShoesAPI.Data;
using PlanetShoesAPI.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace PlanetShoesAPI.Services
{
    public interface IModelosSugeridosService
    {
        Task<APIResponse<List<ModeloSugeridoDTO>>> GetSugeridosByEstiloAsync(string estilo);
    }

    public class ModelosSugeridosService : IModelosSugeridosService
    {
        private readonly ApplicationDbContext _context;

        public ModelosSugeridosService(ApplicationDbContext context) => _context = context;

        public async Task<APIResponse<List<ModeloSugeridoDTO>>> GetSugeridosByEstiloAsync(string estilo)
        {
            var res = new APIResponse<List<ModeloSugeridoDTO>>();
            try
            {
                var consulta = await _context.ModelosSugeridos
                    .AsNoTracking()
                    .Where(m => m.Estilo != null && m.Estilo.Trim().Equals(estilo.Trim()))
                    .Select(m => new ModeloSugeridoDTO
                    {
                        Estilo = m.Estilo,
                        EstiloSugerido = m.EstiloSugerido,
                        Marca = m.Marca,
                        Genero = m.Genero!.Trim(),
                        Material = m.Material,
                        Linea = m.Linea!.Trim(),
                        Rango = m.Rango,
                        Forma = m.Forma ?? "",
                        Uso = m.Uso ?? "",
                        Modelo = m.Modelo,
                        FotoSugerida = m.FotoSugerida
                    })
                    .ToListAsync();

                res.Data = consulta;
                res.Success = true;
                res.Message = $"Se encontraron {consulta.Count} modelos sugeridos para el estilo '{estilo}'.";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error al obtener los modelos sugeridos.";
            }
            return res;
        }
    }
}
