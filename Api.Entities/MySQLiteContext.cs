using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Api.Entities;

namespace Api.Entities
{
    public class MySQLiteContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MySQLiteContext(DbContextOptions<MySQLiteContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<CategoriaLog> CategoriaLog { get; set; }
        public DbSet<Cobrador> Cobrador { get; set; }
        public DbSet<Concepto> Concepto { get; set; }
        public DbSet<Cuota> Cuota { get; set; }
        public DbSet<Folio> Folio { get; set; }
        public DbSet<Gremio> Gremio { get; set; }
        public DbSet<Lider> Lider { get; set; }
        public DbSet<Padron> Padron { get; set; }
        public DbSet<Recaudacion> Recaudacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id_categoria);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<CategoriaLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_categoria });
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_categoria).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Cobrador>(entity =>
            {
                entity.HasKey(e => e.Id_cobrador);
                entity.Property(e => e.Id_cobrador).IsRequired();
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.A_paterno).IsRequired();
                entity.Property(e => e.A_materno).IsRequired();
                entity.Property(e => e.Telefono);
                entity.Property(e => e.Email);
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<CobradorLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_cobrador });
                entity.Property(e => e.Id_movimiento).IsRequired(); 
                entity.Property(e => e.Id_cobrador).IsRequired();
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.A_paterno).IsRequired();
                entity.Property(e => e.A_materno).IsRequired();
                entity.Property(e => e.Telefono);
                entity.Property(e => e.Email);
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.HasKey(e => e.Id_concepto);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<ConceptoLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_concepto});
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_concepto).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Cuota>(entity =>
            {
                entity.HasKey(e => e.Id_cuota);
                entity.Property(e => e.Monto).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<CuotaLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_cuota });
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_cuota).IsRequired();
                entity.Property(e => e.Monto).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Folio>(entity =>
            {
                entity.HasKey(e => e.Id_folio);
                entity.Property(e => e.N_Folio).IsRequired();
                entity.Property(e => e.Fecha).IsRequired();
            });

            modelBuilder.Entity<Gremio>(entity =>
            {
                entity.HasKey(e => e.Id_gremio);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Id_lider).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<GremioLog>(entity =>
            {
                entity.HasKey(e => new { e.id_movimiento, e.id_gremio });
                entity.Property(e => e.id_movimiento).IsRequired();
                entity.Property(e => e.id_gremio).IsRequired();
                entity.Property(e => e.descripcion).IsRequired();
                entity.Property(e => e.id_lider).IsRequired();
                entity.Property(e => e.estado).IsRequired();
                entity.Property(e => e.usuario_modificacion).IsRequired();
                entity.Property(e => e.fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Lider>(entity =>
            {
                entity.HasKey(e => e.Id_lider);
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.A_paterno).IsRequired();
                entity.Property(e => e.A_materno).IsRequired();
                entity.Property(e => e.Telefono);
                entity.Property(e => e.Email);
                entity.Property(e => e.Direccion);
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<LiderLog>(entity =>
            {
                entity.HasKey(e => new { e.id_movimiento, e.id_lider });
                entity.Property(e => e.id_movimiento).IsRequired();
                entity.Property(e => e.id_lider).IsRequired();
                entity.Property(e => e.nombre).IsRequired();
                entity.Property(e => e.a_paterno).IsRequired();
                entity.Property(e => e.a_materno).IsRequired();
                entity.Property(e => e.telefono);
                entity.Property(e => e.email);
                entity.Property(e => e.direccion);
                entity.Property(e => e.estado).IsRequired();                
                entity.Property(e => e.usuario_modificacion).IsRequired();
                entity.Property(e => e.fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Padron>(entity =>
            {
                entity.HasKey(e => e.Id_padron);
                entity.Property(e => e.Matricula).IsRequired();
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.A_paterno).IsRequired();
                entity.Property(e => e.A_materno).IsRequired();
                entity.Property(e => e.Curp);
                entity.Property(e => e.Direccion);
                entity.Property(e => e.Telefono);
                entity.Property(e => e.Email);
                entity.Property(e => e.Id_gremio).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Recaudacion>(entity =>
            {
                entity.HasKey(e => e.Id_recaudacion);
                entity.Property(e => e.Id_padron).IsRequired();
                entity.Property(e => e.Id_concepto).IsRequired();
                entity.Property(e => e.Monto).IsRequired();
                entity.Property(e => e.Id_cobrador).IsRequired();
                entity.Property(e => e.Fecha_cobro).IsRequired();
            });
        }
    }
}
