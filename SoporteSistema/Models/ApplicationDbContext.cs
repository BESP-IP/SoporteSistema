using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SoporteSistema.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Historial> Historials { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5AF8DAE1D7");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__Comentar__1BA6C6F478D46F5D");

            entity.ToTable("Comentario");

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdReporte).HasColumnName("id_reporte");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Texto)
                .HasColumnType("text")
                .HasColumnName("texto");

            entity.HasOne(d => d.IdReporteNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdReporte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__id_re__68487DD7");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__id_us__693CA210");
        });

        modelBuilder.Entity<Historial>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__76E6C5027769EA84");

            entity.ToTable("Historial");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.Accion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("accion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdReporte).HasColumnName("id_reporte");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdReporteNavigation).WithMany(p => p.Historials)
                .HasForeignKey(d => d.IdReporte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__id_re__656C112C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Historials)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__id_us__6477ECF3");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK__Reporte__87E4F5CB25E55BC4");

            entity.ToTable("Reporte");

            entity.Property(e => e.IdReporte).HasColumnName("id_reporte");
            entity.Property(e => e.ArchivoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("archivo_url");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.TipoReporte)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_reporte");
            entity.Property(e => e.TituloReporte)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("titulo_reporte");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reporte__id_cate__619B8048");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04ADB63AB1AE");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__AB6E6164E2B5F124").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
