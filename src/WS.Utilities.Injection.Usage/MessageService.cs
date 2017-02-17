using Serilog.Core;

namespace WS.Utilities.Injection.Usage
{
    public class MessageService : IMessageService
    {
        private readonly Logger _logger;

        public MessageService(Logger logger)
        {
            _logger = logger;
        }

        public void SayHello(string name)
        {
            _logger.Information("Hello {name}", name);
        }
    }
}
