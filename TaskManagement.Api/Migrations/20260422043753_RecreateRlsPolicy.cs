using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class RecreateRlsPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                  IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'app_user') THEN
                    CREATE ROLE app_user WITH LOGIN PASSWORD 'app_password';
                  END IF;
                END
                $$;
                GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO app_user;
                GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO app_user;
            ");

            migrationBuilder.Sql(@"
                DROP POLICY IF EXISTS ""TaskIsolationPolicy"" ON ""Tasks"";
                CREATE POLICY ""TaskIsolationPolicy"" ON ""Tasks""
                AS PERMISSIVE FOR ALL
                USING (
                    ""CreatorId"" = current_setting('app.current_user_id', true)
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP POLICY IF EXISTS ""TaskIsolationPolicy"" ON ""Tasks"";
            ");
        }
    }
}
