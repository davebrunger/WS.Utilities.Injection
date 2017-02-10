using Serilog.Core;

namespace WS.Utilities.Injection.Usage
{
    internal class AddService : IAddService
    {
        private readonly Logger _logger;

        public AddService(Logger logger)
        {
            _logger = logger;
        }

        public int Add(int op1, int op2)
        {
            _logger.Debug("About to add {op1} and {op2}", op1, op2);
            var result = op1 + op2;
            _logger.Debug("Added {op1} to {op2} and the result was {result}", op1, op2, result);
            return result;
        }
    }
}
