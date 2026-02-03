namespace PlanetShoesAPI.Models.DTOS
{
    public class PedidoPartidaDTO
    {
        public string Articulo { get; set; } = null!;
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public string Usuario { get; set; } = null!;
    }
}
