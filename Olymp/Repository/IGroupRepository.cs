using Olymp.Repository.Dto;

namespace Olymp.Repository;

public interface IGroupRepository
{
    public Task<GroupDto[]> GetAll(CancellationToken cancellationToken);
    
    public Task<GroupDto?> Find(int id, CancellationToken cancellationToken);
    
    public Task<bool> Insert(GroupDto groupDto, CancellationToken cancellationToken);
    
    public Task<GroupDto> Insert(string name, string description, int[] participantIds, CancellationToken cancellationToken);
    
    public Task<bool> Update(int id, string name, string description, CancellationToken cancellationToken);
    
    public Task<bool> Update(int id, string name, CancellationToken cancellationToken);
    
    public Task<bool> Delete(int id, CancellationToken cancellationToken);

    public Task<bool> AddParticipant(int id, int participantId, CancellationToken cancellationToken);
    
    public Task<bool> DeleteParticipant(int id, int participantId, CancellationToken cancellationToken);
}