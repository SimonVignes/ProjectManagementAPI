using System;
namespace ProjectManagementAPI.DTO
{
    public record struct ProjectDto(
        string Description,
        string Name,
        List<EpicDto> Epics);

}

