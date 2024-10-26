namespace AppointmentScheduler.Domain.Repositories;

public interface IConfigurationPropertiesService
{
    public string GetProperty(string propertyName, string defaultValue = "");
    public bool SetProperty(string propertyName, string value = "");
}