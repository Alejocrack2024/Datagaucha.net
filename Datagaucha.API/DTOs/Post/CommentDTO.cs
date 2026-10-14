/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

public class CommentDTO
{
    public long id { get; set; }
    public long postId { get; set; }
    public long userId { get; set; }
    public string userName { get; set; } = "";
    public string? urlAvatar { get; set; }
    public string text { get; set; } = "";
    public DateTime createdAt { get; set; }
}
