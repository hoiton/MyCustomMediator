using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMediator.Sample
{
    public class PrintToConsoleRequest : IRequest
    {
        public string Text { get; set; }
    }
}
