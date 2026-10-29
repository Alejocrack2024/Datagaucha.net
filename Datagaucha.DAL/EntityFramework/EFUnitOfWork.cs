/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.EntityFramework.Auth;
using Datagaucha.DAL.EntityFramework.FileSystem;
using Datagaucha.DAL.EntityFramework.Post;
using Datagaucha.DAL.EntityFramework.Security;
using Datagaucha.DAL.EntityFramework.Social;
using Datagaucha.DAL.interfaces;
using Datagaucha.DAL.interfaces.Auth;
using Datagaucha.DAL.interfaces.FileSystem;
using Datagaucha.DAL.interfaces.Post;
using Datagaucha.DAL.interfaces.Security;
using Datagaucha.DAL.interfaces.Social;
using Microsoft.EntityFrameworkCore;

namespace Datagaucha.DAL.EntityFramework;

public class EFUnitOfWork(DatagauchaDbContext context) : IUnitOfWork
{
    private readonly DatagauchaDbContext _context = context;

    private IUserRepository? userRepository;

    public IUserRepository UserRepository
    {
        get
        {
            if (this.userRepository is null)
            {
                this.userRepository = new EFUserRepository(_context);
            }
            return this.userRepository;
        }
    }

    private IPersonRepository? personRepository;

    public IPersonRepository PersonRepository
    {
        get
        {
            if (this.personRepository is null)
            {
                this.personRepository = new EFPersonRepository(_context);
            }
            return this.personRepository;
        }
    }

    private IFileRepository? fileRepository;

    public IFileRepository FileRepository
    {
        get
        {
            if (this.fileRepository is null)
            {
                this.fileRepository = new EFFileRepository(_context);
            }
            return this.fileRepository;
        }
    }

    private IImageRepository? imageRepository;

    public IImageRepository ImageRepository
    {
        get
        {
            if (this.imageRepository is null)
            {
                this.imageRepository = new EFImageRepository(_context);
            }
            return this.imageRepository;
        }
    }

    private IPostRepository? postRepository;

    public IPostRepository PostRepository
    {
        get
        {
            if (this.postRepository is null)
            {
                this.postRepository = new EFPostRepository(_context);
            }
            return this.postRepository;
        }
    }

    private ICommentRepository? commentRepository;

    public ICommentRepository CommentRepository
    {
        get
        {
            if (this.commentRepository is null)
            {
                this.commentRepository = new EFCommentRepository(_context);
            }
            return this.commentRepository;
        }
    }

    private IReactionRepository? reactionRepository;

    public IReactionRepository ReactionRepository
    {
        get
        {
            if (this.reactionRepository is null)
            {
                this.reactionRepository = new EFReactionRepository(_context);
            }
            return this.reactionRepository;
        }
    }

    private IFollowRepository? followRepository;

    public IFollowRepository FollowRepository    {
        get
        {
            if (this.followRepository is null)
            {
                this.followRepository = new EFFollowRepository(_context);
            }
            return this.followRepository;
        }
    }

    private IBanRepository? banRepository;

    public IBanRepository BanRepository
    {
        get
        {
            if (this.banRepository is null)
            {
                this.banRepository = new EFBanRepository(_context);
            }
            return this.banRepository;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
