using PlanetShoesAPI.Data;
using PlanetShoesAPI.Models.DTOS;
using Microsoft.EntityFrameworkCore;
using PlanetShoesAPI.Models.Entities;

namespace PlanetShoesAPI.Services
{
    public interface IPedidosService
    {
        Task<APIResponse<int>> CrearPedidoPartidaAsync(PedidoPartidaDTO dto);
        Task<APIResponse<List<PedidoDetalleDTO>>> GetDetallesPedidosAsync();
        Task<APIResponse<bool>> SurtirPedidoAsync(int pedidoId);
    }

    public class PedidosService : IPedidosService
    {
        private readonly ApplicationDbContext _context;
        
        public PedidosService(ApplicationDbContext context) => _context = context;
        
        public async Task<APIResponse<int>> CrearPedidoPartidaAsync(PedidoPartidaDTO dto)
        {
            var res = new APIResponse<int>();
            try
            {
                // Creamos la Entidad "Bonita" a partir del DTO
                var nuevaPartida = new PedidoPartida
                {
                    Articulo = dto.Articulo,
                    Cantidad = 1,//dto.Cantidad,
                    Surtido = 0,//dto.Surtido,
                    PorSurtir = 0,//dto.Cantidad,
                    Precio = dto.Precio,
                    UsuarioId = dto.Usuario,
                    UsuarioFecha = DateTime.Now.Date,
                    UsuarioHora = DateTime.Now.ToString("HH:mm:ss")
                };

                _context.PedidoPartidas.Add(nuevaPartida);
                await _context.SaveChangesAsync();

                res.Data = nuevaPartida.PedidoId; // Retornamos el ID generado
                res.Success = true;
                res.Message = "Partida de pedido creada correctamente.";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "No se pudo guardar la partida.";
            }
        
            return res;
        }
        public async Task<APIResponse<bool>> SurtirPedidoAsync(int pedidoId)
        {
            var res = new APIResponse<bool>();
            try
            {
                var afectados = await _context.PedidoPartidas
                    .Where(p => p.PedidoId == pedidoId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Surtido, 1));

                if (afectados == 0)
                {
                    res.Success = false;
                    res.Message = $"No se encontró el pedido con ID {pedidoId}.";
                    return res;
                }

                res.Data = true;
                res.Success = true;
                res.Message = "Pedido marcado como surtido correctamente.";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error al surtir el pedido.";
            }
            return res;
        }

        public async Task<APIResponse<List<PedidoDetalleDTO>>> GetDetallesPedidosAsync()
        {
            var res = new APIResponse<List<PedidoDetalleDTO>>();
            try
            {
                var consulta = await (from p in _context.PedidoPartidas
                                      join v in _context.Vendedores on p.UsuarioId equals v.Id
                                      join m in _context.ModelosWeb on p.Articulo equals m.Id
                                      where p.Surtido == 0
                                      select new PedidoDetalleDTO
                                      {
                                          Id = p.PedidoId,
                                          Articulo = p.Articulo,
                                          Surtido = p.Surtido,
                                          PorSurtir = p.PorSurtir,
                                          Vendedor = new VendedorDTO { Id = p.UsuarioId ?? string.Empty, Nombre = v.Nombre },
                                          ModeloDescripcion = m.Descripcion ?? string.Empty, // Usando los nombres de tus entidades
                                          Talla = m.Talla ?? string.Empty,
                                          Estilo = m.Estilo ?? string.Empty,
                                          Foto = m.Foto,
                                          Marca = m.Marca!.Trim() ?? string.Empty,
                                          Color = m.Color!.Trim(),
                                          Material = m.Material!.Trim(),
                                          Genero = m.Genero ?? string.Empty
                                      }).ToListAsync();

                res.Data = consulta;
                res.Success = true;
                res.Message = "Consulta de pedidos realizada con éxito";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Error al obtener los detalles de pedidos.";
            }
            return res;
        }
    }
}
