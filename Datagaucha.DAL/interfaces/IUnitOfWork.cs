/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Auth;
using Datagaucha.DAL.interfaces.FileSystem;
using Datagaucha.DAL.interfaces.Post;
using Datagaucha.DAL.interfaces.Security;
using Datagaucha.DAL.interfaces.Social;
using Datagaucha.Domain.Auth;

namespace Datagaucha.DAL.interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IUserRepository UserRepository { get; }

    IPersonRepository PersonRepository { get; }

    IFileRepository FileRepository { get; }

    IImageRepository ImageRepository { get; }

    IPostRepository PostRepository { get; }

    ICommentRepository CommentRepository { get; }

    IReactionRepository ReactionRepository { get; }

    IFollowRepository FollowRepository { get; }

    IBanRepository BanRepository { get; }
}
