namespace PlanetShoesAPI.Models.DTOS;

public class PedidoDetalleDTO
{
    public string Articulo { get; set; } = null!;
    public double Surtido { get; set; }
    public double PorSurtir { get; set; }
    public VendedorDTO? Vendedor { get; set; }
    public string ModeloDescripcion { get; set; } = null!;
    public string Talla { get; set; } = null!;
    public string Estilo { get; set; } = null!;
    public string? Foto { get; set; }
    public string Marca { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Material { get; set; } = null!;
    public string Genero { get; set; } = null!;
}