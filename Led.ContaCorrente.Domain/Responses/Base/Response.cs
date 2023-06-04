using Led.ContaCorrente.Domain.Abstractions.Response;
using Led.ContaCorrente.Domain.Enums;

namespace Led.ContaCorrente.Domain.Responses.Base
{
    public class Response<TDados> : IResponse
    {
        private readonly IList<string> _mensagens = new List<string>();

        private readonly bool possuiErro;

        public Response()
        {         
        }

        public Response(MotivoErro motivoErro, string mensagemErro)
        {
            MotivoErro = motivoErro;

            _mensagens.Add(string.IsNullOrWhiteSpace(mensagemErro)
                ? motivoErro.ToString()
                : mensagemErro);

            DetalheErro = _mensagens.Any() ? string.Join(" | ", _mensagens.ToList()) : string.Empty;
            Dados = default;
        }

        public Response(MotivoErro motivoErro, IEnumerable<string> mensagensErro)
        {
            MotivoErro = motivoErro;
            DetalheErro = mensagensErro.Any() ? string.Join(" | ", mensagensErro.ToList()) : string.Empty;
            Dados = default;
        }

        public Response(MotivoErro motivoErro)
        {
            Dados = default;
            MotivoErro = motivoErro;
            possuiErro = true;
        }

        public Response(TDados dados)
        {
            Dados = dados;
            DetalheErro = string.Empty;
            MotivoErro = null;
        }

        public string DetalheErro { get; set; } = string.Empty;

        public TDados? Dados { get; set; } = default;

        public bool PossuiErro => (!string.IsNullOrWhiteSpace(DetalheErro) || possuiErro);

        public MotivoErro? MotivoErro { get; private set; } = null;

        public IResponse AddErro(string mensagem)
        {
            _mensagens.Add(mensagem);
            DetalheErro = _mensagens.Any() ? string.Join(" | ", _mensagens.ToList()) : string.Empty;
            return this;
        }

        public void DefinirMotivoErro(MotivoErro motivoErro)
        {
            MotivoErro = motivoErro;
        }
    }
}
