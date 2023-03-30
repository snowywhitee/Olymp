using MediatR;
using Olymp.Infrastructure;
using Olymp.Repository;

namespace Olymp.Commands;

public class AddParticipant
{
    public record Command(int GroupId, string Name, string? Wish) : IRequest<Response>;
    
    public record Response(int ParticipantId);

    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IParticipantRepository _participantRepository;

        public Handler(IGroupRepository groupRepository, IParticipantRepository participantRepository)
        {
            _groupRepository = groupRepository;
            _participantRepository = participantRepository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.Find(request.GroupId, cancellationToken);
            
            if (group is null)
            {
                throw new BusinessLogicException("Group not found", 404);
            }
            
            var participant = await _participantRepository.Insert(request.Name, request.Wish ?? "", -1, cancellationToken);

            await _groupRepository.AddParticipant(request.GroupId, participant.Id, cancellationToken);

            return new Response(participant.Id);
        }
    }
}