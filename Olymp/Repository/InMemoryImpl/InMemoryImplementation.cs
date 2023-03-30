using System.Collections.Concurrent;
using Olymp.Repository.Dto;

namespace Olymp.Repository.InMemoryImpl;

public class InMemoryImplementation
{
    public ConcurrentDictionary<int, GroupDto> Groups = new ();
    public ConcurrentDictionary<int, ParticipantDto> Participants = new ();

    public InMemoryImplementation()
    {
        FillParticipants();
        FillGroups();
    }

    private void FillParticipants()
    {
        Participants[0] = new ParticipantDto
        {
            Id = 0,
            Name = "Anna",
            Wish = "Some wish",
            RecipientId = 1
        };
        
        Participants[1] = new ParticipantDto
        {
            Id = 1,
            Name = "Alexander",
            RecipientId = 0,
            Wish = "Some wish"
        };
    }
    
    private void FillGroups()
    {
        Groups[0] = new GroupDto
        {
            Id = 1,
            Name = "First group",
            Description = "This is first group",
            ParticipantIds = new []{0, 1}
        };
    }
}