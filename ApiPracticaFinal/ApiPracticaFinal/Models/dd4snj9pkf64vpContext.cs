using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class dd4snj9pkf64vpContext : DbContext
    {
        public dd4snj9pkf64vpContext()
        {
        }

        public dd4snj9pkf64vpContext(DbContextOptions<dd4snj9pkf64vpContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Areasxproyecto> Areasxproyectos { get; set; }
        public virtual DbSet<Equipoxproyecto> Equipoxproyectos { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<Presupuesto> Presupuestos { get; set; }
        public virtual DbSet<Proyecto> Proyectos { get; set; }
        public virtual DbSet<Publicacionesxproyecto> Publicacionesxproyectos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Validadore> Validadores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseNpgsql("Server=ec2-54-80-123-146.compute-1.amazonaws.com; port=5432; user id = vdqlqjukrdasfg; password = 966b43760dde03f70dee9eb2bc502789f4448712a7794a0d8a31962642ddca9c; database=dd4snj9pkf64vp; pooling = true; SSL Mode=Prefer;Trust Server Certificate=true;");
                //optionsBuilder.UseNpgsql("Server=ec2-54-164-40-66.compute-1.amazonaws.com; port=5432; user id = sljswtrlsxsbat; password = 7ea0c2aeddb82406514b4f202b5caaf76f9fa718bc3f7509f4adba20a1f9b180; database=d6jaagghud72bi; pooling = true; SSL Mode=Prefer;Trust Server Certificate=true;");
                //optionsBuilder.UseNpgsql("Server = ec2-54-165-90-230.compute-1.amazonaws.com; port=5432; user id = oqefqqbfdxzxtw; password = 862bbf5518aeaca2786c5c4e67f21bec5ce44e02e7a35777666ec3eb144a6025; database=d5v7c6mis3gfur; pooling = true; SSL Mode=Require;Trust Server Certificate=true;");
                optionsBuilder.UseNpgsql("Server = ec2-3-211-221-185.compute-1.amazonaws.com; port=5432; user id = dermlqrdsmsoqm; password = 475369c16361721b3c4b290d13a719f389ec898b3da37f67b7e59a9229091368; database=dea0svov7skgbt; pooling = true; SSL Mode=Require;Trust Server Certificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("areas");

                entity.HasComment("TRIAL");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("TRIAL");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Area1)
                    .HasMaxLength(255)
                    .HasColumnName("area")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial059)
                    .HasMaxLength(1)
                    .HasColumnName("trial059")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial196)
                    .HasMaxLength(1)
                    .HasColumnName("trial196")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial444)
                    .HasMaxLength(1)
                    .HasColumnName("trial444")
                    .HasComment("TRIAL");
            });

            modelBuilder.Entity<Areasxproyecto>(entity =>
            {
                entity.HasKey(e => new { e.Idproyecto, e.Idarea })
                    .HasName("areasxproyecto$primarykey");

                entity.ToTable("areasxproyecto");

                entity.HasComment("TRIAL");

                entity.HasIndex(e => e.Idarea, "areasxproyecto$idarea");

                entity.Property(e => e.Idproyecto)
                    .HasColumnName("idproyecto")
                    .HasComment("TRIAL");

                entity.Property(e => e.Idarea)
                    .HasColumnName("idarea")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial069)
                    .HasMaxLength(1)
                    .HasColumnName("trial069")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial206)
                    .HasMaxLength(1)
                    .HasColumnName("trial206")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial451)
                    .HasMaxLength(1)
                    .HasColumnName("trial451")
                    .HasComment("TRIAL");

                entity.HasOne(d => d.IdareaNavigation)
                    .WithMany(p => p.Areasxproyectos)
                    .HasForeignKey(d => d.Idarea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("areasxproyecto$_c__database_proyectos_be_mdb__areasareasxproyec");

                entity.HasOne(d => d.IdproyectoNavigation)
                    .WithMany(p => p.Areasxproyectos)
                    .HasForeignKey(d => d.Idproyecto)
                    .HasConstraintName("areasxproyecto$_c__database_proyectos_be_mdb__proyectosareasxpr");
            });

            modelBuilder.Entity<Equipoxproyecto>(entity =>
            {
                entity.HasKey(e => new { e.IdProyecto, e.IdPersonal })
                    .HasName("equipoxproyecto$primarykey");

                entity.ToTable("equipoxproyecto");

                entity.HasComment("TRIAL");

                entity.Property(e => e.IdProyecto)
                    .HasColumnName("id_proyecto")
                    .HasComment("TRIAL");

                entity.Property(e => e.IdPersonal)
                    .HasColumnName("id_personal")
                    .HasComment("TRIAL");

                entity.Property(e => e.Coordinador)
                    .HasColumnName("coordinador")
                    .HasDefaultValueSql("false")
                    .HasComment("TRIAL");

                entity.Property(e => e.FuncionTarea)
                    .HasMaxLength(255)
                    .HasColumnName("funcion_tarea")
                    .HasComment("TRIAL");

                entity.Property(e => e.SsmaTimestamp)
                    .IsRequired()
                    .HasColumnName("ssma_timestamp")
                    .HasComment("TRIAL");

                entity.Property(e => e.Texto)
                    .HasMaxLength(255)
                    .HasColumnName("texto")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial098)
                    .HasMaxLength(1)
                    .HasColumnName("trial098")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial236)
                    .HasMaxLength(1)
                    .HasColumnName("trial236")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial460)
                    .HasMaxLength(1)
                    .HasColumnName("trial460")
                    .HasComment("TRIAL");

                entity.HasOne(d => d.IdPersonalNavigation)
                    .WithMany(p => p.Equipoxproyectos)
                    .HasForeignKey(d => d.IdPersonal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("equipoxproyecto$_c__database_proyectos_be_mdb__personalequipoxp");

                entity.HasOne(d => d.IdProyectoNavigation)
                    .WithMany(p => p.Equipoxproyectos)
                    .HasForeignKey(d => d.IdProyecto)
                    .HasConstraintName("equipoxproyecto$_c__database_proyectos_be_mdb__proyectosequipox");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.ToTable("personal");

                entity.HasComment("TRIAL");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("TRIAL");

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasComment("TRIAL");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .HasColumnName("nombre")
                    .HasComment("TRIAL");

                entity.Property(e => e.Sector)
                    .HasMaxLength(255)
                    .HasColumnName("sector")
                    .HasComment("TRIAL");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .HasColumnName("titulo")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial467)
                    .HasMaxLength(1)
                    .HasColumnName("trial467")
                    .HasComment("TRIAL");
            });

            modelBuilder.Entity<Presupuesto>(entity =>
            {
                entity.HasKey(e => e.Idpresupuesto)
                    .HasName("presupuestos_pkey");

                entity.ToTable("presupuestos");

                entity.Property(e => e.Idpresupuesto)
                    .HasColumnName("idpresupuesto")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Divisa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("divisa");

                entity.Property(e => e.Equipamiento).HasColumnName("equipamiento");

                entity.Property(e => e.Gastos).HasColumnName("gastos");

                entity.Property(e => e.Honorario).HasColumnName("honorario");

                entity.Property(e => e.Idproyecto).HasColumnName("idproyecto");

                entity.Property(e => e.Montototal).HasColumnName("montototal");

                entity.Property(e => e.Viatico).HasColumnName("viatico");

                entity.HasOne(d => d.IdproyectoNavigation)
                    .WithMany(p => p.Presupuestos)
                    .HasForeignKey(d => d.Idproyecto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idproyecto");
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.ToTable("proyectos");

                entity.HasComment("TRIAL");

                entity.HasIndex(e => e.IdArea, "proyectos$id_area");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("TRIAL");

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasComment("TRIAL");

                entity.Property(e => e.AnioFinalizacion)
                    .HasColumnName("anio_finalizacion")
                    .HasComment("TRIAL");

                entity.Property(e => e.AnioInicio)
                    .HasColumnName("anio_inicio")
                    .HasComment("TRIAL");

                entity.Property(e => e.Certconformidad)
                    .HasColumnName("certconformidad")
                    .HasDefaultValueSql("false")
                    .HasComment("TRIAL");

                entity.Property(e => e.Certificadopor)
                    .HasColumnName("certificadopor")
                    .HasDefaultValueSql("0")
                    .HasComment("TRIAL");

                entity.Property(e => e.ConsultoresAsoc)
                    .HasColumnName("consultores_asoc")
                    .HasComment("TRIAL");

                entity.Property(e => e.Contratante)
                    .HasMaxLength(255)
                    .HasColumnName("contratante")
                    .HasComment("TRIAL");

                entity.Property(e => e.Convenio)
                    .HasColumnName("convenio")
                    .HasComment("TRIAL");

                entity.Property(e => e.Departamento)
                    .HasMaxLength(255)
                    .HasColumnName("departamento")
                    .HasComment("TRIAL");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasComment("TRIAL");

                entity.Property(e => e.Dirección)
                    .HasMaxLength(255)
                    .HasColumnName("dirección")
                    .HasComment("TRIAL");

                entity.Property(e => e.EnCurso)
                    .HasColumnName("en_curso")
                    .HasDefaultValueSql("false")
                    .HasComment("TRIAL");

                entity.Property(e => e.FichaLista)
                    .HasColumnName("ficha_lista")
                    .HasDefaultValueSql("false")
                    .HasComment("TRIAL");

                entity.Property(e => e.IdArea)
                    .HasColumnName("id_area")
                    .HasComment("TRIAL");

                entity.Property(e => e.Link)
                    .HasMaxLength(200)
                    .HasColumnName("link")
                    .HasComment("TRIAL");

                entity.Property(e => e.MesFinalizacion)
                    .HasColumnName("mes_finalizacion")
                    .HasComment("TRIAL");

                entity.Property(e => e.MesInicio)
                    .HasColumnName("mes_inicio")
                    .HasComment("TRIAL");

                entity.Property(e => e.Moneda)
                    .HasMaxLength(255)
                    .HasColumnName("moneda")
                    .HasComment("TRIAL");

                entity.Property(e => e.Monto).HasColumnName("monto");

                entity.Property(e => e.MontoContrato)
                    .HasColumnName("monto_contrato")
                    .HasComment("TRIAL");

                entity.Property(e => e.NroContrato)
                    .HasMaxLength(255)
                    .HasColumnName("nro_contrato")
                    .HasComment("TRIAL");

                entity.Property(e => e.PaisRegion)
                    .HasMaxLength(255)
                    .HasColumnName("pais-region")
                    .HasComment("TRIAL");

                entity.Property(e => e.Resultados)
                    .HasColumnName("resultados")
                    .HasComment("TRIAL");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .HasColumnName("titulo")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial483)
                    .HasMaxLength(1)
                    .HasColumnName("trial483")
                    .HasComment("TRIAL");
            });

            modelBuilder.Entity<Publicacionesxproyecto>(entity =>
            {
                entity.HasKey(e => e.IdPublicacion)
                    .HasName("publicacionesxproyecto$primarykey");

                entity.ToTable("publicacionesxproyecto");

                entity.HasComment("TRIAL");

                entity.HasIndex(e => e.IdProyecto, "publicacionesxproyecto$id_proyecto");

                entity.Property(e => e.IdPublicacion)
                    .HasColumnName("id_publicacion")
                    .HasComment("TRIAL");

                entity.Property(e => e.Año)
                    .HasMaxLength(255)
                    .HasColumnName("año")
                    .HasComment("TRIAL");

                entity.Property(e => e.Codigobcs)
                    .HasMaxLength(255)
                    .HasColumnName("codigobcs")
                    .HasComment("TRIAL");

                entity.Property(e => e.IdProyecto)
                    .HasColumnName("id_proyecto")
                    .HasComment("TRIAL");

                entity.Property(e => e.Medio)
                    .HasMaxLength(255)
                    .HasColumnName("medio")
                    .HasComment("TRIAL");

                entity.Property(e => e.Publicacion)
                    .HasMaxLength(255)
                    .HasColumnName("publicacion")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial131)
                    .HasMaxLength(1)
                    .HasColumnName("trial131")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial278)
                    .HasMaxLength(1)
                    .HasColumnName("trial278")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial500)
                    .HasMaxLength(1)
                    .HasColumnName("trial500")
                    .HasComment("TRIAL");

                entity.HasOne(d => d.IdProyectoNavigation)
                    .WithMany(p => p.Publicacionesxproyectos)
                    .HasForeignKey(d => d.IdProyecto)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("publicacionesxproyecto$_c__database_proyectos_be_mdb__proyectos");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Idrol)
                    .HasName("roles_pkey");

                entity.ToTable("roles");

                entity.HasComment("TRIAL");

                entity.Property(e => e.Idrol)
                    .ValueGeneratedNever()
                    .HasColumnName("idrol")
                    .HasComment("TRIAL");

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("rol")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial506)
                    .HasMaxLength(1)
                    .HasColumnName("trial506")
                    .HasComment("TRIAL");
            });

            modelBuilder.Entity<Sysdiagram>(entity =>
            {
                entity.HasKey(e => e.DiagramId)
                    .HasName("pk__sysdiagr__c2b05b61c61709e0");

                entity.ToTable("sysdiagrams");

                entity.HasComment("TRIAL");

                entity.HasIndex(e => new { e.PrincipalId, e.Name }, "uk_principal_name")
                    .IsUnique();

                entity.Property(e => e.DiagramId)
                    .HasColumnName("diagram_id")
                    .HasComment("TRIAL");

                entity.Property(e => e.Definition)
                    .HasColumnName("definition")
                    .HasComment("TRIAL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name")
                    .HasComment("TRIAL");

                entity.Property(e => e.PrincipalId)
                    .HasColumnName("principal_id")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial141)
                    .HasMaxLength(1)
                    .HasColumnName("trial141")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial291)
                    .HasMaxLength(1)
                    .HasColumnName("trial291")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial513)
                    .HasMaxLength(1)
                    .HasColumnName("trial513")
                    .HasComment("TRIAL");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasComment("TRIAL");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario)
                    .HasName("usuarios_pkey");

                entity.ToTable("usuarios");

                entity.Property(e => e.Idusuario)
                    .HasColumnName("idusuario")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("nombre");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Rol).HasColumnName("rol");

                entity.HasOne(d => d.RolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Rol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rol");
            });

            modelBuilder.Entity<Validadore>(entity =>
            {
                entity.ToTable("validadores");

                entity.HasComment("TRIAL");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("TRIAL");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .HasColumnName("nombre")
                    .HasComment("TRIAL");

                entity.Property(e => e.Sector)
                    .HasMaxLength(255)
                    .HasColumnName("sector")
                    .HasComment("TRIAL");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(255)
                    .HasColumnName("titulo")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial147)
                    .HasMaxLength(1)
                    .HasColumnName("trial147")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial301)
                    .HasMaxLength(1)
                    .HasColumnName("trial301")
                    .HasComment("TRIAL");

                entity.Property(e => e.Trial522)
                    .HasMaxLength(1)
                    .HasColumnName("trial522")
                    .HasComment("TRIAL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
