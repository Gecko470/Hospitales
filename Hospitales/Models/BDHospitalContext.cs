using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hospitales.Models
{
    public partial class BDHospitalContext : DbContext
    {
        public BDHospitalContext()
        {
        }

        public BDHospitalContext(DbContextOptions<BDHospitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Boton> Botons { get; set; } = null!;
        public virtual DbSet<CitaMedicamento> CitaMedicamentos { get; set; } = null!;
        public virtual DbSet<Citum> Cita { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Especialidad> Especialidads { get; set; } = null!;
        public virtual DbSet<EstadoCitum> EstadoCita { get; set; } = null!;
        public virtual DbSet<FormaFarmaceutica> FormaFarmaceuticas { get; set; } = null!;
        public virtual DbSet<HistorialCitum> HistorialCita { get; set; } = null!;
        public virtual DbSet<Medicamento> Medicamentos { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Pagina> Paginas { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<Sede> Sedes { get; set; } = null!;
        public virtual DbSet<Sexo> Sexos { get; set; } = null!;
        public virtual DbSet<TipoSangre> TipoSangres { get; set; } = null!;
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; } = null!;
        public virtual DbSet<TipoUsuarioPagina> TipoUsuarioPaginas { get; set; } = null!;
        public virtual DbSet<TipoUsuarioPaginaBoton> TipoUsuarioPaginaBotons { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boton>(entity =>
            {
                entity.HasKey(e => e.Iidboton)
                    .HasName("PK__BOTON__90C2312F68927075");

                entity.ToTable("Boton");

                entity.Property(e => e.Iidboton).HasColumnName("IIDBOTON");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<CitaMedicamento>(entity =>
            {
                entity.HasKey(e => e.Iidcitamedicamento)
                    .HasName("PK__citamedi__E4CCE514844E03D5");

                entity.ToTable("CitaMedicamento");

                entity.Property(e => e.Iidcitamedicamento)
                    .ValueGeneratedNever()
                    .HasColumnName("IIDCITAMEDICAMENTO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.Iidcita).HasColumnName("IIDCITA");

                entity.Property(e => e.Iidmedicamento).HasColumnName("IIDMEDICAMENTO");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PRECIO");

                entity.HasOne(d => d.IidcitaNavigation)
                    .WithMany(p => p.CitaMedicamentos)
                    .HasForeignKey(d => d.Iidcita)
                    .HasConstraintName("FK__citamedic__IIDCI__03F0984C");

                entity.HasOne(d => d.IidmedicamentoNavigation)
                    .WithMany(p => p.CitaMedicamentos)
                    .HasForeignKey(d => d.Iidmedicamento)
                    .HasConstraintName("FK__citamedic__IIDME__04E4BC85");
            });

            modelBuilder.Entity<Citum>(entity =>
            {
                entity.HasKey(e => e.Iidcita)
                    .HasName("PK_Consulta");

                entity.Property(e => e.Iidcita).HasColumnName("IIDCITA");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcionenfermedad)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCIONENFERMEDAD");

                entity.Property(e => e.Dfechacita)
                    .HasColumnType("datetime")
                    .HasColumnName("DFECHACITA");

                entity.Property(e => e.Dfechainicioenfermedad)
                    .HasColumnType("datetime")
                    .HasColumnName("DFECHAINICIOENFERMEDAD");

                entity.Property(e => e.Diagnostico)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DIAGNOSTICO");

                entity.Property(e => e.Estatura)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ESTATURA");

                entity.Property(e => e.Examenfisico)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EXAMENFISICO");

                entity.Property(e => e.Examenlaboratorio)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EXAMENLABORATORIO");

                entity.Property(e => e.Iiddoctorasignado).HasColumnName("IIDDOCTORASIGNADO");

                entity.Property(e => e.Iidestadocita).HasColumnName("IIDESTADOCITA");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidsede).HasColumnName("IIDSEDE");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Peso)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PESO");

                entity.Property(e => e.Precioatencion)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PRECIOATENCION");

                entity.Property(e => e.Totalpagar)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TOTALPAGAR");

                entity.HasOne(d => d.IiddoctorasignadoNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iiddoctorasignado)
                    .HasConstraintName("FK__Cita__IIDDOCTORA__01142BA1");

                entity.HasOne(d => d.IidestadocitaNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidestadocita)
                    .HasConstraintName("FK__Cita__IIDESTADOC__7E37BEF6");

                entity.HasOne(d => d.IidpersonaNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidpersona)
                    .HasConstraintName("FK__Cita__IIDPERSONA__2739D489");

                entity.HasOne(d => d.IidsedeNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidsede)
                    .HasConstraintName("FK__Cita__IIDSEDE__7F2BE32F");

                entity.HasOne(d => d.IidusuarioNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidusuario)
                    .HasConstraintName("FK__Cita__IIDUSUARIO__7C4F7684");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Iiddoctor);

                entity.ToTable("Doctor");

                entity.Property(e => e.Iiddoctor).HasColumnName("IIDDOCTOR");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Fechacontrato)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACONTRATO");

                entity.Property(e => e.Iidespecialidad).HasColumnName("IIDESPECIALIDAD");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidsede).HasColumnName("IIDSEDE");

                entity.Property(e => e.Sueldo)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("SUELDO");

                entity.HasOne(d => d.IidespecialidadNavigation)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.Iidespecialidad)
                    .HasConstraintName("FK_Doctor_Especialidad");

                entity.HasOne(d => d.IidpersonaNavigation)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.Iidpersona)
                    .HasConstraintName("FK__Doctor__IIDPERSO__693CA210");

                entity.HasOne(d => d.IidsedeNavigation)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.Iidsede)
                    .HasConstraintName("FK_Doctor_Clinica");
            });

            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.Iidespecialidad)
                    .HasName("PK_TipoEspecialidad");

                entity.ToTable("Especialidad");

                entity.Property(e => e.Iidespecialidad).HasColumnName("IIDESPECIALIDAD");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<EstadoCitum>(entity =>
            {
                entity.HasKey(e => e.Iidestado)
                    .HasName("PK__ESTACITA__CB0F471FE45B7978");

                entity.Property(e => e.Iidestado)
                    .ValueGeneratedNever()
                    .HasColumnName("IIDESTADO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Vdescripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("VDESCRIPCION");

                entity.Property(e => e.Vnombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("VNOMBRE");
            });

            modelBuilder.Entity<FormaFarmaceutica>(entity =>
            {
                entity.HasKey(e => e.Iidformafarmaceutica);

                entity.ToTable("FormaFarmaceutica");

                entity.Property(e => e.Iidformafarmaceutica).HasColumnName("IIDFORMAFARMACEUTICA");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<HistorialCitum>(entity =>
            {
                entity.HasKey(e => e.Iidhistorialcita)
                    .HasName("PK__Historia__35A9F434FABE77C7");

                entity.Property(e => e.Iidhistorialcita).HasColumnName("IIDHISTORIALCITA");

                entity.Property(e => e.Dfecha)
                    .HasColumnType("datetime")
                    .HasColumnName("DFECHA");

                entity.Property(e => e.Iidcita).HasColumnName("IIDCITA");

                entity.Property(e => e.Iidestado).HasColumnName("IIDESTADO");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Vobservacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VOBSERVACION");

                entity.HasOne(d => d.IidcitaNavigation)
                    .WithMany(p => p.HistorialCita)
                    .HasForeignKey(d => d.Iidcita)
                    .HasConstraintName("FK__Historial__IIDCI__07C12930");

                entity.HasOne(d => d.IidestadoNavigation)
                    .WithMany(p => p.HistorialCita)
                    .HasForeignKey(d => d.Iidestado)
                    .HasConstraintName("FK__Historial__IIDES__08B54D69");

                entity.HasOne(d => d.IidusuarioNavigation)
                    .WithMany(p => p.HistorialCita)
                    .HasForeignKey(d => d.Iidusuario)
                    .HasConstraintName("FK__Historial__IIDUS__09A971A2");
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.HasKey(e => e.Iidmedicamento);

                entity.ToTable("Medicamento");

                entity.Property(e => e.Iidmedicamento).HasColumnName("IIDMEDICAMENTO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Concentracion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONCENTRACION");

                entity.Property(e => e.Iidformafarmaceutica).HasColumnName("IIDFORMAFARMACEUTICA");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PRECIO");

                entity.Property(e => e.Presentacion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PRESENTACION");

                entity.Property(e => e.Stock).HasColumnName("STOCK");

                entity.HasOne(d => d.IidformafarmaceuticaNavigation)
                    .WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.Iidformafarmaceutica)
                    .HasConstraintName("FK__Medicamen__IIDFO__36B12243");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.Iidpaciente)
                    .HasName("PK_Pacientes_1");

                entity.ToTable("Paciente");

                entity.Property(e => e.Iidpaciente).HasColumnName("IIDPACIENTE");

                entity.Property(e => e.Alergias)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ALERGIAS");

                entity.Property(e => e.Antecedentesquirurgicos)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ANTECEDENTESQUIRURGICOS");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Cuadrovacunas)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CUADROVACUNAS");

                entity.Property(e => e.Enfermedadescronicas)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ENFERMEDADESCRONICAS");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidtiposangre).HasColumnName("IIDTIPOSANGRE");

                entity.HasOne(d => d.IidtiposangreNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Iidtiposangre)
                    .HasConstraintName("FK__Paciente__IIDTIP__17036CC0");
            });

            modelBuilder.Entity<Pagina>(entity =>
            {
                entity.HasKey(e => e.Iidpagina)
                    .HasName("PK__Pagina__8E759E4E24D07BF3");

                entity.ToTable("Pagina");

                entity.Property(e => e.Iidpagina).HasColumnName("IIDPAGINA");

                entity.Property(e => e.Accion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACCION");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Controlador)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONTROLADOR");

                entity.Property(e => e.Mensaje)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MENSAJE");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Iidpersona)
                    .HasName("PK_Paciente");

                entity.ToTable("Persona");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Apmaterno)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("APMATERNO");

                entity.Property(e => e.Appaterno)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("APPATERNO");

                entity.Property(e => e.Bdoctor).HasColumnName("BDOCTOR");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Bpaciente).HasColumnName("BPACIENTE");

                entity.Property(e => e.Btieneusuario).HasColumnName("BTIENEUSUARIO");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fechanacimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHANACIMIENTO");

                entity.Property(e => e.Foto)
                    .IsUnicode(false)
                    .HasColumnName("FOTO");

                entity.Property(e => e.Iidsexo).HasColumnName("IIDSEXO");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Telefonocelular)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONOCELULAR");

                entity.Property(e => e.Telefonofijo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONOFIJO");

                entity.HasOne(d => d.IidsexoNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.Iidsexo)
                    .HasConstraintName("FK_Paciente_Sexo");

                entity.HasOne(d => d.IidusuarioNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.Iidusuario)
                    .HasConstraintName("FK__Persona__IIDUSUA__282DF8C2");
            });

            modelBuilder.Entity<Sede>(entity =>
            {
                entity.HasKey(e => e.Iidsede)
                    .HasName("PK_Clinica");

                entity.ToTable("Sede");

                entity.Property(e => e.Iidsede).HasColumnName("IIDSEDE");

                entity.Property(e => e.Bhabilitado)
                    .HasColumnName("BHABILITADO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<Sexo>(entity =>
            {
                entity.HasKey(e => e.Iidsexo);

                entity.ToTable("Sexo");

                entity.Property(e => e.Iidsexo).HasColumnName("IIDSEXO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<TipoSangre>(entity =>
            {
                entity.HasKey(e => e.Iidtiposangre);

                entity.ToTable("TipoSangre");

                entity.Property(e => e.Iidtiposangre).HasColumnName("IIDTIPOSANGRE");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.Iidtipousuario);

                entity.ToTable("TipoUsuario");

                entity.Property(e => e.Iidtipousuario).HasColumnName("IIDTIPOUSUARIO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<TipoUsuarioPagina>(entity =>
            {
                entity.HasKey(e => e.Iidtipousuariopagina)
                    .HasName("PK__TipoUsua__59B815B9209568AC");

                entity.ToTable("TipoUsuarioPagina");

                entity.Property(e => e.Iidtipousuariopagina).HasColumnName("IIDTIPOUSUARIOPAGINA");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Iidpagina).HasColumnName("IIDPAGINA");

                entity.Property(e => e.Iidtipousuario).HasColumnName("IIDTIPOUSUARIO");

                entity.Property(e => e.Iidvista).HasColumnName("IIDVISTA");

                entity.HasOne(d => d.IidpaginaNavigation)
                    .WithMany(p => p.TipoUsuarioPaginas)
                    .HasForeignKey(d => d.Iidpagina)
                    .HasConstraintName("FK__TipoUsuar__IIDPA__75A278F5");

                entity.HasOne(d => d.IidtipousuarioNavigation)
                    .WithMany(p => p.TipoUsuarioPaginas)
                    .HasForeignKey(d => d.Iidtipousuario)
                    .HasConstraintName("FK__TipoUsuar__IIDTI__74AE54BC");
            });

            modelBuilder.Entity<TipoUsuarioPaginaBoton>(entity =>
            {
                entity.HasKey(e => e.Iidtipousuariopaginaboton)
                    .HasName("PK__TipoUsua__7882D20C06997AB0");

                entity.ToTable("TipoUsuarioPaginaBoton");

                entity.Property(e => e.Iidtipousuariopaginaboton).HasColumnName("IIDTIPOUSUARIOPAGINABOTON");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Iidboton).HasColumnName("IIDBOTON");

                entity.Property(e => e.Iidtipousuariopagina).HasColumnName("IIDTIPOUSUARIOPAGINA");

                entity.HasOne(d => d.IidbotonNavigation)
                    .WithMany(p => p.TipoUsuarioPaginaBotons)
                    .HasForeignKey(d => d.Iidboton)
                    .HasConstraintName("FK__TipoUsuar__IIDBO__797309D9");

                entity.HasOne(d => d.IidtipousuariopaginaNavigation)
                    .WithMany(p => p.TipoUsuarioPaginaBotons)
                    .HasForeignKey(d => d.Iidtipousuariopagina)
                    .HasConstraintName("FK__TipoUsuar__IIDTI__787EE5A0");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Iidusuario);

                entity.ToTable("Usuario");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONTRASEÑA");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidtipousuario).HasColumnName("IIDTIPOUSUARIO");

                entity.Property(e => e.Nombreusuario)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBREUSUARIO");

                entity.HasOne(d => d.IidpersonaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Iidpersona)
                    .HasConstraintName("FK__Usuario__IIDPERS__14270015");

                entity.HasOne(d => d.IidtipousuarioNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Iidtipousuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usuario__IIDTIPO__68487DD7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
