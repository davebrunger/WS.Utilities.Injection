namespace WS.Utilities.Injection.Usage
{
    public class MathsService
    {
        private readonly IAddService _addService;

        public MathsService(IAddService addService)
        {
            _addService = addService;
        }

        public int Add(int op1, int op2)
        {
            return _addService.Add(op1, op2);
        }
    }
}
