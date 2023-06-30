using System;
namespace ProjectManagementAPI.DTO
{
    public record struct EpicDto(
        string Name,
        List<SubTaskDto> SubTasks);

}

