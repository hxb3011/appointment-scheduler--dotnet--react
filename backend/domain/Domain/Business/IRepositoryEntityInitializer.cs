namespace AppointmentScheduler.Domain.Business;

public interface IRepositoryEntityInitializer
{
    bool Initilize(IRepository repository);
}