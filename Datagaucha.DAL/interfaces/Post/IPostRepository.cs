/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.DAL.interfaces.Post;

public interface IPostRepository
{
    Task<List<Domain.Post.Post>> GetPosts(User user,
        int currentPage,
        int pageSize,
        string orderBy,
        string orderDirection,
        string? search);

    Task<Domain.Post.Post?> GetPostById(long postId);

    Task<bool> Create(Domain.Post.Post post);

    Task<bool> Delete(Domain.Post.Post post);
}
