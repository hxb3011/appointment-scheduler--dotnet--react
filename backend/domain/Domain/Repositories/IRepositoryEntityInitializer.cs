namespace AppointmentScheduler.Domain.Repositories;

public interface IRepositoryEntityInitializer
{
    Task<bool> Initilize(IRepository repository);
}