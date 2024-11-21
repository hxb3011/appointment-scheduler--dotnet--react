
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class ConfigurationPropertiesServiceImpl : IConfigurationPropertiesService
{
    private const string
        CreateTable = """
            CREATE TABLE IF NOT EXISTS `property`(
                `Key` varchar(100) NOT NULL,
                `Value` varchar(100) NOT NULL,
                PRIMARY KEY (`Key`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
            """,
        InsertValue = "INSERT INTO `property` (`Value`, `Key`) VALUES ({0}, {1})",
        UpdateValue = "UPDATE `property` SET `Value` = {0} Where `Key` = {1}",
        GetValue = "SELECT `Value` FROM `property` WHERE `Key` = {0}";

    private readonly DbContext _context;

    internal ConfigurationPropertiesServiceImpl(DbContext context) => _context = context;

    string IConfigurationPropertiesService.GetProperty(string propertyName, string defaultValue)
    {
        var database = _context.Database;
        database.ExecuteSqlRaw(CreateTable);
        return database.SqlQueryRaw<string>(GetValue, propertyName)
            .AsEnumerable().FirstOrDefault(defaultValue);
    }

    bool IConfigurationPropertiesService.SetProperty(string propertyName, string value)
    {
        var database = _context.Database;
        return database.ExecuteSqlRaw(UpdateValue, value, propertyName) != 0
            || database.ExecuteSqlRaw(InsertValue, value, propertyName) != 0
            || (database.ExecuteSqlRaw(CreateTable) != 0
                && database.ExecuteSqlRaw(InsertValue, value, propertyName) != 0);
    }
}