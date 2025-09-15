// Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Application.Mappings;

public static class LogradouroMappings
{
    public static LogradouroDTO ToDto(this Logradouro logradouro)
    {
        return new LogradouroDTO
        {
            Id = logradouro.Id,
            Cep = logradouro.Cep,
            Nome = logradouro.Nome,
            Bairro = logradouro.Bairro,
            Cidade = logradouro.Cidade,
            Estado = logradouro.Estado,
            Pais = logradouro.Pais
        };
    }
    public static Logradouro ToEntity(this LogradouroDTO logradouroDto)
    {
        return Logradouro.Criar(
        logradouroDto.Id,
        logradouroDto.Cep,
        logradouroDto.Nome,
        logradouroDto.Bairro,
        logradouroDto.Cidade,
        logradouroDto.Estado,
        logradouroDto.Pais);
    }
    public static Logradouro UpdateFromDto(this Logradouro logradouro, LogradouroDTO logradouroDto)
    {
        

        return Logradouro.Criar(
        logradouro.Id, 
        logradouro.Cep, 
        logradouroDto.Nome ?? logradouro.Nome,
        logradouroDto.Bairro ?? logradouro.Bairro,
        logradouroDto.Cidade ?? logradouro.Cidade,
        logradouroDto.Estado ?? logradouro.Estado,
        logradouroDto.Pais ?? logradouro.Pais);
    }
}