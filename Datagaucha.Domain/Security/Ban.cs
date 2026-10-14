/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.Domain.Security;

public class Ban
{
    private long id;

    public long Id { get => id; set => id = value; }

    private User? user;

    public virtual User? User { get => user; set => user = value; }

    private Person? person;

    public virtual Person? Person { get => person; set => person = value; }

    private string reason = string.Empty;

    public string Reason { get => reason; set => reason = value; }

    private DateTime bannedAt = DateTime.Now;

    public DateTime BannedAt { get => bannedAt; set => bannedAt = value; }

    public string GetUserName()
    {
        if (this.User != null) return this.User.UserName;
        return "Usuario no encontrado";
    }
}
