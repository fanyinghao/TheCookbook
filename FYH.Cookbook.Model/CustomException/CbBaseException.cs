using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYH.Cookbook.Model.CustomException
{
    public abstract class CbBaseException : Exception
    {
        protected CbBaseException() { }

        protected CbBaseException(string message) : base(message) { }

        protected CbBaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
