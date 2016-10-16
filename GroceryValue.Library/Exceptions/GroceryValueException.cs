using System;

namespace GroceryValue.Library
{

    /*
     * Custom exceptions are rarely justified, they are counter intuitive and non common,
     * If a piece of code does something wrong, it is easier to expect and handle a common .NET exception.
       Which is what most developers expect from your code to throw- such as InvalidOperationException which almost always qualifies
       You should at least have your exceptions derive from common ones so that they can be handled in higher, more unaware layers.

        Consider: https://msdn.microsoft.com/en-us/library/ms229007(v=vs.100).aspx
     */

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
