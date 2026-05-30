namespace Richter.WhoAmIApi.CrossCutting.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException()
        { }

        public BusinessRuleException(string mensagem) : base(mensagem)
        { }

        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}