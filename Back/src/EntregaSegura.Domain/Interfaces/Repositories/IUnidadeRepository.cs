using EntregaSegura.Domain.Entities;

namespace EntregaSegura.Domain.Interfaces.Repositories
{
    public interface IUnidadeRepository : IRepository<Unidade>
    {
        Task<IEnumerable<Unidade>> ObterUnidadesComCondominioAsync();
        Task<Unidade> ObterUnidadePorIdComCondominioEMoradoresAsync(Guid id);
        Task<Unidade> ObterPorUnidadePorCondominioBlocoNumeroAsync(Guid condominioId, string bloco, string numero);
        Task<Unidade> ObterUnidadeComMoradoresPorCondominioBlocoNumeroAsync(Guid condominioId, string bloco, string numero);
        Task<Unidade> ObterUnidadeComEntregasPorCondominioBlocoNumeroAsync(Guid condominioId, string bloco, string numero);
    }
}