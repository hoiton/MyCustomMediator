using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMediator
{
    public interface IRequest : IRequest<Unit> { }

    public interface IRequest<out TResponse>{ }

    //internal interface IBaseRequest { }
}
