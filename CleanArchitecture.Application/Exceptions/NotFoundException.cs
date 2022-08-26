using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Exceptions
{
    // Heredamso del propio system
    internal class NotFoundException : ApplicationException
    {
        // Constructor
        // Key hace referencia al ID
        // Con base estamos pasando estos valores hacia la clase padre, Applicationexception
        public NotFoundException(string name, object key) :base ($"Entity \"{name}\" ({key}) no fue encontrado")
        {
        }
    }
}
