/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Security;

namespace Datagaucha.DAL.interfaces.Security;

public interface IBanRepository
{
    Task<List<Ban>> GetBans(int currentPage, int pageSize);

    Task<Ban?> GetBanById(long banId);

    Task<Ban?> GetBanByUserId(long userId);

    Task<bool> Create(Ban ban);

    Task<bool> Delete(Ban ban);
}
