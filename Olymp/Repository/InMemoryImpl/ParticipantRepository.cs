using Olymp.Repository.Dto;

namespace Olymp.Repository.InMemoryImpl;

public class ParticipantRepository : IParticipantRepository
{
    private readonly InMemoryImplementation _inMemoryImplementation;


    public ParticipantRepository(InMemoryImplementation inMemoryImplementation)
    {
        _inMemoryImplementation = inMemoryImplementation;
    }

    public Task<ParticipantDto[]> GetAll(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<ParticipantDto[]>(cancellationToken);
        }

        return Task.FromResult(_inMemoryImplementation.Participants.Values.ToArray());
    }

    public Task<ParticipantDto?> Find(int id, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<ParticipantDto?>(cancellationToken);
        }

        if (!_inMemoryImplementation.Participants.TryGetValue(id, out var participant))
        {
            return Task.FromResult<ParticipantDto?>(null);
        }

        return Task.FromResult<ParticipantDto?>(participant);
    }

    public Task<ParticipantDto[]> FindMany(int[] ids, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<ParticipantDto[]>(cancellationToken);
        }

        var participants = _inMemoryImplementation.Participants
            .Where(p => ids.Contains(p.Key))
            .Select(p => p.Value)
            .ToArray();

        return Task.FromResult(participants);
    }

    public Task<bool> Insert(ParticipantDto participantDto, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (_inMemoryImplementation.Participants.ContainsKey(participantDto.Id))
        {
            return Task.FromResult(false);
        }

        _inMemoryImplementation.Participants[participantDto.Id] = participantDto;

        return Task.FromResult(true);
    }

    public Task<ParticipantDto> Insert(string name, string wish, int recipientId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<ParticipantDto>(cancellationToken);
        }

        var participantDto = new ParticipantDto
        {
            Id = GenerateNewKey(),
            Name = name,
            Wish = wish,
            RecipientId = recipientId
        };

        _inMemoryImplementation.Participants[participantDto.Id] = participantDto;

        return Task.FromResult(participantDto);
    }

    public Task<bool> Update(int id, ParticipantDto participantDto, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (!_inMemoryImplementation.Participants.ContainsKey(id))
        {
            return Task.FromResult(false);
        }

        _inMemoryImplementation.Participants[id] = participantDto;
        
        return Task.FromResult(true);
    }

    public Task<bool> AssignRecipient(int id, int recipientId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (!_inMemoryImplementation.Participants.ContainsKey(recipientId))
        {
            return Task.FromResult(false);
        }
        
        if (!_inMemoryImplementation.Participants.TryGetValue(recipientId, out var participant))
        {
            return Task.FromResult(false);
        }

        var newParticipant = new ParticipantDto
        {
            Id = id,
            Name = participant.Name,
            Wish = participant.Wish,
            RecipientId = participant.RecipientId
        };

        _inMemoryImplementation.Participants[id] = newParticipant;
        
        return Task.FromResult(true);
    }

    public Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (!_inMemoryImplementation.Participants.ContainsKey(id))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(_inMemoryImplementation.Participants.TryRemove(id, out _));
    }

    private int GenerateNewKey()
    {
        var keys = _inMemoryImplementation.Participants.Keys;

        for (int i = 0; i < keys.Max(); i++)
        {
            if (!keys.Contains(i))
            {
                return i;
            }
        }

        return keys.Max() + 1;
    }
}