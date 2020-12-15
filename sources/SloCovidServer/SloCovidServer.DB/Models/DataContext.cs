using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ModelsModel> ModelsModels { get; set; }
        public virtual DbSet<ModelsPrediction> ModelsPredictions { get; set; }
        public virtual DbSet<ModelsPredictiondatum> ModelsPredictiondata { get; set; }
        public virtual DbSet<ModelsPredictionintervaltype> ModelsPredictionintervaltypes { get; set; }
        public virtual DbSet<ModelsPredictionintervalwidth> ModelsPredictionintervalwidths { get; set; }
        public virtual DbSet<ModelsScenario> ModelsScenarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ConnectionStrings.DataApi");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<ModelsModel>(entity =>
            {
                entity.ToTable("models_model");

                entity.HasIndex(e => e.Name, "models_model_name_308dcf78_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.Name, "models_model_name_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ContactEmail)
                    .IsRequired()
                    .HasMaxLength(254)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("contact_name");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Www)
                    .HasMaxLength(200)
                    .HasColumnName("www");
            });

            modelBuilder.Entity<ModelsPrediction>(entity =>
            {
                entity.ToTable("models_prediction");

                entity.HasIndex(e => new { e.Date, e.ModelId }, "models_prediction_date_model_id_3c3e1d2f_uniq")
                    .IsUnique();

                entity.HasIndex(e => e.IntervalTypeId, "models_prediction_interval_type_id_b6bdbdbf");

                entity.HasIndex(e => e.IntervalWidthId, "models_prediction_interval_width_id_4b4f57db");

                entity.HasIndex(e => e.ModelId, "models_prediction_model_id_a8aad42b");

                entity.HasIndex(e => e.ScenarioId, "models_prediction_scenario_id_99cb64d9");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.IntervalTypeId).HasColumnName("interval_type_id");

                entity.Property(e => e.IntervalWidthId).HasColumnName("interval_width_id");

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.ScenarioId).HasColumnName("scenario_id");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("updated");

                entity.HasOne(d => d.IntervalType)
                    .WithMany(p => p.ModelsPredictions)
                    .HasForeignKey(d => d.IntervalTypeId)
                    .HasConstraintName("models_prediction_interval_type_id_b6bdbdbf_fk_models_pr");

                entity.HasOne(d => d.IntervalWidth)
                    .WithMany(p => p.ModelsPredictions)
                    .HasForeignKey(d => d.IntervalWidthId)
                    .HasConstraintName("models_prediction_interval_width_id_4b4f57db_fk_models_pr");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.ModelsPredictions)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("models_prediction_model_id_a8aad42b_fk_models_model_id");

                entity.HasOne(d => d.Scenario)
                    .WithMany(p => p.ModelsPredictions)
                    .HasForeignKey(d => d.ScenarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("models_prediction_scenario_id_99cb64d9_fk_models_scenario_id");
            });

            modelBuilder.Entity<ModelsPredictiondatum>(entity =>
            {
                entity.ToTable("models_predictiondata");

                entity.HasIndex(e => e.PredictionId, "models_predictiondata_prediction_id_5e0e7440");

                entity.HasIndex(e => new { e.PredictionId, e.Date }, "models_predictiondata_prediction_id_date_1af62455_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Deceased).HasColumnName("deceased");

                entity.Property(e => e.DeceasedLowerBound).HasColumnName("deceasedLowerBound");

                entity.Property(e => e.DeceasedToDate).HasColumnName("deceasedToDate");

                entity.Property(e => e.DeceasedToDateLowerBound).HasColumnName("deceasedToDateLowerBound");

                entity.Property(e => e.DeceasedToDateUpperBound).HasColumnName("deceasedToDateUpperBound");

                entity.Property(e => e.DeceasedUpperBound).HasColumnName("deceasedUpperBound");

                entity.Property(e => e.Hospitalized).HasColumnName("hospitalized");

                entity.Property(e => e.HospitalizedLowerBound).HasColumnName("hospitalizedLowerBound");

                entity.Property(e => e.HospitalizedUpperBound).HasColumnName("hospitalizedUpperBound");

                entity.Property(e => e.Icu).HasColumnName("icu");

                entity.Property(e => e.IcuLowerBound).HasColumnName("icuLowerBound");

                entity.Property(e => e.IcuUpperBound).HasColumnName("icuUpperBound");

                entity.Property(e => e.PredictionId).HasColumnName("prediction_id");

                entity.HasOne(d => d.Prediction)
                    .WithMany(p => p.ModelsPredictiondata)
                    .HasForeignKey(d => d.PredictionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("models_predictiondat_prediction_id_5e0e7440_fk_models_pr");
            });

            modelBuilder.Entity<ModelsPredictionintervaltype>(entity =>
            {
                entity.ToTable("models_predictionintervaltype");

                entity.HasIndex(e => e.Name, "models_predictionintervaltype_name_904486f2_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.Name, "models_predictionintervaltype_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ModelsPredictionintervalwidth>(entity =>
            {
                entity.ToTable("models_predictionintervalwidth");

                entity.HasIndex(e => e.Width, "models_predictionintervalwidth_width_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Width).HasColumnName("width");
            });

            modelBuilder.Entity<ModelsScenario>(entity =>
            {
                entity.ToTable("models_scenario");

                entity.HasIndex(e => e.Name, "models_scenario_name_686b850f_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.Name, "models_scenario_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
