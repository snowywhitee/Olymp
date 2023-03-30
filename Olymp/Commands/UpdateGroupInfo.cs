using MediatR;
using Olymp.Repository;

namespace Olymp.Commands;

public class UpdateGroupInfo
{
    public record Command(int Id, string Name, string Description) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IGroupRepository _groupRepository;

        public Handler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _groupRepository.Update(request.Id, request.Name, request.Description, cancellationToken);
            
            return Unit.Value;
        }
    }
}