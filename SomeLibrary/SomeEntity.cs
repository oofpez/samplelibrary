using System;
using System.Collections.Generic;
using System.Text;

namespace SomeLibrary
{
    public class SomeEntity
    {
        public int Id { get; private set; }

        //TODO: This should be immutable
        public string Name { get; set; }

        public SomeEntity(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
