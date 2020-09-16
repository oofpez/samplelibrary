using System;
using System.Collections.Generic;
using System.IO;

namespace SomeLibrary
{
    public class SomeClass
    {
        Dictionary<int, SomeEntity> _entities;

        string _writePathRoot = "C:/filedump/";

        public SomeClass()
        {
            _entities = new Dictionary<int, SomeEntity>();
        }


        [Memoizer.Memoized]
        public SomeEntity GetValue(int id)
        {
            if (id == 0)
            {
                return null;
            }
            if (_entities.TryGetValue(id, out var result))
            {
                return result;
            }
            return null;
        }

        //Store the entity in memory and on disk. Null if not existing.
        public SomeEntity UpdateNameAndWriteFile(SomeEntity entity)
        {
            if (entity.Id == 0)
            {
                return null;
            }

            var existing = GetValue(entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                File.Delete($"{ _writePathRoot}{ entity.Id}");
                File.WriteAllText($"{_writePathRoot}{entity.Id}", entity.ToString());
                return existing;              
            }
            _entities.Add(entity.Id,entity);
            File.WriteAllText($"{_writePathRoot}{entity.Id}", entity.ToString());
            return existing;
        }
    }
}
