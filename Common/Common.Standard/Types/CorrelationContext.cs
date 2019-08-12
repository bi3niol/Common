using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Types
{
    public class CorrelationContext : ICorrelationContext
    {
        public static ICorrelationContext Empty => new CorrelationContext();

        public Guid Id { get; }
    }
}
