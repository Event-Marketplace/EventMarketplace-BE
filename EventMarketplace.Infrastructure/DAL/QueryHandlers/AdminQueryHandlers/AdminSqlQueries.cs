namespace EventMarketplace.Infrastructure.DAL.QueryHandlers.AdminQueryHandlers;

public static class AdminSqlQueries
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

    public const string AdminAlerts = """
                                          SELECT 
                                              e."Title", 
                                              u."FirstName" || ' ' || u."LastName" AS Organizer
                                          FROM "Events" e 
                                          JOIN "Users" u ON u."Id" = e."OrganizerId"
                                          where e."EventStatus" = @pendingStatus
                                          Order By e."CreatedAt" desc 
                                          limit 5; 
                                      
                                          SELECT COUNT(*) AS PendingEventsCount FROM "Events" WHERE "EventStatus" = @pendingStatus;
                                      
                                          -- another list in close future (we use query multiple from dapper)
                                      """;
}