using System.Threading.Tasks;
using Api.Contracts.V1;
using Api.Contracts.V1.Request;
using Application.CQRS.Posts.Commands.CreatePost;
using Application.CQRS.Posts.Commands.DeletePost;
using Application.CQRS.Posts.Commands.UpdatePost;
using Application.CQRS.Posts.Queries.GetPost;
using Application.CQRS.Posts.Queries.GetPosts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1
{
    public class PostController : ApiControllerBase
    {
        [HttpGet(ApiRoutes.Post.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetPostsQuery()));
        }

        [HttpGet(ApiRoutes.Post.Get)]
        public async Task<IActionResult> Get(long postId)
        {
            return Ok(await Mediator.Send(new GetPostQuery(postId)));
        }
        
        [HttpPost(ApiRoutes.Post.Create)]
        public async Task<IActionResult> Create(CreatePostRequest request)
        {
            var result = await Mediator.Send(new CreatePostCommand(request.Title, request.Content,
                request.Tags));

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = baseUrl + "/" + ApiRoutes.Post.Get.Replace("{postId:long}", result.Id.ToString());
            return Created(location, result);
        }

        [HttpPut(ApiRoutes.Post.Update)]
        public async Task<IActionResult> Update(long postId, UpdatePostRequest request)
        {
             var result = await Mediator.Send(new UpdatePostCommand(postId, request.Title, request.Content,
                request.Tags));
            return Ok(result);
        }

        [HttpDelete(ApiRoutes.Post.Delete)]
        public async Task<IActionResult> Delete(long postId)
        {
            await Mediator.Send(new DeletePostCommand(postId));
            return NoContent();
        }
    }
}