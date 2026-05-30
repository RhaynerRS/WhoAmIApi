namespace Richter.WhoAmIApi.CrossCutting.Exceptions
{
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException()
        { }

        public RegraDeNegocioException(string mensagem) : base(mensagem)
        { }

        public RegraDeNegocioException(string mensagem, Exception innerException) : base(mensagem, innerException)
        { }
    }
}