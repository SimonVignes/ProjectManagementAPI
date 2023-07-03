namespace ProjectManagementAPI.Models;

public class ProjectItem
{
    public int Project_id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ProjectManager { get; set; }

    public List<EpicItem>? Epics { get; set; }

}
