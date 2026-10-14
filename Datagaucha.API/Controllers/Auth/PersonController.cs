/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.DAL.interfaces;
using Datagaucha.Domain.Auth;
using Datagaucha.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Datagaucha.API.Controllers.Auth;

[ApiController]
[Route("api/person")]
public class PersonController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork dataBase = unitOfWork;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonRequest request)
    {
        validationName(request.name);
        await validationDuplicate(request.name);

        Person person = new Person
        {
            Name = request.name.Trim()
        };

        await this.dataBase.PersonRepository.Create(person);
        await this.dataBase.SaveChangesAsync();

        return Ok(new ResponseDTO<PersonDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Persona creada correctamente",
            success = true,
            payload = buildDTO(person)
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetPeople([FromQuery] PagedRequest request)
    {
        validationPaged(request.currentPage, request.pageSize);

        List<Person> people = await this.dataBase.PersonRepository.GetPeople(request.currentPage, request.pageSize);

        List<PersonDTO> personDTOs = new List<PersonDTO>();
        foreach (Person person in people)
        {
            personDTOs.Add(buildDTO(person));
        }

        return Ok(new ResponseDTO<List<PersonDTO>>
        {
            code = (int)HttpStatusCode.OK,
            message = "Personas obtenidas correctamente",
            success = true,
            payload = personDTOs
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        Person? person = await this.dataBase.PersonRepository.GetPersonById(id);

        if (person is null)
        {
            throw new BusinessNotFoundException("La persona no existe");
        }

        return Ok(new ResponseDTO<PersonDTO>
        {
            code = (int)HttpStatusCode.OK,
            message = "Persona obtenida correctamente",
            success = true,
            payload = buildDTO(person)
        });
    }

    private static void validationName(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("El nombre de la persona es un dato obligatorio");
        }
    }

    private async Task validationDuplicate(string name)
    {
        Person? existedPerson = await this.dataBase.PersonRepository.GetPersonByName(name.Trim());

        if (existedPerson != null)
        {
            throw new BusinessConflictException("Ya existe una persona con el nombre " + name.Trim());
        }
    }

    private static void validationPaged(int currentPage, int pageSize)
    {
        if (currentPage < 1)
        {
            throw new ValidationException("La pagina debe ser mayor o igual a 1");
        }

        if (pageSize < 1)
        {
            throw new ValidationException("El tamaño de pagina debe ser mayor o igual a 1");
        }
    }

    private static PersonDTO buildDTO(Person person)
    {
        return new PersonDTO
        {
            id = person.Id,
            name = person.Name
        };
    }
}
