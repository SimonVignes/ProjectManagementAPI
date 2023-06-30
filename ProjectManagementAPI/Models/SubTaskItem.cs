namespace ProjectManagementAPI.Models;

public class SubTaskItem
{
    public int Subtask_id { get; set; }

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public int Epic_id { get; set; }

    //public string Responsible { get; set; }
}
