using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyMediator.Sample
{
    public class PrintToConsoleHandler : RequestHandler<PrintToConsoleRequest>
    {
        public override void Handle(PrintToConsoleRequest request)
        {
            Console.WriteLine(request.Text);
        }
    }
}
