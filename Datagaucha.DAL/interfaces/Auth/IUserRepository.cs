/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Auth;

namespace Datagaucha.DAL.interfaces.Auth;

public interface IUserRepository
{
    Task<User?> GetuserByUserName(string userName);

    Task<bool> Create(User user);
    
    Task Update(User user);
    
    Task<User?> GetuserById(long userId);

    Task<List<User>> GetUsers(int currentPage, int pageSize, string search);
}