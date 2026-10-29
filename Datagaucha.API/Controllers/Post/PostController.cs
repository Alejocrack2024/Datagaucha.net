/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Microsoft.AspNetCore.Mvc;
using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Datagaucha.API.Utils;
using Datagaucha.Domain.Post;
using Datagaucha.Domain.Auth;
using Datagaucha.API.DTOs.Post;
using System.Net;

namespace Datagaucha.API.Controllers;

[ApiController]
[Route("api/post")]
public class PostController(IUnitOfWork unitOfWork, IWebHostEnvironment env) : ControllerBase
{
    private readonly IUnitOfWork dataBase = unitOfWork;
    private readonly IWebHostEnvironment env = env;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPost([FromQuery]GetPostRequest request)
    {
        User user = await getAuthenticatedUser();

        List<Post> posts = await this.dataBase.PostRepository.GetPosts(
            user,
            request.currentPage,
            request.pageSize,
            request.orderBy,
            request.orderDirection,
            request.search
        );

        List<long> postIds = posts.Select(post => post.Id).ToList();
        Dictionary<long, ReactionType> reactions = await this.dataBase.ReactionRepository
            .GetReactionsByUserAndPostIds(user.Id, postIds);

        List<PostDTO> postDTOs = new List<PostDTO>();
        foreach (Post post in posts)
        {
            ReactionType? myReaction = reactions.TryGetValue(
                post.Id,
                out ReactionType reactionType)
                    ? reactionType
                    : null;

            postDTOs.Add(buildDTO(post, myReaction));
        }
        return Ok(new ResponseDTO<List<PostDTO>>(){
            code = (int)HttpStatusCode.OK,
            message = "Post obtenido",
            payload = postDTOs,
            success = true
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePostRequest request)
    {
        User user = await getAuthenticatedUser();
        validationTitle(request.title);
        validationBody(request.body);

        Image? image = await generateImage(request);

        Post post = new Post
        {
            Title = request.title.Trim(),
            Body = request.body.Trim(),
            User = user,
            File = image
        };

        await this.dataBase.PostRepository.Create(post);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<PostDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Post creado correctamente",
            payload = buildDTO(post),
            success = true
        });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        User user = await getAuthenticatedUser();

        Post? post = await this.dataBase.PostRepository.GetPostById(id);

        if (post is null)
        {
            throw new BusinessNotFoundException("El post no existe");
        }

        validationAuthor(post, user);

        await this.dataBase.PostRepository.Delete(post);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<PostDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Post eliminado correctamente",
            payload = null,
            success = true
        });
    }

    private static void validationTitle(string title)
    {
        if (String.IsNullOrWhiteSpace(title))
        {
            throw new ValidationException("El titulo del post es un dato obligatorio");
        }
    }

    private static void validationBody(string body)
    {
        if (String.IsNullOrWhiteSpace(body))
        {
            throw new ValidationException("El texto del post es un dato obligatorio");
        }
    }

    private static void validationAuthor(Post post, User user)
    {
        if (post.User is null || post.User.Id != user.Id)
        {
            throw new BusinessForbiddenException("Solo el autor puede eliminar su post");
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

    private async Task<Image?> generateImage(CreatePostRequest request)
    {
        if (request.file is null) return null;

        FileStorageService fileStorageService = new FileStorageService(this.env);
        return await fileStorageService.SaveImageAsync(request.file);
    }

    private static PostDTO buildDTO(Post post, ReactionType? myReaction = null)
    {
        return new PostDTO
        {
            id = post.Id,
            title = post.Title,
            userName = post.GetUserName(),
            content = post.Body,
            urlImage = post.ImageUrl(),
            myReaction = myReaction
        };
    }
}
