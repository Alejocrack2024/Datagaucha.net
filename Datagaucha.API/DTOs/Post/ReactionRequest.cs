/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Post;

public class ReactionRequest
{
    public long postId { get; set; }
    public ReactionType type { get; set; }
}
