using System;

namespace ApplicationCore.Exceptions
{
    public class CarExistsException: Exception
    {
        public CarExistsException()
            : base("Car is already added")
        { }
    }  
    public class CarNotExistsException: Exception
    {
        public CarNotExistsException()
            : base("Сначала добавьте машину!")
        { }
    }
}