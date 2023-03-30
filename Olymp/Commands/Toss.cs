using MediatR;
using Olymp.Infrastructure;
using Olymp.Repository;

namespace Olymp.Commands;

public class Toss
{
    public record Command(int GroupId) : IRequest<Response>;
    
    public record Response(Model.Group Group);

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

            if (group.ParticipantIds.Length < 3)
            {
                throw new BusinessLogicException("Not enough participants", 409);
            }
            
            //toss logic
            var possibleIds = new HashSet<int>(group.ParticipantIds);
            foreach (var participant in group.ParticipantIds)
            {
                int assignee;
                while (true)
                {
                    assignee = new Random().Next(possibleIds.Min(), possibleIds.Max());
                    if (assignee != participant)
                    {
                        possibleIds.Remove(assignee);
                        break;
                    }
                }
                
                var result = await _participantRepository.AssignRecipient(participant, assignee, cancellationToken);
                
                if (!result)
                {
                    throw new BusinessLogicException("Cannot assign participant", 500);
                }
            }
            
            
            var participants = await _participantRepository.FindMany(group.ParticipantIds, cancellationToken);
            return new Response(group.ToGroup(participants));
        }
    }
}