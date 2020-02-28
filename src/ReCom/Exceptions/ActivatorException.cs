using System;

namespace ReCom.Exceptions
{
    public class ActivatorException : Exception
    {
        public ActivatorException(string message) : base(message)
        {
            
        }
    }

    public class ActivatorCircularDependencyException : ActivatorException
    {
        public ActivatorCircularDependencyException(string message) : base(message)
        {
        }
    }

    public class ActivatorServiceNotFoundException : ActivatorException
    {
        public ActivatorServiceNotFoundException(string message) : base(message)
        {
        }
    }
}
