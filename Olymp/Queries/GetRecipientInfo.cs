using MediatR;
using Olymp.Infrastructure;
using Olymp.Repository;
using Olymp.Repository.Dto;

namespace Olymp.Queries;

public class GetRecipientInfo
{
    public record Query(int GroupId, int ParticipantId) : IRequest<Response>;
    public record Response(ParticipantDto Participant);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IParticipantRepository _participantRepository;

        public Handler(IGroupRepository groupRepository, IParticipantRepository participantRepository)
        {
            _groupRepository = groupRepository;
            _participantRepository = participantRepository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.Find(request.GroupId, cancellationToken);

            if (group is null)
            {
                throw new BusinessLogicException("Group not found", 404);
            }
            
            var participant = await _participantRepository.Find(request.ParticipantId, cancellationToken);

            if (participant is null)
            {
                throw new BusinessLogicException("Participant not found", 404);
            }
            
            var recipient = await _participantRepository.Find(participant.RecipientId, cancellationToken);

            if (recipient is null)
            {
                throw new BusinessLogicException("Participant not found", 404);
            }

            return new Response(recipient);
        }
    }
}