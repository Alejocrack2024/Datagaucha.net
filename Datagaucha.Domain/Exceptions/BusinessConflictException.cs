/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using Datagaucha.Domain.Exceptions;
namespace Datagaucha.Domain.Exceptions;

public class BusinessConflictException : BusinessException
{
    public BusinessConflictException(string message)
        : base(message)
    {
    }
}