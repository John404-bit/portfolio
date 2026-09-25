namespace backend.Models;

//hvad projektet indeholder
public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
}
