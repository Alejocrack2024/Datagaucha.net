/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Post;

namespace Datagaucha.DAL.interfaces.Post;

public interface ICommentRepository
{
    Task<List<Comment>> GetCommentsByPostId(long postId, int currentPage, int pageSize);

    Task<Comment?> GetCommentById(long commentId);

    Task<bool> Create(Comment comment);

    Task<bool> Delete(Comment comment);
}
