using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCalculator
{
    public class JarOfKiopolu
    {
        public JarOfKiopolu()
        {
            Peppers = 200;
            Tomatoes = 200;
            Eggplant = 300;
            Salt = 25;
            Onion = 50;
            Garlic = 25;
        }

        public double Peppers { get; set; }

        public double Tomatoes { get; set; }

        public double Eggplant { get; set; }

        public double Salt { get; set; }

        public double Onion { get; set; }

        public double Garlic { get; set; }
    }
}
