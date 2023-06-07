using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers
{
    public interface IFurni
    {
        int Id { get; set; }
        string Name { get; set; }

        void Place();
        void Remove();
        void Interact();
    }

}
