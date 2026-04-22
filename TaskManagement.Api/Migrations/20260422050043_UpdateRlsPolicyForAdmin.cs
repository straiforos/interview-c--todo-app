using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRlsPolicyForAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP POLICY IF EXISTS ""TaskIsolationPolicy"" ON ""Tasks"";

                CREATE POLICY ""TaskIsolationPolicy"" ON ""Tasks""
                FOR ALL
                TO app_user
                USING (
                    current_setting('app.user_role', true) = 'Admin' 
                    OR ""CreatorId"" = current_setting('app.current_user_id', true)
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP POLICY IF EXISTS ""TaskIsolationPolicy"" ON ""Tasks"";

                CREATE POLICY ""TaskIsolationPolicy"" ON ""Tasks""
                FOR ALL
                TO app_user
                USING (""CreatorId"" = current_setting('app.current_user_id', true));
            ");
        }
    }
}
