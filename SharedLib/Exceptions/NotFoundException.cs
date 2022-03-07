using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string id) : base($"Item not found {id}")
        {
        }
    }
}
