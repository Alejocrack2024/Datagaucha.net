/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.FileSystem;
using Datagaucha.Domain.FileSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Datagaucha.DAL.EntityFramework.FileSystem;

public class EFFileRepository : IFileRepository
{
    private DatagauchaDbContext dbContext;
    public EFFileRepository(DatagauchaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Datagaucha.Domain.FileSystem.File?> GetById(long id)
    {
        Datagaucha.Domain.FileSystem.File? file = await this.dbContext.Files.FindAsync(id);
        return file;
    }

    public async Task<Domain.FileSystem.File?> GetByName(string name)
    {
        List<Datagaucha.Domain.FileSystem.File> files = await this.dbContext.Files.Where(file => file.FileName.Trim().ToUpper(System.Globalization.CultureInfo.CurrentCulture).Equals(name.Trim().ToUpper())).ToListAsync();
        if (files?.Count > 0) return files[0];

        return null;
    }
}