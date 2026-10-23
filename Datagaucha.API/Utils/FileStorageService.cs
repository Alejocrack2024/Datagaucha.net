/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.FileSystem;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;

public class FileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _storagePath;
    private readonly string _relativeStoragePath;

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
        _relativeStoragePath = Path.Combine("Storage", "Images");
        _storagePath = Path.Combine(_env.ContentRootPath, _relativeStoragePath);
    }

    public async Task<Image> SaveImageAsync(IFormFile file)
    {
        await using Stream stream = file.OpenReadStream();
        using MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        return await SaveImageAsync(memoryStream.ToArray(), file.FileName);
    }

    public Task<Image> SaveImageAsync(byte[] data, string fileName)
    {
        Directory.CreateDirectory(_storagePath);

        var extension = Path.GetExtension(fileName);
        var finalFileName = $"{Guid.NewGuid()}{extension}";
        var physicalPath = Path.Combine(_storagePath, finalFileName);
        var relativeStoragePath = Path.Combine(_relativeStoragePath, finalFileName);

        using var image = new MagickImage(data);

        image.Format = MagickFormat.WebP;
        image.Write(physicalPath);

        return Task.FromResult(new Image
        {
            FileName = finalFileName,
            ContentType = "image/webp",
            StoragePath = relativeStoragePath,
            Width = (int)image.Width,
            Height = (int)image.Height
        });
    }

    public string GetPhysicalPath(string storagePath)
    {
        if (string.IsNullOrWhiteSpace(storagePath))
        {
            return string.Empty;
        }

        if (Path.IsPathRooted(storagePath))
        {
            return storagePath;
        }

        return Path.Combine(_env.ContentRootPath, storagePath);
    }
}
