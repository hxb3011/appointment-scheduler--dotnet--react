namespace AppointmentScheduler.Domain.IEntities;

public interface IRepositoryEntityInitializer
{
    bool Initilize(IRepository repository);
}