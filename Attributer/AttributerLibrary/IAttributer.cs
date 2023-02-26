using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributerLibrary
{
    public interface IAttributer
    {
        void OnBeforeExecute(AttributerContext context);
        void OnAfterExecuted(AttributerContext context);
    }
}
