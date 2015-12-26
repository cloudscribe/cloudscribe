using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using cloudscribe.Logging.EF;

namespace cloudscribe.Logging.EF.Migrations
{
    [DbContext(typeof(LoggingDbContext))]
    partial class LoggingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("Relational:Sequence:.LogIds", "'LogIds', '', '1', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.Core.Models.Logging.LogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnName", "ID")
                        .HasAnnotation("Relational:GeneratedValueSql", "NEXT VALUE FOR LogIds")
                        .HasAnnotation("SqlServer:ColumnType", "int");

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
