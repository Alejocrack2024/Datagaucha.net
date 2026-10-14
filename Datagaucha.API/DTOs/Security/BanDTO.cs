/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

public class BanDTO
{
    public long id { get; set; }
    public long userId { get; set; }
    public string userName { get; set; } = "";
    public string reason { get; set; } = "";
    public DateTime bannedAt { get; set; }
}
