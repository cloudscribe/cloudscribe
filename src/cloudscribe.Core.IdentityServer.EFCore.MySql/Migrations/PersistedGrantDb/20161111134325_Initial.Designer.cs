using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Core.IdentityServer.EFCore.MySql;

namespace cloudscribe.Core.IdentityServer.EFCore.MySql.Migrations.PersistedGrantDb
{
    [DbContext(typeof(PersistedGrantDbContext))]
    [Migration("20161111134325_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key");

                    b.Property<string>("Type");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Data")
                        .IsRequired();

                    b.Property<DateTime>("Expiration");

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("SubjectId");

                    b.HasKey("Key", "Type");

                    b.HasIndex("SiteId");

                    b.ToTable("csids_PersistedGrants");
                });
        }
    }
}
