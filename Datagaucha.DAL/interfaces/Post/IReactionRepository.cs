/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Post;

namespace Datagaucha.DAL.interfaces.Post;

public interface IReactionRepository
{
    Task<Reaction?> GetReaction(long userId, long postId);

    Task<Dictionary<long, ReactionType>> GetReactionsByUserAndPostIds(
        long userId,
        List<long> postIds);

    Task<bool> Create(Reaction reaction);

    Task<bool> Delete(Reaction reaction);
}
