using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrowaveOvenClasses.Interfaces
{
    public interface ICookController
    {
        void StartCooking(int power, int time);
        void Stop();
    }
}
