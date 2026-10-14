/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Auth;
using Datagaucha.Domain.Auth;
using Microsoft.EntityFrameworkCore;
namespace Datagaucha.DAL.EntityFramework.Auth;

public class EFUserRepository(DatagauchaDbContext dbContext) : IUserRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<bool> Create(User user)
    {
        await this.dbContext.Users.AddAsync(user);
        return true;
    }

    public async Task<User?> GetuserById(long userId)
    {
        return await this.dbContext.Users.FindAsync(userId);
    }

    public Task<User?> GetuserByUserName(string userName)
    {
        return this.dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }
}