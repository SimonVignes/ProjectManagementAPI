namespace ProjectManagementAPI.Models;

public class EpicItem
{
    public int Epic_id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Project_id { get; set; }

    public List<SubTaskItem>? SubTasks { get; set; }

}
