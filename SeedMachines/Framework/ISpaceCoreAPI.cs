using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMachines.Framework
{
    public interface ISpaceCoreAPI
    {
        void RegisterSerializerType(Type type);
    }
}
