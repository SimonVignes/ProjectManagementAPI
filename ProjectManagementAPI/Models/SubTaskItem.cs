namespace ProjectManagementAPI.Models;

public class SubTaskItem
{
    public int Subtask_id { get; set; }

    public string TaskName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Responsible { get; set; } = string.Empty;

    public int Epic_id { get; set; }
}
