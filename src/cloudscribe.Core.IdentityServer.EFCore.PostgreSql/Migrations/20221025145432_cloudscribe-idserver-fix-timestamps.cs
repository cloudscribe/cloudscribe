using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql.Migrations
{
    public partial class cloudscribeidserverfixtimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
          @"DO $$
            BEGIN
                ALTER TABLE csids_api_resources
                  ALTER created TYPE timestamptz USING created AT TIME ZONE 'UTC'
                , ALTER created SET DEFAULT now();

                ALTER TABLE csids_api_resources
                  ALTER last_accessed TYPE timestamptz USING last_accessed AT TIME ZONE 'UTC'
                , ALTER last_accessed SET DEFAULT now();

                ALTER TABLE csids_api_resources
                  ALTER updated TYPE timestamptz USING updated AT TIME ZONE 'UTC'
                , ALTER updated SET DEFAULT now();


                ALTER TABLE csids_api_secrets
                  ALTER expiration TYPE timestamptz USING expiration AT TIME ZONE 'UTC'
                , ALTER expiration SET DEFAULT now();

                ALTER TABLE csids_client_secrets
                  ALTER expiration TYPE timestamptz USING expiration AT TIME ZONE 'UTC'
                , ALTER expiration SET DEFAULT now();


                ALTER TABLE csids_clients
                  ALTER created TYPE timestamptz USING created AT TIME ZONE 'UTC'
                , ALTER created SET DEFAULT now();

                ALTER TABLE csids_clients
                  ALTER last_accessed TYPE timestamptz USING last_accessed AT TIME ZONE 'UTC'
                , ALTER last_accessed SET DEFAULT now();

                ALTER TABLE csids_clients
                  ALTER updated TYPE timestamptz USING updated AT TIME ZONE 'UTC'
                , ALTER updated SET DEFAULT now();


                ALTER TABLE csids_identity_resources
                  ALTER created TYPE timestamptz USING created AT TIME ZONE 'UTC'
                , ALTER created SET DEFAULT now();

                ALTER TABLE csids_identity_resources
                  ALTER updated TYPE timestamptz USING updated AT TIME ZONE 'UTC'
                , ALTER updated SET DEFAULT now();

            END$$;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
