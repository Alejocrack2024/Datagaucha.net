/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.FileSystem;
using Datagaucha.Domain.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Datagaucha.API.Controllers.Auth;

[ApiController]
[Route("api/user")]

public class UserController : ControllerBase{
    private readonly IUnitOfWork dataBase;
    private IWebHostEnvironment env;
    private JwtTokenGenerator generator;

    public UserController(
        IUnitOfWork database,
        IWebHostEnvironment env,
        JwtTokenGenerator generator)
    {
        this.dataBase = database;
        this.env = env;
        this.generator = generator;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
    {
        await validationUser(request);

        Image? image = await generateImage(request);

        User user = createUser(request, image);

        await saveInDataBase(user);

        CreateUserResponse response = new CreateUserResponse
        {
            id = user.Id,
            name = user.Name,
            avatarUrl = user.GetAvatarurl(),
            userName = user.UserName
        };

        return Ok(new ResponseDTO<CreateUserResponse>
        {
            code = (int)HttpStatusCode.OK,
            message = "Usuario guardado correctamente",
            success = true,
            payload = response
        });
    }

    private async Task saveInDataBase(User user)
    {
        await dataBase.UserRepository.Create(user);
        await this.dataBase.SaveChangesAsync();
    }

    private static User createUser(CreateUserRequest request, Image? image)
    {
        User user = new User
        {
            Name = request.name,
            UserName = request.userName,
            DateBirth = request.dateBirth,
            Image = image
        };

        user.SetPassword(request.password);
        return user;
    }

    private async Task<Image?> generateImage(CreateUserRequest request)
    {
        if (request.file is null) return null;

        FileStorageService fileStorageService = new FileStorageService(env);
        return await fileStorageService.SaveImageAsync(request.file);
    }

    private async Task validationUser(CreateUserRequest request)
    {
        if (String.IsNullOrEmpty(request.name))
        {
            throw new ValidationException("El nombre de la persona es un dato obligatorio");
        }

        if (String.IsNullOrEmpty(request.password))
        {
            throw new ValidationException("La contraseña es un dato obligatorio");
        }

        if (String.IsNullOrEmpty(request.userName))
        {
            throw new ValidationException("El nombre de usuario es un dato obligatorio");
        }

        User? existedUser = await this.dataBase.UserRepository.GetuserByUserName(request.userName);

        if (existedUser != null)
        {
            throw new ValidationException("Ya existe un usuario con el nombre " + request.userName);
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User user = await validationLogin(request);

        string token = this.generator.GenerateToken(user.Id);

        LoginResponse response = new LoginResponse
        {
            id = user.Id,
            token = token,
            urlAvatar = user.GetAvatarurl(),
            userName = user.UserName
        };

        return Ok(new ResponseDTO<LoginResponse>
        {
            code = (int)HttpStatusCode.OK,
            message = "Login exitoso",
            success = true,
            payload = response
        });
    }

    private async Task<User> validationLogin(LoginRequest request)
    {
        string userName = request.UserName ?? string.Empty;

        if (String.IsNullOrEmpty(userName))
        {
            throw new InvalidCredentialsException("Nombre de usuario es un dato obligatorio");
        }

        if (String.IsNullOrEmpty(request.Password))
        {
            throw new InvalidCredentialsException("La clave es un dato obligatorio");
        }

        User? user = await this.dataBase.UserRepository.GetuserByUserName(userName);
        if (user == null)
        {
            throw new InvalidCredentialsException("Nombre de usuario o contraseña incorrectos");
        }
        if (!user.IsPassword(request.Password))
        {
            throw new InvalidCredentialsException("Nombre de usuario o contraseña incorrectos");
        }

        await validationBan(user);

        return user;
    }

    private async Task validationBan(User user)
    {
        Ban? ban = await this.dataBase.BanRepository.GetBanByUserId(user.Id);

        if (ban != null)
        {
            throw new BusinessForbiddenException("Tu cuenta esta baneada: " + ban.Reason);
        }
    }
}
