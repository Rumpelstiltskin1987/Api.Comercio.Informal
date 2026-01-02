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
    public class MySQLiteContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public MySQLiteContext(DbContextOptions<MySQLiteContext> options)
            : base(options)
        {
        }

        public DbSet<Cobrador> Cobrador { get; set; }
        public DbSet<CobradorLog> CobradorLog { get; set; }
        public DbSet<Concepto> Concepto { get; set; }
        public DbSet<ConceptoLog> ConceptoLog { get; set; }
        public DbSet<Folio> Folio { get; set; }
        public DbSet<Gremio> Gremio { get; set; }
        public DbSet<GremioLog> GremioLog { get; set; }
        public DbSet<Lider> Lider { get; set; }
        public DbSet<LiderLog> LiderLog { get; set; }
        public DbSet<MatriculaContador> MatriculaContador { get; set; }
        public DbSet<Padron> Padron { get; set; }
        public DbSet<PadronLog> PadronLog { get; set; }
        public DbSet<Recaudacion> Recaudacion { get; set; }
        public DbSet<Tarifa> Tarifa { get; set; }
        public DbSet<TarifaLog> TarifaLog { get; set; } 
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
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
                entity.Property(e => e.Tipo_movimiento).IsRequired();
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
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<ConceptoLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_concepto});
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_concepto).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Tipo_movimiento).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Folio>(entity =>
            {
                entity.HasKey(e => e.Id_folio_serie);
                entity.Property(e => e.Id_gremio);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Prefijo).IsRequired();
                entity.Property(e => e.Siguiente_folio).IsRequired();
                entity.Property(e => e.Anio_vigente).IsRequired();
            });

            modelBuilder.Entity<Gremio>(entity =>
            {
                entity.HasKey(e => e.Id_gremio);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Id_lider).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<GremioLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_gremio });
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_gremio).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Id_lider).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Tipo_movimiento).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
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
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<LiderLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_lider });
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_lider).IsRequired();
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.A_paterno).IsRequired();
                entity.Property(e => e.A_materno).IsRequired();
                entity.Property(e => e.Telefono);
                entity.Property(e => e.Email);
                entity.Property(e => e.Direccion);
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Tipo_movimiento).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<Padron>(entity =>
            {
                entity.HasKey(e => e.Id_padron);
                entity.Property(e => e.Nombre).IsRequired(); 
                entity.Property(e => e.A_paterno).IsRequired(); 
                entity.Property(e => e.A_materno).IsRequired(); 
                entity.Property(e => e.Curp).IsRequired();
                entity.Property(e => e.Direccion).IsRequired();
                entity.Property(e => e.Telefono).IsRequired();
                entity.Property(e => e.Email);
                entity.Property(e => e.Matricula).IsRequired();
                entity.Property(e => e.Matricula_anterior);
                entity.Property(e => e.Id_gremio).IsRequired();
                entity.Property(e => e.Tipo_vendedor).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<PadronLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_padron });
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_padron).IsRequired();
                entity.Property(e => e.Matricula).IsRequired();
                entity.Property(e => e.Nombre).IsRequired();
                entity.Property(e => e.A_paterno).IsRequired();
                entity.Property(e => e.A_materno).IsRequired();
                entity.Property(e => e.Curp).IsRequired();
                entity.Property(e => e.Direccion).IsRequired();
                entity.Property(e => e.Telefono).IsRequired();
                entity.Property(e => e.Email);
                entity.Property(e => e.Matricula_anterior);
                entity.Property(e => e.Id_gremio).IsRequired();
                entity.Property(e => e.Tipo_vendedor).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Tipo_movimiento).IsRequired();
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<Recaudacion>(entity =>
            {
                entity.HasKey(e => e.Id_recaudacion);
                entity.Property(e => e.Id_padron).IsRequired();
                entity.Property(e => e.Id_concepto).IsRequired();
                entity.Property(e => e.Monto).IsRequired();
                entity.Property(e => e.Id_cobrador).IsRequired();
                entity.Property(e => e.Fecha_cobro).IsRequired();
                entity.Property(e => e.Folio_Recibo).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Latitud);
                entity.Property(e => e.Longitud);
            });

            modelBuilder.Entity<Tarifa>(entity =>
            {
                entity.HasKey(e => e.Id_tarifa);
                entity.Property(e => e.Id_concepto).IsRequired();
                entity.Property(e => e.Id_gremio);
                entity.Property(e => e.Monto).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Usuario_alta).IsRequired();
                entity.Property(e => e.Fecha_alta).IsRequired();
                entity.Property(e => e.Usuario_modificacion);
                entity.Property(e => e.Fecha_modificacion);
            });

            modelBuilder.Entity<TarifaLog>(entity =>
            {
                entity.HasKey(e => new { e.Id_movimiento, e.Id_tarifa });
                entity.Property(e => e.Id_movimiento).IsRequired();
                entity.Property(e => e.Id_tarifa).IsRequired();
                entity.Property(e => e.Id_concepto).IsRequired();
                entity.Property(e => e.Id_gremio);
                entity.Property(e => e.Monto).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Tipo_movimiento).IsRequired();
                entity.Property(e => e.Usuario_modificacion).IsRequired();
                entity.Property(e => e.Fecha_modificacion).IsRequired();
            });

            modelBuilder.Entity<MatriculaContador>(entity =>
            {
                // Clave primaria compuesta
                entity.HasKey(e => new { e.Tipo_vendedor, e.Anio });
                entity.Property(e => e.Tipo_vendedor).IsRequired();
                entity.Property(e => e.Anio).IsRequired();
                entity.Property(e => e.Siguiente_numero).IsRequired();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.A_paterno).IsRequired().HasMaxLength(100);
                entity.Property(u => u.A_materno).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Usuario_alta).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Fecha_alta).IsRequired();
                entity.Property(u => u.Alias).HasMaxLength(50);
                entity.Property(u => u.Fecha_modificacion);
                entity.Property(u => u.Usuario_modificacion).HasMaxLength(50);
            });


        }
    }
}
