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

    // Update (faltaba en el archivo anterior)
    public Task Update(User user)
    {
        this.dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<List<User>> GetUsers(int currentPage, int pageSize, string search)
    {
        IQueryable<User> query = this.dbContext.Users
            .Include(u => u.Image);

        if (!string.IsNullOrWhiteSpace(search))
        {
            string s = search.Trim().ToLower();

            query = query.Where(u =>
                u.UserName.ToLower().Contains(s) ||
                u.Name.ToLower().Contains(s));
        }

        return await query
            .OrderBy(u => u.UserName)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}