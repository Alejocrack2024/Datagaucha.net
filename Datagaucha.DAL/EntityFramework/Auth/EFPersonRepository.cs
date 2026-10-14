/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces.Auth;
using Datagaucha.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Datagaucha.DAL.EntityFramework.Auth;

public class EFPersonRepository(DatagauchaDbContext dbContext) : IPersonRepository
{
    private readonly DatagauchaDbContext dbContext = dbContext;

    public async Task<List<Person>> GetPeople(int currentPage, int pageSize)
    {
        List<Person> people = await this.dbContext.People
            .OrderBy(person => person.Name)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return people;
    }

    public async Task<Person?> GetPersonById(long personId)
    {
        return await this.dbContext.People.FindAsync(personId);
    }

    public async Task<Person?> GetPersonByName(string name)
    {
        List<Person> people = await this.dbContext.People
            .Where(person => person.Name.ToUpper().Equals(name.ToUpper()))
            .ToListAsync();

        if (people.Count > 0) return people[0];

        return null;
    }

    public async Task<bool> Create(Person person)
    {
        await this.dbContext.People.AddAsync(person);
        return true;
    }
}
