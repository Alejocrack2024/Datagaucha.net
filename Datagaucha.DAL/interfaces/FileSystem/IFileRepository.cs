/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.FileSystem;
namespace Datagaucha.DAL.interfaces.FileSystem;

public interface IFileRepository
{
    Task<Datagaucha.Domain.FileSystem.File?> GetById(long id);
    Task<Domain.FileSystem.File?> GetByName(string id);
}