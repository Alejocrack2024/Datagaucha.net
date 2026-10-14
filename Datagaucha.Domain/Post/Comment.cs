/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.Domain.Post;

public class Comment
{
    private long id;

    public long Id { get => id; set => id = value; }

    private Post? post;

    public virtual Post? Post { get => post; set => post = value; }

    private User? user;

    public virtual User? User { get => user; set => user = value; }

    private string text = string.Empty;

    public string Text { get => text; set => text = value; }

    private DateTime createdAt = DateTime.Now;

    public DateTime CreatedAt { get => createdAt; set => createdAt = value; }

    public string GetUserName()
    {
        if (this.User != null) return this.User.UserName;
        return "Usuario no encontrado";
    }

    public string? GetAvatarUrl()
    {
        if (this.User != null) return this.User.GetAvatarurl();
        return null;
    }
}
