/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.API.Utils;
using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Datagaucha.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork dataBase = unitOfWork;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
    {
        User user = await getAuthenticatedUser();
        Post post = await getPost(request.postId);
        validationText(request.text);

        Comment comment = new Comment
        {
            Text = request.text.Trim(),
            User = user,
            CreatedAt = DateTime.Now
        };

        post.AddComment(comment);

        await this.dataBase.CommentRepository.Create(comment);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<CommentDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Comentario creado correctamente",
            success = true,
            payload = buildDTO(comment)
        });
    }

    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetByPost(long postId, [FromQuery] GetCommentsRequest request)
    {
        await getPost(postId);
        validationPaged(request.currentPage, request.pageSize);

        List<Comment> comments = await this.dataBase.CommentRepository.GetCommentsByPostId(
            postId,
            request.currentPage,
            request.pageSize);

        List<CommentDTO> commentDTOs = new List<CommentDTO>();
        foreach (Comment comment in comments)
        {
            commentDTOs.Add(buildDTO(comment));
        }

        return Ok(new ResponseDTO<List<CommentDTO>>
        {
            code = (int)HttpStatusCode.OK,
            message = "Comentarios obtenidos correctamente",
            success = true,
            payload = commentDTOs
        });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        User user = await getAuthenticatedUser();
        Comment comment = await getComment(id);
        validationAuthor(comment, user);

        await this.dataBase.CommentRepository.Delete(comment);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<CommentDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Comentario eliminado correctamente",
            success = true,
            payload = null
        });
    }

    private static void validationText(string text)
    {
        if (String.IsNullOrWhiteSpace(text))
        {
            throw new ValidationException("El texto del comentario es un dato obligatorio");
        }
    }

    private static void validationPaged(int currentPage, int pageSize)
    {
        if (currentPage < 1)
        {
            throw new ValidationException("La pagina debe ser mayor o igual a 1");
        }

        if (pageSize < 1)
        {
            throw new ValidationException("El tamaño de pagina debe ser mayor o igual a 1");
        }
    }

    private static void validationAuthor(Comment comment, User user)
    {
        if (comment.User is null || comment.User.Id != user.Id)
        {
            throw new BusinessForbiddenException("Solo el autor puede eliminar su comentario");
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

    private async Task<Comment> getComment(long commentId)
    {
        Comment? comment = await this.dataBase.CommentRepository.GetCommentById(commentId);

        if (comment is null)
        {
            throw new BusinessNotFoundException("El comentario no existe");
        }

        return comment;
    }

    private static CommentDTO buildDTO(Comment comment)
    {
        return new CommentDTO
        {
            id = comment.Id,
            postId = comment.Post is null ? 0 : comment.Post.Id,
            userId = comment.User is null ? 0 : comment.User.Id,
            userName = comment.GetUserName(),
            urlAvatar = comment.GetAvatarUrl(),
            text = comment.Text,
            createdAt = comment.CreatedAt
        };
    }
}
