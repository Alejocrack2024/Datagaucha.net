/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.Domain.Post;

public enum ReactionType
{
    Like,
    Dislike
}

public class Reaction
{
    private long id;

    public long Id { get => id; set => id = value; }

    private User user = null!;

    public virtual User User { get => user; set => user = value; }

    private Post post = null!;

    public virtual Post Post { get => post; set => post = value; }

    private ReactionType type;

    public ReactionType Type { get => type; set => type = value; }
}
