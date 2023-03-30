using MediatR;
using Microsoft.AspNetCore.Mvc;
using Olymp.Commands;
using Olymp.Queries;

namespace Olymp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GroupController : Controller
{
    private readonly IMediator _mediator;

    public GroupController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AddGroup.Response>> AddGroup([FromBody] AddGroupRequest body)
    {
        return Ok(await _mediator.Send(new AddGroup.Command(body.Name, body.Description)));
    }

    [HttpGet("groups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAllGroups.Response>> GetGroups()
    {
        return Ok(await _mediator.Send(new GetAllGroups.Query()));
    }
    
    [HttpGet("group/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetGroupInfo.Response>> GetGroupInfo([FromRoute] int id)
    {
        return Ok(await _mediator.Send(new GetGroupInfo.Query(id)));
    }
    
    [HttpPut("group/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Unit>> UpdateGroupInfo([FromRoute] int id, [FromBody] UpdateGroupInfoRequest body)
    {
        return Ok(await _mediator.Send(new UpdateGroupInfo.Command(id, body.Name, body.Description)));
    }
    
    [HttpDelete("group/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Unit>> DeleteGroup([FromRoute] int id)
    {
        return Ok(await _mediator.Send(new DeleteGroup.Command(id)));
    }
    
    [HttpPost("group/{id:int}/participant")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Unit>> AddParticipant([FromRoute] int id, [FromBody] AddParticipantRequest body)
    {
        return Ok(await _mediator.Send(new AddParticipant.Command(id, body.Name, body.Wish)));
    }
    
    [HttpDelete("group/{groupId:int}/participant/{participantId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Unit>> DeleteParticipant([FromRoute] int groupId, [FromRoute] int participantId)
    {
        return Ok(await _mediator.Send(new DeleteParticipant.Command(groupId, participantId)));
    }
    
    [HttpGet("group/{groupId:int}/participant/{participantId:int}/recipient")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetRecipientInfo.Response>> GetRecipientInfo([FromRoute] int groupId, [FromRoute] int participantId)
    {
        return Ok(await _mediator.Send(new GetRecipientInfo.Query(groupId, participantId)));
    }
    
    [HttpGet("group/{groupId:int}/toss")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Toss.Response>> Toss([FromRoute] int groupId)
    {
        return Ok(await _mediator.Send(new Toss.Command(groupId)));
    }

    public record AddGroupRequest
    {
        public string Name { get; init; }
        public string? Description { get; init; }
    }

    public record UpdateGroupInfoRequest
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }

    public record AddParticipantRequest
    { 
        public string Name { get; init; }
        public string? Wish { get; init; }
    }
}