namespace AppointmentScheduler.Domain.Repositories;

public interface IRepositoryEntityInitializer
{
    Task<bool> Initialize(IRepository repository);
}