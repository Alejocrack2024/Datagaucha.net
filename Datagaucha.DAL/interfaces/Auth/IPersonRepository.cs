/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

namespace Datagaucha.DAL.interfaces.Auth;

using Datagaucha.Domain.Auth;

public interface IPersonRepository
{
    Task<List<Person>> GetPeople(int currentPage, int pageSize);

    Task<Person?> GetPersonById(long personId);

    Task<Person?> GetPersonByName(string name);

    Task<bool> Create(Person person);
}
