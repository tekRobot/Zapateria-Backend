namespace PlanetShoesAPI.Models.Entities
{
    public class PedidoPartida
    {
        public int PedidoId { get; set; }
        public required string Articulo { get; set; }
        public double Cantidad { get; set; }
        public double Surtido { get; set; }
        public double PorSurtir { get; set; }
        public double Precio { get; set; }
        public string? UsuarioId { get; set; }
        public DateTime? UsuarioFecha { get; set; }
        public string? UsuarioHora { get; set; }
    }
}
