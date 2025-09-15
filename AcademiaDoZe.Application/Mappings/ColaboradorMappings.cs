// Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObjects;
namespace AcademiaDoZe.Application.Mappings;

public static class ColaboradorMappings
{
    public static ColaboradorDTO ToDto(this Colaborador colaborador)
    {
        return new ColaboradorDTO
        {
            Id = colaborador.Id,
            Nome = colaborador.Nome,
            Cpf = colaborador.Cpf,
            DataNascimento = colaborador.DataNascimento,
            Telefone = colaborador.Telefone,
            Email = colaborador.Email,
            Endereco = colaborador.Endereco.ToDto(),
            Numero = colaborador.Numero,
            Complemento = colaborador.Complemento,
            Senha = null,
            Foto = colaborador.Foto != null ? new ArquivoDTO { Conteudo = colaborador.Foto.Conteudo } : null,
            DataAdmissao = colaborador.DataAdmissao,
            Tipo = colaborador.Tipo.ToApp(),
            Vinculo = colaborador.Vinculo.ToApp()
        };
    }
    public static Colaborador ToEntity(this ColaboradorDTO colaboradorDto)
    {
        return Colaborador.Criar(
        colaboradorDto.Id,
        colaboradorDto.Nome,
        colaboradorDto.Cpf,
        colaboradorDto.DataNascimento,
        colaboradorDto.Telefone,
        colaboradorDto.Email!,
        colaboradorDto.Endereco.ToEntity(), 
        colaboradorDto.Numero,
        colaboradorDto.Complemento!,
        colaboradorDto.Senha!,
        (colaboradorDto.Foto?.Conteudo != null) ? Arquivo.Criar(colaboradorDto.Foto.Conteudo) : null!, 
        colaboradorDto.DataAdmissao,
        colaboradorDto.Tipo.ToDomain(),
        colaboradorDto.Vinculo.ToDomain()
        );
    }
    public static Colaborador UpdateFromDto(this Colaborador colaborador, ColaboradorDTO colaboradorDto)
    {
        return Colaborador.Criar(
        colaborador.Id,
        colaboradorDto.Nome ?? colaborador.Nome,
        colaborador.Cpf, 
        colaboradorDto.DataNascimento != default ? colaboradorDto.DataNascimento : colaborador.DataNascimento,
        colaboradorDto.Telefone ?? colaborador.Telefone,
        colaboradorDto.Email ?? colaborador.Email,
        colaboradorDto.Endereco.ToEntity() ?? colaborador.Endereco,
        colaboradorDto.Numero ?? colaborador.Numero,
        colaboradorDto.Complemento ?? colaborador.Complemento,
        colaboradorDto.Senha ?? colaborador.Senha,
        (colaboradorDto.Foto?.Conteudo != null) ? Arquivo.Criar(colaboradorDto.Foto.Conteudo) : colaborador.Foto, 
        colaboradorDto.DataAdmissao != default ? colaboradorDto.DataAdmissao : colaborador.DataAdmissao,
        colaboradorDto.Tipo != default ? colaboradorDto.Tipo.ToDomain() : colaborador.Tipo,
        colaboradorDto.Vinculo != default ? colaboradorDto.Vinculo.ToDomain() : colaborador.Vinculo
        );
    }
}