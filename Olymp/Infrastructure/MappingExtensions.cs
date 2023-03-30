using Olymp.Repository.Dto;

namespace Olymp.Infrastructure;

public static class MappingExtensions
{
    public static Model.Participant ToParticipant(this ParticipantDto participantDto)
    {
        return new Model.Participant
        {
            Id = participantDto.Id,
            Name = participantDto.Name,
            Wish = participantDto.Wish
        };
    }
    
    public static Model.Group ToGroup(this GroupDto groupDto, ParticipantDto[] participants)
    {
        return new Model.Group
        {
            Id = groupDto.Id,
            Name = groupDto.Name,
            Description = groupDto.Description,
            Participants = participants.Select(p => p.ToParticipant()).ToArray()
        };
    }
}