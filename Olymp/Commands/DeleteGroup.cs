using MediatR;
using Olymp.Repository;

namespace Olymp.Commands;

public class DeleteGroup
{
    public record Command(int Id) : IRequest<Unit>;
    
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IGroupRepository _groupRepository;

        public Handler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _groupRepository.Delete(request.Id, cancellationToken);

            return Unit.Value;
        }
    }
}