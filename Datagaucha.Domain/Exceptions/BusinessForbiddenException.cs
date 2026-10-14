/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

namespace Datagaucha.Domain.Exceptions;

public class BusinessForbiddenException : BusinessException
{
    public BusinessForbiddenException(string message)
        : base(message)
    {
    }
}
