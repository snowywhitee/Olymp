using MediatR;
using Olymp.Infrastructure;
using Olymp.Repository;

namespace Olymp.Commands;

public class DeleteParticipant
{
    public record Command(int GroupId, int ParticipantId) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IParticipantRepository _participantRepository;

        public Handler(IGroupRepository groupRepository, IParticipantRepository participantRepository)
        {
            _groupRepository = groupRepository;
            _participantRepository = participantRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var isDeleted = await _groupRepository.DeleteParticipant(request.GroupId, request.ParticipantId, cancellationToken);

            if (!isDeleted)
            {
                throw new BusinessLogicException("Participant couldn't be removed", 500);
            }

            await _participantRepository.Delete(request.ParticipantId, cancellationToken);
            
            return Unit.Value;
        }
    }
}