using Microsoft.EntityFrameworkCore;
using PlanetShoesAPI.Models.Entities;

namespace PlanetShoesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ModeloWeb> ModelosWeb { get; set; }
        public DbSet<ModeloSugerido> ModelosSugeridos { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<PedidoPartida> PedidoPartidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModeloWeb>(e => {
                e.HasNoKey();
                e.ToView("ModelosWeb"); // Nombre de la vista en SQL
                e.Property(p => p.Id).HasColumnName("ARTICULO");
                e.Property(p => p.Talla).HasColumnName("talla");
                e.Property(p => p.Existencia).HasColumnName("EXISTENCIA");
                e.Property(p => p.Descripcion).HasColumnName("Descrip");
                e.Property(p => p.Precio1).HasColumnName("precio1");
                e.Property(p => p.Estilo).HasColumnName("estilo");
                e.Property(p => p.Rango).HasColumnName("rango");
                e.Property(p => p.Foto).HasColumnName("foto");
                e.Property(p => p.Marca).HasColumnName("Marca");
                e.Property(p => p.Color).HasColumnName("Color");
                e.Property(p => p.Material).HasColumnName("Material");
                e.Property(p => p.Genero).HasColumnName("Genero");
            });

            modelBuilder.Entity<ModeloSugerido>(e => {
                e.HasNoKey();
                e.ToView("ModelosSugeridos");
                e.Property(p => p.Estilo).HasColumnName("Estilo");
                e.Property(p => p.EstiloSugerido).HasColumnName("EstiloSug");
                e.Property(p => p.Marca).HasColumnName("marca");
                e.Property(p => p.Genero).HasColumnName("Genero");
                e.Property(p => p.Material).HasColumnName("material");
                e.Property(p => p.Linea).HasColumnName("linea");
                e.Property(p => p.Rango).HasColumnName("rango");
                e.Property(p => p.Forma).HasColumnName("forma");
                e.Property(p => p.Uso).HasColumnName("Uso");
                e.Property(p => p.Modelo).HasColumnName("modelo");
                e.Property(p => p.FotoSugerida).HasColumnName("FotoSug");
            });

            modelBuilder.Entity<Vendedor>(e => {
                e.HasKey(v => v.Id);
                e.ToTable("vends"); // Nombre de la tabla en SQL
                e.Property(v => v.Id).HasColumnName("Vend");
                e.Property(v => v.Nombre).HasColumnName("Nombre");
            });

            modelBuilder.Entity<PedidoPartida>(e =>
            {
                e.ToTable("pedpar");

                // Clave primaria
                e.HasKey(p => p.PedidoId);

                // Mapeo de columnas: Propiedad C# -> Columna SQL
                e.Property(p => p.PedidoId).HasColumnName("pedido");
                e.Property(p => p.Articulo).HasColumnName("ARTICULO").HasMaxLength(30);
                e.Property(p => p.Cantidad).HasColumnName("CANTIDAD");
                e.Property(p => p.Surtido).HasColumnName("SURTIDO");
                e.Property(p => p.PorSurtir).HasColumnName("POR_SURT");
                e.Property(p => p.Precio).HasColumnName("PRECIO");
                e.Property(p => p.UsuarioId).HasColumnName("Usuario").HasMaxLength(10);
                e.Property(p => p.UsuarioFecha).HasColumnName("UsuFecha");
                e.Property(p => p.UsuarioHora).HasColumnName("UsuHora").HasMaxLength(8);
            });
        }
    }
}
