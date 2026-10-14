/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

namespace Datagaucha.Domain.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {
    }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}