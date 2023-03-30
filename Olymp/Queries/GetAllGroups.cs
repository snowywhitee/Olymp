using MediatR;
using Olymp.Repository;
using Olymp.Repository.Dto;

namespace Olymp.Queries;

public class GetAllGroups
{
    public record Query() : IRequest<Response>;
    public record Response(GroupDto[] Groups);
    
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IGroupRepository _groupRepository;

        public Handler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAll(cancellationToken);
            return new Response(Groups: groups);
        }
    }
}