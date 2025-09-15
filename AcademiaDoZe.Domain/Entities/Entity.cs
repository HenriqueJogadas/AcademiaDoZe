// Henrique Churkin Correia Alberton

using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }
    protected Entity(int id = 0)
    {
        if (id < 0) throw new DomainException("ID_NEGATIVO");
        Id = id;
    }
    public override bool Equals([System.Diagnostics.CodeAnalysis.AllowNull] object obj)
    {
        if (obj is not Entity other)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id && GetType() == other.GetType();
    }
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}