/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.Domain.Post;

public class Post
{
    public long Id { get; set; }

    private string title = string.Empty;

    public string Title { get => title; set => title = value; }

    private string body = string.Empty;

    public string Body { get => body; set => body = value; }

    private Datagaucha.Domain.FileSystem.File? file;

    public virtual Datagaucha.Domain.FileSystem.File? File { get => file; set => file = value; }

    private User? user;

    public virtual User? User { get => user; set => user = value; }

    private List<Comment> comments = new List<Comment>();

    public virtual List<Comment> Comments { get => comments; set => comments = value; }

    public string GetUserName()
    {
        if (this.User != null) return this.User.UserName;
        return "Usuario no encontrado";
    }

    public string? ImageUrl()
    {
        if (this.File != null) return this.File.ImageUrl();
        return null;
    }

    public void AddComment(Comment comment)
    {
        comment.Post = this;
        this.comments.Add(comment);
    }
}
