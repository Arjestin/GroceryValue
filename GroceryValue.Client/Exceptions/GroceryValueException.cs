using System;

namespace GroceryValue.Client
{
    public class GroceryValueException : Exception
    {
        public GroceryValueException()
        {
        }

        public GroceryValueException(string message) : base(message)
        {
        }

        public GroceryValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
