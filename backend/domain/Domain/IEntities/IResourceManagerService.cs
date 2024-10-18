namespace AppointmentScheduler.Domain.IEntities;

public interface IResourceManagerService
{
    Stream LoadResource<TEntity>(string resourceId) where TEntity : IBehavioralEntity;
    bool StoreResource<TEntity>(string resourceId, Stream resourceStream) where TEntity : IBehavioralEntity;
}

public interface IConfigurationPropertiesService
{
    public string GetProperty(string propertyName, string defaultValue = "");
    public bool SetProperty(string propertyName, string value = "");
}