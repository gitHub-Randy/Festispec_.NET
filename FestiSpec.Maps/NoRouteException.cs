using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Maps
{
    /// Thrown when a route is calculated between two points where one or both points are invalid
    public class NoRouteException : Exception
    {
    }
}
