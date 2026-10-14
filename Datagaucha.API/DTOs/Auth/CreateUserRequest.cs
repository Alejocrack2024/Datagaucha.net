/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

public class CreateUserRequest
{
    public string name { get; set; } = "";

    public string userName { get; set; } = "";

    public string password { get; set; } = "";

    public DateTime? dateBirth { get; set; }

    public IFormFile? file { get; set; }
}
