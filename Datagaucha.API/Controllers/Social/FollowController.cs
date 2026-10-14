/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.API.Utils;
using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Datagaucha.API.Controllers;

[ApiController]
[Route("api/follow")]
public class FollowController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork dataBase = unitOfWork;

    [Authorize]
    [HttpPost("{userId}")]
    public async Task<IActionResult> Follow(long userId)
    {
        User currentUser = await getAuthenticatedUser();
        validationSelfFollow(currentUser, userId);
        User userToFollow = await getUser(userId);

        Follow? existedFollow = await this.dataBase.FollowRepository.GetFollow(currentUser.Id, userId);

        if (existedFollow != null)
        {
            throw new BusinessConflictException("Ya seguis a este usuario");
        }

        Follow follow = new Follow
        {
            Follower = currentUser,
            Following = userToFollow
        };

        await this.dataBase.FollowRepository.Create(follow);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<UserSummaryDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Usuario seguido correctamente",
            success = true,
            payload = buildDTO(userToFollow)
        });
    }

    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Unfollow(long userId)
    {
        User currentUser = await getAuthenticatedUser();

        Follow? follow = await this.dataBase.FollowRepository.GetFollow(currentUser.Id, userId);

        if (follow is null)
        {
            throw new BusinessNotFoundException("No seguis a este usuario");
        }

        await this.dataBase.FollowRepository.Delete(follow);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<UserSummaryDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Dejaste de seguir al usuario correctamente",
            success = true,
            payload = null
        });
    }

    [HttpGet("{userId}/followers")]
    public async Task<IActionResult> GetFollowers(long userId, [FromQuery] PagedRequest request)
    {
        await getUser(userId);
        validationPaged(request.currentPage, request.pageSize);

        List<User> followers = await this.dataBase.FollowRepository.GetFollowers(
            userId,
            request.currentPage,
            request.pageSize);

        return Ok(new ResponseDTO<List<UserSummaryDTO>>
        {
            code = (int)HttpStatusCode.OK,
            message = "Seguidores obtenidos correctamente",
            success = true,
            payload = buildDTOs(followers)
        });
    }

    [HttpGet("{userId}/following")]
    public async Task<IActionResult> GetFollowing(long userId, [FromQuery] PagedRequest request)
    {
        await getUser(userId);
        validationPaged(request.currentPage, request.pageSize);

        List<User> following = await this.dataBase.FollowRepository.GetFollowing(
            userId,
            request.currentPage,
            request.pageSize);

        return Ok(new ResponseDTO<List<UserSummaryDTO>>
        {
            code = (int)HttpStatusCode.OK,
            message = "Usuarios seguidos obtenidos correctamente",
            success = true,
            payload = buildDTOs(following)
        });
    }

    private static void validationSelfFollow(User currentUser, long userId)
    {
        if (currentUser.Id == userId)
        {
            throw new ValidationException("No podes seguirte a vos mismo");
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

    private async Task<User> getAuthenticatedUser()
    {
        long userId = User.GetUserId();
        return await getUser(userId);
    }

    private async Task<User> getUser(long userId)
    {
        User? user = await this.dataBase.UserRepository.GetuserById(userId);

        if (user is null)
        {
            throw new BusinessNotFoundException("El usuario no existe");
        }

        return user;
    }

    private static UserSummaryDTO buildDTO(User user)
    {
        return new UserSummaryDTO
        {
            id = user.Id,
            userName = user.UserName,
            urlAvatar = user.GetAvatarurl()
        };
    }

    private static List<UserSummaryDTO> buildDTOs(List<User> users)
    {
        List<UserSummaryDTO> userDTOs = new List<UserSummaryDTO>();

        foreach (User user in users)
        {
            userDTOs.Add(buildDTO(user));
        }

        return userDTOs;
    }
}
