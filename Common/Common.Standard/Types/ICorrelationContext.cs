using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Types
{
    public interface ICorrelationContext
    {
        Guid Id { get; }
    }
}
