using System.Threading.Tasks;
using Api.Contracts.V1;
using Application.CQRS.Tags.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1
{
    public class TagsController : ApiControllerBase
    {
        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetTagsQuery()));
        }
    }
}