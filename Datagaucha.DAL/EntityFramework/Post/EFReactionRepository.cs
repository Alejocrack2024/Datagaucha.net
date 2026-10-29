/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Post;
using Datagaucha.Domain.Post;
using Microsoft.EntityFrameworkCore;

namespace Datagaucha.DAL.EntityFramework.Post;

public class EFReactionRepository(DatagauchaDbContext dbContext) : IReactionRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public Task<Reaction?> GetReaction(long userId, long postId)
    {
        return this.dbContext.Reactions
            .FirstOrDefaultAsync(reaction =>
                reaction.User != null && reaction.User.Id == userId
                && reaction.Post != null && reaction.Post.Id == postId);
    }

    public async Task<Dictionary<long, ReactionType>> GetReactionsByUserAndPostIds(
        long userId,
        List<long> postIds)
    {
        return await this.dbContext.Reactions
            .Where(reaction =>
                reaction.User.Id == userId
                && postIds.Contains(reaction.Post.Id))
            .ToDictionaryAsync(
                reaction => reaction.Post.Id,
                reaction => reaction.Type);
    }

    public async Task<bool> Create(Reaction reaction)
    {
        await this.dbContext.Reactions.AddAsync(reaction);
        return true;
    }

    public Task<bool> Delete(Reaction reaction)
    {
        this.dbContext.Reactions.Remove(reaction);
        return Task.FromResult(true);
    }
}
