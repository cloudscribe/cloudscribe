using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.Core.IdentityServer.EFCore.MySql;

namespace cloudscribe.Core.IdentityServer.EFCore.MySql.Migrations.PersistedGrantDb
{
    [DbContext(typeof(PersistedGrantDbContext))]
    [Migration("20161206130934_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("cloudscribe.Core.IdentityServer.EFCore.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .HasMaxLength(50);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime");

                    b.Property<string>("Data")
                        .IsRequired();

                    b.Property<DateTime>("Expiration");

                    b.Property<string>("SiteId")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200);

                    b.HasKey("Key", "Type");

                    b.HasIndex("SiteId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("SubjectId", "ClientId");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("csids_PersistedGrants");
                });
        }
    }
}
