using Abstractions.Commands;
using System.Threading.Tasks;

namespace Core
{
    public class SetRendezvousPointExecutor : CommandExecutorBase<ISetRendezvousPointCommand>
    {
        public override async Task ExecuteSpecificCommand(ISetRendezvousPointCommand command)
        {
            GetComponent<MainBuilding>().RendezvousPoint = command.RendezvousPoint;
        }
    }
}