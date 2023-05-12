using EntregaSegura.Application.Interfaces;
using EntregaSegura.Application.Notifications;
using EntregaSegura.Domain.Entities;
using EntregaSegura.Domain.Interfaces.Repositories;
using EntregaSegura.Domain.Validators;
using EntregaSegura.Infrastructure.UnitOfWork;

namespace EntregaSegura.Application.Services;

public class CondominioService : BaseService, ICondominioService
{
    private readonly ICondominioRepository _condominioRepository;

    public CondominioService(ICondominioRepository condominioRepository,
                             IUnitOfWork unitOfWork,
                             INotificadorErros notificadorErros) : base(unitOfWork, notificadorErros)
    {
        _condominioRepository = condominioRepository;
    }

    public async Task Adicionar(Condominio condominio)
    {
        if(!ExecutarValidacao(new CondominioValidator(), condominio)) return;

        if(_condominioRepository.BuscarAsync(c => c.CNPJ == condominio.CNPJ).Result.Any())
        {
            Notificar("Já existe um condomínio com este CNPJ.");
            return;
        }

        if(_condominioRepository.BuscarAsync(c => c.Nome == condominio.Nome).Result.Any())
        {
            Notificar("Já existe um condomínio com este nome.");
            return;
        }

        _condominioRepository.Adicionar(condominio);
        await CommitAsync();
    }

    public async Task Atualizar(Condominio condominio)
    {
        if(!ExecutarValidacao(new CondominioValidator(), condominio)) return;

        if(_condominioRepository.BuscarAsync(c => c.CNPJ == condominio.CNPJ && c.Id != condominio.Id).Result.Any())
        {
            Notificar("Já existe um condomínio com este CNPJ.");
            return;
        }

        _condominioRepository.Atualizar(condominio);
        await CommitAsync();
    }

    public async Task Remover(Guid id)
    {
        if(_condominioRepository.ObterCondominioComUnidadesAsync(id).Result.Unidades.Any())
        {
            Notificar("O condomínio possui unidades cadastradas!");
            return;
        }
        _condominioRepository.Remover(id);
        await CommitAsync();
    }

    public async Task<IEnumerable<Condominio>> ObterTodosAsync()
    {
        return await _condominioRepository.ObterTodosAsync();
    }

    public async Task<Condominio> ObterPorIdAsync(Guid id)
    {
        return await _condominioRepository.ObterPorIdAsync(id);
    }

    public async Task<Condominio> ObterPorNomeAsync(string nome)
    {
        return await _condominioRepository.ObterPorNomeAsync(nome);
    }

    public async Task<Condominio> ObterCondominioComFuncionariosAsync(Guid condominioId)
    {
        return await _condominioRepository.ObterCondominioComFuncionariosAsync(condominioId);
    }

    public async Task<Condominio> ObterCondominioComUnidadesAsync(Guid condominioId)
    {
        return await _condominioRepository.ObterCondominioComUnidadesAsync(condominioId);
    }

    public void Dispose()
    {
        _unitOfWork?.Dispose();
    }
}
