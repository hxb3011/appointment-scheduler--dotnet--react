using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.ImplInterface
{
    public class IDiagnosticService : IDiagnosticService
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}