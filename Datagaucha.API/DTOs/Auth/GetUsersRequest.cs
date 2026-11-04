/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

public class GetUsersRequest
{
    public int currentPage { get; set; } = 1;
    public int pageSize { get; set; } = 12;
    public string search { get; set; } = string.Empty;
}