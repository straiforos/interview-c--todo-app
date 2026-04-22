using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Api.Data.Migrations
{
    public partial class AddRlsPolicies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Enable RLS on Tasks table
            migrationBuilder.Sql("ALTER TABLE \"Tasks\" ENABLE ROW LEVEL SECURITY;");
            migrationBuilder.Sql("ALTER TABLE \"Tasks\" FORCE ROW LEVEL SECURITY;");

            // Policy: Users can only see tasks they created
            migrationBuilder.Sql(@"
                CREATE POLICY task_isolation_policy ON ""Tasks""
                USING (
                    ""CreatorId"" = current_setting('app.current_user_id', true)
                )
                WITH CHECK (
                    ""CreatorId"" = current_setting('app.current_user_id', true)
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP POLICY IF EXISTS task_isolation_policy ON \"Tasks\";");
            migrationBuilder.Sql("ALTER TABLE \"Tasks\" DISABLE ROW LEVEL SECURITY;");
        }
    }
}
