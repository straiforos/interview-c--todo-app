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

            // Policy: Users can only see tasks they created or are assigned to
            migrationBuilder.Sql(@"
                CREATE POLICY task_isolation_policy ON ""Tasks""
                USING (
                    ""CreatorId"" = current_setting('app.current_user_id', true) 
                    OR ""AssigneeId"" = current_setting('app.current_user_id', true)
                )
                WITH CHECK (
                    ""CreatorId"" = current_setting('app.current_user_id', true)
                );
            ");

            // Enable RLS on Notifications table
            migrationBuilder.Sql("ALTER TABLE \"Notifications\" ENABLE ROW LEVEL SECURITY;");
            migrationBuilder.Sql("ALTER TABLE \"Notifications\" FORCE ROW LEVEL SECURITY;");

            // Policy: Users can only see their own notifications
            migrationBuilder.Sql(@"
                CREATE POLICY notification_isolation_policy ON ""Notifications""
                USING (""UserId"" = current_setting('app.current_user_id', true))
                WITH CHECK (""UserId"" = current_setting('app.current_user_id', true));
            ");

            // Enable RLS on MediaItems table
            migrationBuilder.Sql("ALTER TABLE \"MediaItems\" ENABLE ROW LEVEL SECURITY;");
            migrationBuilder.Sql("ALTER TABLE \"MediaItems\" FORCE ROW LEVEL SECURITY;");

            // Policy: Users can only see their own media
            migrationBuilder.Sql(@"
                CREATE POLICY media_isolation_policy ON ""MediaItems""
                USING (""CreatorId"" = current_setting('app.current_user_id', true))
                WITH CHECK (""CreatorId"" = current_setting('app.current_user_id', true));
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP POLICY IF EXISTS task_isolation_policy ON \"Tasks\";");
            migrationBuilder.Sql("ALTER TABLE \"Tasks\" DISABLE ROW LEVEL SECURITY;");

            migrationBuilder.Sql("DROP POLICY IF EXISTS notification_isolation_policy ON \"Notifications\";");
            migrationBuilder.Sql("ALTER TABLE \"Notifications\" DISABLE ROW LEVEL SECURITY;");

            migrationBuilder.Sql("DROP POLICY IF EXISTS media_isolation_policy ON \"MediaItems\";");
            migrationBuilder.Sql("ALTER TABLE \"MediaItems\" DISABLE ROW LEVEL SECURITY;");
        }
    }
}
