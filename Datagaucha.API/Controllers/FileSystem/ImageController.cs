/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Microsoft.AspNetCore.Mvc;
using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Exceptions;
using Datagaucha.Domain.FileSystem;

namespace Datagaucha.API.Controllers;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private IUnitOfWork dataBase;
	private IWebHostEnvironment env;
    
    public ImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        this.dataBase = unitOfWork;
        this.env = env;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(string id, CancellationToken cancellationToken)
    {
        Datagaucha.Domain.FileSystem.File? file = await this.dataBase.FileRepository.GetByName(id);
        if (file is null)
        {
            throw new BusinessNotFoundException("La imagen no existe en la base de datos");
        }

		FileStorageService fileStorage = new FileStorageService(this.env);
        var physicalPath = fileStorage.GetPhysicalPath(file.StoragePath);

        if (string.IsNullOrWhiteSpace(physicalPath) || !System.IO.File.Exists(physicalPath))
        {
            throw new BusinessNotFoundException("No se encontró la imagen fisica");
        }

        var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
        return PhysicalFile(physicalPath, contentType);
    }
}