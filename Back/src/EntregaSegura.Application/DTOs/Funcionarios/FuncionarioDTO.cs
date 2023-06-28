using System.ComponentModel.DataAnnotations;

namespace EntregaSegura.Application.DTOs;

public class FuncionarioDTO
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 2)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(11, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 11)]
    public string CPF { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(11, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 11)]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 2)]
    public string Email { get; set; }

    public string Cargo { get; set; }

    public DateTime DataAdmissao { get; set; }

    public DateTime? DataDemissao { get; set; }

    public string FotoUpload { get; set; }

    public string Foto { get; set; }

    public int CondominioId { get; set; }

    public string NomeCondominio { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]

    public int UserId { get; set; }
}