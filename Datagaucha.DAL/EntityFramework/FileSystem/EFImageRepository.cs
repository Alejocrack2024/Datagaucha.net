/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.FileSystem;
using Datagaucha.Domain.FileSystem;
namespace Datagaucha.DAL.EntityFramework.FileSystem;

public class EFImageRepository(DatagauchaDbContext dbContext) : IImageRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<Image?> GetById(long imageId)
    {
        return await this.dbContext.Images.FindAsync(imageId);
    }

    public async Task<bool> Create(Image image)
    {
        await this.dbContext.Images.AddAsync(image);
        return true;
    }
}
