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
        return await this.dbContext.Users
            .Include(u => u.Image)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public Task<User?> GetuserByUserName(string userName)
    {
        return this.dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<List<User>> GetUsers(int currentPage, int pageSize, string? search)
    {
        DateTime now = DateTime.UtcNow;

        // IDs de usuarios con ban activo:
        // - ExpiresAt == null  => ban permanente
        // - ExpiresAt > now    => ban temporal todavía vigente
        List<long> bannedIds = await this.dbContext.Bans
            .Where(ban => ban.User != null
                && (ban.ExpiresAt == null || ban.ExpiresAt > now))
            .Select(ban => ban.User!.Id)
            .ToListAsync();

        IQueryable<User> users = this.dbContext.Users
            .Include(u => u.Image)
            .Where(u => !bannedIds.Contains(u.Id));

        if (!String.IsNullOrWhiteSpace(search))
        {
            users = users.Where(u => u.UserName.Contains(search));
        }

        return await users
            .OrderBy(u => u.UserName)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}