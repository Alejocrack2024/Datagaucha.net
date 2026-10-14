/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.Domain.Social;

public class Follow{
    private long id;

    public long Id { get => id; set => id = value; }

    private User? follower;

    public virtual User? Follower { get => follower; set => follower = value; }

    private User? following;

    public virtual User? Following { get => following; set => following = value; }
}