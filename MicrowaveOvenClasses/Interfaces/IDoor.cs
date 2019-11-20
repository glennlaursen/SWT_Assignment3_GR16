using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrowaveOvenClasses.Interfaces
{
    public interface IDoor
    {
        event EventHandler Opened;
        event EventHandler Closed;

        void Open();
        void Close();
    }
}
