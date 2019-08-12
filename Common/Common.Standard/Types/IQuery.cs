using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Types
{
    public interface IQuery { }
    public interface IQuery<TResult> : IQuery
    {
    }
}
