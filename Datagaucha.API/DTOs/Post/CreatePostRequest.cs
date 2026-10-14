/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

public class CreatePostRequest
{
    public string title { get; set; } = "";
    public string body { get; set; } = "";
    public IFormFile? file { get; set; }
}
