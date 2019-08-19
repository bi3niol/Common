using Common.Standard.Messages;
using Common.Standard.Types;
using System.Threading.Tasks;

namespace Common.Standard.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext correlationContext);
    }
}
