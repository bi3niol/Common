using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Messages
{
    public interface IRejectedEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}
