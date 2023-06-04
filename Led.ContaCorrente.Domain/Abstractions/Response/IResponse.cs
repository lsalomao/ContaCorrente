using Led.ContaCorrente.Domain.Enums;

namespace Led.ContaCorrente.Domain.Abstractions.Response
{
    public interface IResponse
    {
        IResponse AddErro(string mensagem);
        void DefinirMotivoErro(MotivoErro motivoFalha);
    }
}
