/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using System.Net;
using Datagaucha.API.Utils;
using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Datagaucha.API.Controllers;

[ApiController]
[Route("api/reaction")]
public class ReactionController : ControllerBase
{
    private readonly IUnitOfWork dataBase;

    public ReactionController(IUnitOfWork unitOfWork)
    {
        this.dataBase = unitOfWork;
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> SetReaction([FromBody] ReactionRequest request)
    {
        User user = await getAuthenticatedUser();
        Post post = await getPost(request.postId);
        validationType(request.type);

        Reaction? reaction = await this.dataBase.ReactionRepository.GetReaction(
            user.Id,
            post.Id);

        if (reaction is null)
        {
            reaction = new Reaction
            {
                User = user,
                Post = post,
                Type = request.type
            };

            await this.dataBase.ReactionRepository.Create(reaction);
        }
        else
        {
            reaction.Type = request.type;
        }

        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<object>
        {
            code = (int)HttpStatusCode.OK,
            message = "Reacción guardada correctamente",
            success = true,
            payload = null
        });
    }

    [Authorize]
    [HttpDelete("{postId}")]
    public async Task<IActionResult> Delete(long postId)
    {
        User user = await getAuthenticatedUser();

        Reaction? reaction = await this.dataBase.ReactionRepository.GetReaction(
            user.Id,
            postId);

        if (reaction is null)
        {
            throw new BusinessNotFoundException("No tenés una reacción en esta publicación");
        }

        await this.dataBase.ReactionRepository.Delete(reaction);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<object>
        {
            code = (int)HttpStatusCode.OK,
            message = "Reacción eliminada correctamente",
            success = true,
            payload = null
        });
    }

    private static void validationType(ReactionType type)
    {
        if (!Enum.IsDefined(type))
        {
            throw new ValidationException("El tipo de reacción no es válido");
        }
    }

    private async Task<User> getAuthenticatedUser()
    {
        long userId = User.GetUserId();
        User? user = await this.dataBase.UserRepository.GetuserById(userId);

        if (user is null)
        {
            throw new BusinessNotFoundException("El usuario autenticado no existe");
        }

        return user;
    }

    private async Task<Post> getPost(long postId)
    {
        Post? post = await this.dataBase.PostRepository.GetPostById(postId);

        if (post is null)
        {
            throw new BusinessNotFoundException("El post no existe");
        }

        return post;
    }
}
