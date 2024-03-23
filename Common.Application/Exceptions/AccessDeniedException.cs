﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Exceptions;

public class AccessDeniedException : HttpException
{
    public AccessDeniedException() : base(new { Message = "Access denied" }, HttpStatusCode.Forbidden)
    {
    }
}

