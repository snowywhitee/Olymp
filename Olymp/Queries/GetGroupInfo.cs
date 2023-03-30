using MediatR;
using Olymp.Infrastructure;
using Olymp.Repository;
using Olymp.Repository.Dto;

namespace Olymp.Queries;

public class GetGroupInfo
{
    public record Query(int Id) : IRequest<Response>;
    public record Response(GroupDto Group);
    
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IGroupRepository _groupRepository;

        public Handler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.Find(request.Id, cancellationToken);
            
            if (group is null)
            {
                throw new BusinessLogicException("Group not found", 404);
            }

            return new Response(group);
        }
    }
}