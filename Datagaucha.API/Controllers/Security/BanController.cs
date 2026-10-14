/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.API.Utils;
using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Datagaucha.API.Controllers;

[ApiController]
[Route("api/ban")]
public class BanController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork dataBase = unitOfWork;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBanRequest request)
    {
        User currentUser = await getAuthenticatedUser();
        validationReason(request.reason);
        User bannedUser = await getUser(request.userId);
        validationSelfBan(currentUser, bannedUser);

        Ban? existedBan = await this.dataBase.BanRepository.GetBanByUserId(bannedUser.Id);

        if (existedBan != null)
        {
            throw new BusinessConflictException("El usuario ya esta baneado");
        }

        Ban ban = new Ban
        {
            User = bannedUser,
            Person = bannedUser,
            Reason = request.reason.Trim(),
            BannedAt = DateTime.Now
        };

        await this.dataBase.BanRepository.Create(ban);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<BanDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Usuario baneado correctamente",
            success = true,
            payload = buildDTO(ban)
        });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBans([FromQuery] PagedRequest request)
    {
        validationPaged(request.currentPage, request.pageSize);

        List<Ban> bans = await this.dataBase.BanRepository.GetBans(request.currentPage, request.pageSize);

        List<BanDTO> banDTOs = new List<BanDTO>();
        foreach (Ban ban in bans)
        {
            banDTOs.Add(buildDTO(ban));
        }

        return Ok(new ResponseDTO<List<BanDTO>>
        {
            code = (int)HttpStatusCode.OK,
            message = "Baneos obtenidos correctamente",
            success = true,
            payload = banDTOs
        });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        Ban? ban = await this.dataBase.BanRepository.GetBanById(id);

        if (ban is null)
        {
            throw new BusinessNotFoundException("El baneo no existe");
        }

        await this.dataBase.BanRepository.Delete(ban);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<BanDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Baneo eliminado correctamente",
            success = true,
            payload = null
        });
    }

    private static void validationReason(string reason)
    {
        if (String.IsNullOrWhiteSpace(reason))
        {
            throw new ValidationException("El motivo del baneo es un dato obligatorio");
        }
    }

    private static void validationSelfBan(User currentUser, User bannedUser)
    {
        if (currentUser.Id == bannedUser.Id)
        {
            throw new ValidationException("No podes banear tu propia cuenta");
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

    private static BanDTO buildDTO(Ban ban)
    {
        return new BanDTO
        {
            id = ban.Id,
            userId = ban.User is null ? 0 : ban.User.Id,
            userName = ban.GetUserName(),
            reason = ban.Reason,
            bannedAt = ban.BannedAt
        };
    }
}
