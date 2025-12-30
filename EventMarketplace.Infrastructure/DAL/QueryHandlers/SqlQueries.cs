namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public static class SqlQueries
{
    public const string AdminStats = """
         SELECT
             (SELECT COUNT(*) FROM "Events") AS "TotalEvents",
             (SELECT COUNT(*) FROM "Events" WHERE "EventStatus" = @approvedStatus) AS "ApprovedEvents",
             (SELECT COUNT(*) FROM "Events" WHERE "EventStatus" = @rejectedStatus) AS "RejectedEvents",
             (SELECT COUNT(*) FROM "Events" WHERE "EventStatus" = @pendingStatus) AS "PendingEvents",
             (SELECT COUNT(*) FROM "Users") AS "TotalUsers",
             (SELECT COUNT(*) FROM "Users" u
                 JOIN "UserRoles" ur ON u."Id" = ur."UserId"
                 JOIN "Roles" r ON ur."RoleId" = r."Id"
                 WHERE r."RoleType" = @organizerRoleType) AS "TotalOrganizers",
             (SELECT COUNT(*) FROM "Users" u
                 JOIN "UserRoles" ur ON u."Id" = ur."UserId"
                 JOIN "Roles" r ON ur."RoleId" = r."Id"
                 WHERE r."RoleType" = @participantRoleType) AS "TotalParticipants";
     """;
}