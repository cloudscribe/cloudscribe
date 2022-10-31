using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations.PersistedGrantDb
{
    public partial class cloudscribeidservergrantsfixtimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"DO $$
            BEGIN
                ALTER TABLE csids_device_flow_codes
                  ALTER creation_time TYPE timestamptz USING creation_time AT TIME ZONE 'UTC'
                , ALTER creation_time SET DEFAULT now();

                ALTER TABLE csids_device_flow_codes
                  ALTER expiration TYPE timestamptz USING expiration AT TIME ZONE 'UTC'
                , ALTER expiration SET DEFAULT now();


                ALTER TABLE csids_persisted_grants
                  ALTER creation_time TYPE timestamptz USING creation_time AT TIME ZONE 'UTC'
                , ALTER creation_time SET DEFAULT now();

                ALTER TABLE csids_persisted_grants
                  ALTER expiration TYPE timestamptz USING expiration AT TIME ZONE 'UTC'
                , ALTER expiration SET DEFAULT now();
            END$$;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
