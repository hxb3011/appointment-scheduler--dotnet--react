namespace AppointmentScheduler.Domain.Entities;

public interface IRepositoryEntityInitializer
{
    bool Initilize(IRepository repository);
}