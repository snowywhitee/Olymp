using Olymp.Repository.Dto;

namespace Olymp.Repository.InMemoryImpl;

public class GroupRepository : IGroupRepository
{
    private readonly InMemoryImplementation _inMemoryImplementation;


    public GroupRepository(InMemoryImplementation inMemoryImplementation)
    {
        _inMemoryImplementation = inMemoryImplementation;
    }
    
    public Task<GroupDto[]> GetAll(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<GroupDto[]>(cancellationToken);
        }

        return Task.FromResult(_inMemoryImplementation.Groups.Values.ToArray());
    }

    public Task<GroupDto?> Find(int id, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<GroupDto?>(cancellationToken);
        }

        if (!_inMemoryImplementation.Groups.TryGetValue(id, out var group))
        {
            return Task.FromResult<GroupDto?>(null);
        }

        return Task.FromResult<GroupDto?>(group);
    }

    public Task<bool> Insert(GroupDto groupDto, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (_inMemoryImplementation.Groups.ContainsKey(groupDto.Id))
        {
            return Task.FromResult(false);
        }

        _inMemoryImplementation.Groups[groupDto.Id] = groupDto;

        return Task.FromResult(true);
    }

    public Task<GroupDto> Insert(string name, string description, int[] participantIds, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<GroupDto>(cancellationToken);
        }

        var groupDto = new GroupDto
        {
            Id = GenerateNewKey(),
            Name = name,
            Description = description,
            ParticipantIds = participantIds
        };

        _inMemoryImplementation.Groups[groupDto.Id] = groupDto;

        return Task.FromResult(groupDto);
    }

    public Task<bool> Update(int id, string name, string description, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (!_inMemoryImplementation.Groups.ContainsKey(id))
        {
            return Task.FromResult(false);
        }
        
        var updatedGroup = new GroupDto
        {
            Id = id,
            Name = name,
            Description = description
        };

        _inMemoryImplementation.Groups[id] = updatedGroup;
        
        return Task.FromResult(true);
    }
    
    public Task<bool> Update(int id, string name, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (!_inMemoryImplementation.Groups.ContainsKey(id))
        {
            return Task.FromResult(false);
        }
        
        var updatedGroup = new GroupDto
        {
            Id = id,
            Name = name,
            Description = _inMemoryImplementation.Groups[id].Description
        };

        _inMemoryImplementation.Groups[id] = updatedGroup;
        
        return Task.FromResult(true);
    }

    public Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        if (!_inMemoryImplementation.Groups.ContainsKey(id))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(_inMemoryImplementation.Groups.TryRemove(id, out _));
    }

    public Task<bool> AddParticipant(int id, int participantId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }
        
        if (!_inMemoryImplementation.Groups.TryGetValue(id, out var group))
        {
            return Task.FromResult(false);
        }

        var newParticipantList = group.ParticipantIds.ToList();
        if (newParticipantList.Contains(participantId))
        {
            return Task.FromResult(true);
        }

        newParticipantList.Add(participantId);

        _inMemoryImplementation.Groups[id].ParticipantIds = newParticipantList.ToArray();
        return Task.FromResult(true);
    }

    public Task<bool> DeleteParticipant(int id, int participantId, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }
        
        if (!_inMemoryImplementation.Groups.TryGetValue(id, out var group))
        {
            return Task.FromResult(false);
        }
        
        var newParticipantList = group.ParticipantIds.ToList();
        if (!newParticipantList.Contains(participantId))
        {
            return Task.FromResult(true);
        }
        
        newParticipantList.Remove(participantId);
        
        _inMemoryImplementation.Groups[id].ParticipantIds = newParticipantList.ToArray();
        return Task.FromResult(true);
    }

    private int GenerateNewKey()
    {
        var keys = _inMemoryImplementation.Groups.Keys;

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