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

        public virtual DbSet<AuthGroup> AuthGroups { get; set; }
        public virtual DbSet<AuthGroupPermission> AuthGroupPermissions { get; set; }
        public virtual DbSet<AuthPermission> AuthPermissions { get; set; }
        public virtual DbSet<AuthUser> AuthUsers { get; set; }
        public virtual DbSet<AuthUserGroup> AuthUserGroups { get; set; }
        public virtual DbSet<AuthUserUserPermission> AuthUserUserPermissions { get; set; }
        public virtual DbSet<DjangoAdminLog> DjangoAdminLogs { get; set; }
        public virtual DbSet<DjangoContentType> DjangoContentTypes { get; set; }
        public virtual DbSet<DjangoMigration> DjangoMigrations { get; set; }
        public virtual DbSet<DjangoSession> DjangoSessions { get; set; }
        public virtual DbSet<EasyThumbnailsSource> EasyThumbnailsSources { get; set; }
        public virtual DbSet<EasyThumbnailsThumbnail> EasyThumbnailsThumbnails { get; set; }
        public virtual DbSet<EasyThumbnailsThumbnaildimension> EasyThumbnailsThumbnaildimensions { get; set; }
        public virtual DbSet<ModelsModel> ModelsModels { get; set; }
        public virtual DbSet<ModelsPrediction> ModelsPredictions { get; set; }
        public virtual DbSet<ModelsPredictiondatum> ModelsPredictiondata { get; set; }
        public virtual DbSet<ModelsPredictionintervaltype> ModelsPredictionintervaltypes { get; set; }
        public virtual DbSet<ModelsPredictionintervalwidth> ModelsPredictionintervalwidths { get; set; }
        public virtual DbSet<ModelsScenario> ModelsScenarios { get; set; }
        public virtual DbSet<PostsPost> PostsPosts { get; set; }
        public virtual DbSet<RestrictionsRestriction> RestrictionsRestrictions { get; set; }
        public virtual DbSet<TastypieApiaccess> TastypieApiaccesses { get; set; }
        public virtual DbSet<TastypieApikey> TastypieApikeys { get; set; }
        public virtual DbSet<ThumbnailKvstore> ThumbnailKvstores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ConnectionStrings:DataApi");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<AuthGroup>(entity =>
            {
                entity.ToTable("auth_group");

                entity.HasIndex(e => e.Name, "auth_group_name_a6ea08ec_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.Name, "auth_group_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<AuthGroupPermission>(entity =>
            {
                entity.ToTable("auth_group_permissions");

                entity.HasIndex(e => e.GroupId, "auth_group_permissions_group_id_b120cbf9");

                entity.HasIndex(e => new { e.GroupId, e.PermissionId }, "auth_group_permissions_group_id_permission_id_0cd325b0_uniq")
                    .IsUnique();

                entity.HasIndex(e => e.PermissionId, "auth_group_permissions_permission_id_84c5c92e");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AuthGroupPermissions)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("auth_group_permissions_group_id_b120cbf9_fk_auth_group_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AuthGroupPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("auth_group_permissio_permission_id_84c5c92e_fk_auth_perm");
            });

            modelBuilder.Entity<AuthPermission>(entity =>
            {
                entity.ToTable("auth_permission");

                entity.HasIndex(e => e.ContentTypeId, "auth_permission_content_type_id_2f476e4b");

                entity.HasIndex(e => new { e.ContentTypeId, e.Codename }, "auth_permission_content_type_id_codename_01ab375a_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codename)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("codename");

                entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.AuthPermissions)
                    .HasForeignKey(d => d.ContentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("auth_permission_content_type_id_2f476e4b_fk_django_co");
            });

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.ToTable("auth_user");

                entity.HasIndex(e => e.Username, "auth_user_username_6821ab7c_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.Username, "auth_user_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateJoined)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("date_joined");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsStaff).HasColumnName("is_staff");

                entity.Property(e => e.IsSuperuser).HasColumnName("is_superuser");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("last_login");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<AuthUserGroup>(entity =>
            {
                entity.ToTable("auth_user_groups");

                entity.HasIndex(e => e.GroupId, "auth_user_groups_group_id_97559544");

                entity.HasIndex(e => e.UserId, "auth_user_groups_user_id_6a12ed8b");

                entity.HasIndex(e => new { e.UserId, e.GroupId }, "auth_user_groups_user_id_group_id_94350c0c_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AuthUserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("auth_user_groups_group_id_97559544_fk_auth_group_id");
            });

            modelBuilder.Entity<AuthUserUserPermission>(entity =>
            {
                entity.ToTable("auth_user_user_permissions");

                entity.HasIndex(e => e.PermissionId, "auth_user_user_permissions_permission_id_1fbb5f2c");

                entity.HasIndex(e => e.UserId, "auth_user_user_permissions_user_id_a95ead1b");

                entity.HasIndex(e => new { e.UserId, e.PermissionId }, "auth_user_user_permissions_user_id_permission_id_14a6b632_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AuthUserUserPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("auth_user_user_permi_permission_id_1fbb5f2c_fk_auth_perm");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthUserUserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("auth_user_user_permissions_user_id_a95ead1b_fk_auth_user_id");
            });

            modelBuilder.Entity<DjangoAdminLog>(entity =>
            {
                entity.ToTable("django_admin_log");

                entity.HasIndex(e => e.ContentTypeId, "django_admin_log_content_type_id_c4bce8eb");

                entity.HasIndex(e => e.UserId, "django_admin_log_user_id_c564eba6");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActionFlag).HasColumnName("action_flag");

                entity.Property(e => e.ActionTime)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("action_time");

                entity.Property(e => e.ChangeMessage)
                    .IsRequired()
                    .HasColumnName("change_message");

                entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");

                entity.Property(e => e.ObjectId).HasColumnName("object_id");

                entity.Property(e => e.ObjectRepr)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("object_repr");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.DjangoAdminLogs)
                    .HasForeignKey(d => d.ContentTypeId)
                    .HasConstraintName("django_admin_log_content_type_id_c4bce8eb_fk_django_co");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DjangoAdminLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("django_admin_log_user_id_c564eba6_fk_auth_user_id");
            });

            modelBuilder.Entity<DjangoContentType>(entity =>
            {
                entity.ToTable("django_content_type");

                entity.HasIndex(e => new { e.AppLabel, e.Model }, "django_content_type_app_label_model_76bd3d3b_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppLabel)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("app_label");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("model");
            });

            modelBuilder.Entity<DjangoMigration>(entity =>
            {
                entity.ToTable("django_migrations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.App)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("app");

                entity.Property(e => e.Applied)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("applied");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<DjangoSession>(entity =>
            {
                entity.HasKey(e => e.SessionKey)
                    .HasName("django_session_pkey");

                entity.ToTable("django_session");

                entity.HasIndex(e => e.ExpireDate, "django_session_expire_date_a5c62663");

                entity.HasIndex(e => e.SessionKey, "django_session_session_key_c0390e0f_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.Property(e => e.SessionKey)
                    .HasMaxLength(40)
                    .HasColumnName("session_key");

                entity.Property(e => e.ExpireDate)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("expire_date");

                entity.Property(e => e.SessionData)
                    .IsRequired()
                    .HasColumnName("session_data");
            });

            modelBuilder.Entity<EasyThumbnailsSource>(entity =>
            {
                entity.ToTable("easy_thumbnails_source");

                entity.HasIndex(e => e.Name, "easy_thumbnails_source_name_5fe0edc6");

                entity.HasIndex(e => e.Name, "easy_thumbnails_source_name_5fe0edc6_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.StorageHash, "easy_thumbnails_source_storage_hash_946cbcc9");

                entity.HasIndex(e => e.StorageHash, "easy_thumbnails_source_storage_hash_946cbcc9_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => new { e.StorageHash, e.Name }, "easy_thumbnails_source_storage_hash_name_481ce32d_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Modified)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("modified");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.StorageHash)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("storage_hash");
            });

            modelBuilder.Entity<EasyThumbnailsThumbnail>(entity =>
            {
                entity.ToTable("easy_thumbnails_thumbnail");

                entity.HasIndex(e => new { e.StorageHash, e.Name, e.SourceId }, "easy_thumbnails_thumbnai_storage_hash_name_source_fb375270_uniq")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "easy_thumbnails_thumbnail_name_b5882c31");

                entity.HasIndex(e => e.Name, "easy_thumbnails_thumbnail_name_b5882c31_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.SourceId, "easy_thumbnails_thumbnail_source_id_5b57bc77");

                entity.HasIndex(e => e.StorageHash, "easy_thumbnails_thumbnail_storage_hash_f1435f49");

                entity.HasIndex(e => e.StorageHash, "easy_thumbnails_thumbnail_storage_hash_f1435f49_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Modified)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("modified");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.SourceId).HasColumnName("source_id");

                entity.Property(e => e.StorageHash)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("storage_hash");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.EasyThumbnailsThumbnails)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("easy_thumbnails_thum_source_id_5b57bc77_fk_easy_thum");
            });

            modelBuilder.Entity<EasyThumbnailsThumbnaildimension>(entity =>
            {
                entity.ToTable("easy_thumbnails_thumbnaildimensions");

                entity.HasIndex(e => e.ThumbnailId, "easy_thumbnails_thumbnaildimensions_thumbnail_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.ThumbnailId).HasColumnName("thumbnail_id");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.HasOne(d => d.Thumbnail)
                    .WithOne(p => p.EasyThumbnailsThumbnaildimension)
                    .HasForeignKey<EasyThumbnailsThumbnaildimension>(d => d.ThumbnailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("easy_thumbnails_thum_thumbnail_id_c3a0c549_fk_easy_thum");
            });

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

            modelBuilder.Entity<PostsPost>(entity =>
            {
                entity.ToTable("posts_post");

                entity.HasIndex(e => e.Created, "posts_post_created_6da6a35d");

                entity.HasIndex(e => e.Pinned, "posts_post_pinned_48f7def0");

                entity.HasIndex(e => e.Published, "posts_post_published_8ea66e33");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Author)
                    .HasMaxLength(100)
                    .HasColumnName("author");

                entity.Property(e => e.AuthorEn)
                    .HasMaxLength(100)
                    .HasColumnName("author_en");

                entity.Property(e => e.AuthorSl)
                    .HasMaxLength(100)
                    .HasColumnName("author_sl");

                entity.Property(e => e.Blurb)
                    .IsRequired()
                    .HasColumnName("blurb");

                entity.Property(e => e.BlurbEn).HasColumnName("blurb_en");

                entity.Property(e => e.BlurbSl).HasColumnName("blurb_sl");

                entity.Property(e => e.Body).HasColumnName("body");

                entity.Property(e => e.BodyEn).HasColumnName("body_en");

                entity.Property(e => e.BodySl).HasColumnName("body_sl");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.LinkTo)
                    .HasMaxLength(200)
                    .HasColumnName("link_to");

                entity.Property(e => e.LinkToEn)
                    .HasMaxLength(200)
                    .HasColumnName("link_to_en");

                entity.Property(e => e.LinkToSl)
                    .HasMaxLength(200)
                    .HasColumnName("link_to_sl");

                entity.Property(e => e.Pinned).HasColumnName("pinned");

                entity.Property(e => e.Published).HasColumnName("published");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.TitleEn)
                    .HasMaxLength(200)
                    .HasColumnName("title_en");

                entity.Property(e => e.TitleSl)
                    .HasMaxLength(200)
                    .HasColumnName("title_sl");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<RestrictionsRestriction>(entity =>
            {
                entity.ToTable("restrictions_restriction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasColumnName("comments");

                entity.Property(e => e.CommentsEn).HasColumnName("comments_en");

                entity.Property(e => e.CommentsSl).HasColumnName("comments_sl");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created");

                entity.Property(e => e.Exceptions)
                    .IsRequired()
                    .HasColumnName("exceptions");

                entity.Property(e => e.ExceptionsEn).HasColumnName("exceptions_en");

                entity.Property(e => e.ExceptionsSl).HasColumnName("exceptions_sl");

                entity.Property(e => e.ExtraRules)
                    .IsRequired()
                    .HasColumnName("extra_rules");

                entity.Property(e => e.ExtraRulesEn).HasColumnName("extra_rules_en");

                entity.Property(e => e.ExtraRulesSl).HasColumnName("extra_rules_sl");

                entity.Property(e => e.LegalLink)
                    .HasMaxLength(200)
                    .HasColumnName("legal_link");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.Published).HasColumnName("published");

                entity.Property(e => e.Regions)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("regions");

                entity.Property(e => e.RegionsEn)
                    .HasMaxLength(255)
                    .HasColumnName("regions_en");

                entity.Property(e => e.RegionsSl)
                    .HasMaxLength(255)
                    .HasColumnName("regions_sl");

                entity.Property(e => e.Rule)
                    .IsRequired()
                    .HasColumnName("rule");

                entity.Property(e => e.RuleEn).HasColumnName("rule_en");

                entity.Property(e => e.RuleSl).HasColumnName("rule_sl");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.TitleEn)
                    .HasMaxLength(255)
                    .HasColumnName("title_en");

                entity.Property(e => e.TitleSl)
                    .HasMaxLength(255)
                    .HasColumnName("title_sl");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("updated");

                entity.Property(e => e.ValidSince)
                    .HasColumnType("date")
                    .HasColumnName("valid_since");

                entity.Property(e => e.ValidUntil)
                    .HasColumnType("date")
                    .HasColumnName("valid_until");

                entity.Property(e => e.ValidityComment)
                    .IsRequired()
                    .HasColumnName("validity_comment");

                entity.Property(e => e.ValidityCommentEn).HasColumnName("validity_comment_en");

                entity.Property(e => e.ValidityCommentSl).HasColumnName("validity_comment_sl");
            });

            modelBuilder.Entity<TastypieApiaccess>(entity =>
            {
                entity.ToTable("tastypie_apiaccess");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accessed).HasColumnName("accessed");

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("identifier");

                entity.Property(e => e.RequestMethod)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("request_method");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url");
            });

            modelBuilder.Entity<TastypieApikey>(entity =>
            {
                entity.ToTable("tastypie_apikey");

                entity.HasIndex(e => e.Key, "tastypie_apikey_key_17b411bb");

                entity.HasIndex(e => e.Key, "tastypie_apikey_key_17b411bb_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.HasIndex(e => e.UserId, "tastypie_apikey_user_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("key");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.TastypieApikey)
                    .HasForeignKey<TastypieApikey>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tastypie_apikey_user_id_8c8fa920_fk_auth_user_id");
            });

            modelBuilder.Entity<ThumbnailKvstore>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("thumbnail_kvstore_pkey");

                entity.ToTable("thumbnail_kvstore");

                entity.HasIndex(e => e.Key, "thumbnail_kvstore_key_3f850178_like")
                    .HasOperators(new[] { "varchar_pattern_ops" });

                entity.Property(e => e.Key)
                    .HasMaxLength(200)
                    .HasColumnName("key");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
