namespace AppointmentScheduler.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public override int GetHashCode() => Id;
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj == null) return false;
        if (GetType() != obj.GetType()) return false;
        return Id == ((BaseEntity)obj).Id;
    }
}