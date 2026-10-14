/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

namespace Datagaucha.Domain.FileSystem;

public class Image : File
{
    private int width = 0;

    public int Width { get => width; set => width = value; }

    private int height = 0;

    public int Height { get => height; set => height = value; }

    public override string? ImageUrl()
    {
        return "api/images/" + this.FileName;
    }
}
