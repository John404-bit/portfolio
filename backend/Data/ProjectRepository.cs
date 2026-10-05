using Dapper;
using backend.Models;

namespace backend.Data;

public class ProjectRepository(DapperContext context) : IProjectRepository
{
    public async Task<IEnumerable<Project>> GetAllAsync() //liste af projekter
    {
        using var connection = context.CreateConnection(); //åbn forbindelse
        return await connection.QueryAsync<Project>( //kør SQL via Dapper
            "SELECT Id, Title, Description, CreatedUtc FROM dbo.Projects"); // SQL der bliver sent til databsen
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        using var connection = context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Project>(
            "SELECT Id, Title, Description, CreatedUtc FROM dbo.Projects WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> CreateAsync(Project project)
    {
        using var connection = context.CreateConnection();
        return await connection.QuerySingleAsync<int>(
            """
            INSERT INTO dbo.Projects (Title, Description)
            OUTPUT INSERTED.Id
            VALUES (@Title, @Description)
            """,
            project);
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        using var connection = context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            """
            UPDATE dbo.Projects
            SET Title = @Title, Description = @Description
            WHERE Id = @Id
            """,
            project);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = context.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "DELETE FROM dbo.Projects WHERE Id = @Id",
            new { Id = id });
        return rows > 0;
    }
}
