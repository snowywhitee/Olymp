using MediatR;
using Olymp.Repository;

namespace Olymp.Commands;

public class AddGroup
{
    public record Command(string Name, string? Description) : IRequest<Response>;
    
    public record Response(int GroupId);

    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IGroupRepository _groupRepository;

        public Handler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var groupDto = await _groupRepository.Insert(request.Name, request.Description ?? "", Array.Empty<int>(),
                cancellationToken);

            return new Response(groupDto.Id);
        }
    }
}