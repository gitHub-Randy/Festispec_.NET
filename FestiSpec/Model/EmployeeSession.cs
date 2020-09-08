using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Model
{
    public sealed class EmployeeSession
    {
        private static EmployeeSession instance = null;
        private static readonly object padlock = new object();

        public EmployeeAccount User { get; set; }

        EmployeeSession()
        {
        }

        public static EmployeeSession Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EmployeeSession();
                    }
                    return instance;
                }
            }
        }
    }
}
