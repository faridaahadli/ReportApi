﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.ExceptionHandle.Models;

public class ForbiddenException : Exception
{
    public ForbiddenException(string msg)
       : base(msg) { }
}
