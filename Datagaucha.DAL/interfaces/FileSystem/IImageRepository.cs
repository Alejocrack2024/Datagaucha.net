/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.FileSystem;

namespace Datagaucha.DAL.interfaces.FileSystem;

public interface IImageRepository
{
    Task<Image?> GetById(long imageId);

    Task<bool> Create(Image image);
}
