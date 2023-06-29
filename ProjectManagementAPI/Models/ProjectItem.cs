namespace ProjectManagementAPI.Models;

public class ProjectItem
{
    public int Project_id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    public string ProjectManager { get; set; } = String.Empty;

}
