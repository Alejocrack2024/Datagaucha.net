/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Social;

namespace Datagaucha.DAL.interfaces.Social;

public interface IFollowRepository
{
    Task<List<User>> GetFollowers(long userId, int currentPage, int pageSize);

    Task<List<User>> GetFollowing(long userId, int currentPage, int pageSize);

    Task<Follow?> GetFollow(long followerId, long followingId);

    Task<bool> IsFollowing(long followerId, long followingId);

    Task<bool> Create(Follow follow);

    Task<bool> Delete(Follow follow);
}
