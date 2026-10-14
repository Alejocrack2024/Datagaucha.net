/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */


namespace Datagaucha.Domain.FileSystem;

public abstract class File
{
    private long id;

    public long Id { get => id; set => id = value; }

    private string fileName = string.Empty;

    public string FileName { get => fileName; set => fileName = value; }

    private string contentType = string.Empty;

    public string ContentType { get => contentType; set => contentType = value; }

    private string storagePath = string.Empty;

    public string StoragePath { get => storagePath; set => storagePath = value; }

    public abstract string? ImageUrl();
}