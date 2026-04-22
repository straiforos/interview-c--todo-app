using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRowLevelSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Tasks"" ENABLE ROW LEVEL SECURITY;
                ALTER TABLE ""Tasks"" FORCE ROW LEVEL SECURITY;
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
                ALTER TABLE ""Tasks"" DISABLE ROW LEVEL SECURITY;
            ");
        }
    }
}
