namespace ProjectManagementAPI.Models;

public class EpicItem
{
    public int Epic_id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    public int Project_id { get; set; }

}
