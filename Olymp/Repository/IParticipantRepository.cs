using Olymp.Repository.Dto;

namespace Olymp.Repository;

public interface IParticipantRepository
{
    public Task<ParticipantDto[]> GetAll(CancellationToken cancellationToken);
    
    public Task<ParticipantDto?> Find(int id, CancellationToken cancellationToken);
    
    public Task<ParticipantDto[]> FindMany(int[] ids, CancellationToken cancellationToken);
    
    public Task<bool> Insert(ParticipantDto participantDto, CancellationToken cancellationToken);
    
    public Task<ParticipantDto> Insert(string name, string wish, int recipientId, CancellationToken cancellationToken);
    
    public Task<bool> Update(int id, ParticipantDto participantDto, CancellationToken cancellationToken);
    
    public Task<bool> AssignRecipient(int id, int recipientId, CancellationToken cancellationToken);
    
    public Task<bool> Delete(int id, CancellationToken cancellationToken);
}