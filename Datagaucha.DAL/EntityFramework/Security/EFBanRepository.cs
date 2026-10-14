/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Security;
using Datagaucha.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace Datagaucha.DAL.EntityFramework.Security;

public class EFBanRepository(DatagauchaDbContext dbContext) : IBanRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<List<Ban>> GetBans(int currentPage, int pageSize)
    {
        List<Ban> bans = await this.dbContext.Bans
            .OrderByDescending(ban => ban.BannedAt)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return bans;
    }

    public async Task<Ban?> GetBanById(long banId)
    {
        return await this.dbContext.Bans.FindAsync(banId);
    }

    public async Task<Ban?> GetBanByUserId(long userId)
    {
        List<Ban> bans = await this.dbContext.Bans
            .Where(ban => ban.User != null && ban.User.Id == userId)
            .ToListAsync();

        if (bans.Count > 0) return bans[0];

        return null;
    }

    public async Task<bool> Create(Ban ban)
    {
        await this.dbContext.Bans.AddAsync(ban);
        return true;
    }

    public Task<bool> Delete(Ban ban)
    {
        this.dbContext.Bans.Remove(ban);
        return Task.FromResult(true);
    }
}
