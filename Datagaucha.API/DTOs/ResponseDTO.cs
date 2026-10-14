/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

public class ResponseDTO<T>
{
    public bool success { get; set; }
    public string? message { get; set; }
    public int code { get; set; }

    public T? payload { get; set; }
}