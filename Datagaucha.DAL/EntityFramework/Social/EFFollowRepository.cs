/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Social;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Social;
using Microsoft.EntityFrameworkCore;
namespace Datagaucha.DAL.EntityFramework.Social;

public class EFFollowRepository(DatagauchaDbContext dbContext) : IFollowRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<List<User>> GetFollowers(long userId, int currentPage, int pageSize)
    {
        List<User> followers = await this.dbContext.Follows
            .Where(follow => follow.Following != null && follow.Following.Id == userId)
            .Select(follow => follow.Follower!)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return followers;
    }

    public async Task<List<User>> GetFollowing(long userId, int currentPage, int pageSize)
    {
        List<User> following = await this.dbContext.Follows
            .Where(follow => follow.Follower != null && follow.Follower.Id == userId)
            .Select(follow => follow.Following!)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return following;
    }

    public async Task<Follow?> GetFollow(long followerId, long followingId)
    {
        List<Follow> follows = await this.dbContext.Follows
            .Where(follow => follow.Follower != null && follow.Follower.Id == followerId
                && follow.Following != null && follow.Following.Id == followingId)
            .ToListAsync();

        if (follows.Count > 0) return follows[0];

        return null;
    }

    public async Task<bool> IsFollowing(long followerId, long followingId)
    {
        Follow? follow = await this.GetFollow(followerId, followingId);

        if (follow != null) return true;

        return false;
    }

    public async Task<bool> Create(Follow follow)
    {
        await this.dbContext.Follows.AddAsync(follow);
        return true;
    }

    public Task<bool> Delete(Follow follow)
    {
        this.dbContext.Follows.Remove(follow);
        return Task.FromResult(true);
    }
}
