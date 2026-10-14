/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

namespace Datagaucha.Domain.Auth;

public class Person
{
    private long id;

    public long Id { get => id; set => id = value; }

    private string name = string.Empty;

    public string Name { get => name; set => name = value; }
}
