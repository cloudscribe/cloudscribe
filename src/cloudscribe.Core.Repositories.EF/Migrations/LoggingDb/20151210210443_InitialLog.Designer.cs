using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using cloudscribe.Core.Repositories.EF;

namespace cloudscribe.Core.Repositories.EF.Migrations.LoggingDb
{
    [DbContext(typeof(LoggingDbContext))]
    [Migration("20151210210443_InitialLog")]
    partial class InitialLog
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Core.Models.Logging.LogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Culture")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("IpAddress")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<DateTime>("LogDateUtc")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "LogDate")
                        .HasAnnotation("SqlServer:ColumnType", "datetime")
                        .HasAnnotation("SqlServer:GeneratedValueSql", "getutcdate()");

                    b.Property<string>("LogLevel")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("Logger")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Message");

                    b.Property<string>("ShortUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Thread")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "mp_SystemLog");
                });
        }
    }
}
