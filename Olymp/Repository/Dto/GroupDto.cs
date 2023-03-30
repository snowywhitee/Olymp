namespace Olymp.Repository.Dto;

public class GroupDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int[] ParticipantIds { get; set; }
}