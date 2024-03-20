using MediatR;

namespace NSE.Cliente.API.Application.Events
{
    public class ClientEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            //Enviar um evento de confirmação
            return Task.CompletedTask;
        }
    }
}
