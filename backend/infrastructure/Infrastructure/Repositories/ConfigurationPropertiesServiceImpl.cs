
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class ConfigurationPropertiesServiceImpl : IConfigurationPropertiesService
{
    private DbContext _context;

    internal ConfigurationPropertiesServiceImpl(DbContext context) => _context = context;

    string IConfigurationPropertiesService.GetProperty(string propertyName, string defaultValue)
    {
        var database = _context.Database;
        database.ExecuteSqlRaw(@"CREATE TABLE `property`(`Key` varchar(100) NOT NULL, `Value` varchar(100) NOT NULL, PRIMARY KEY (`Key`)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci");
        return database.SqlQuery<string>($"SELECT `Value` FROM `property` WHERE `Key` == {propertyName}").FirstOrDefault(defaultValue);
    }

    bool IConfigurationPropertiesService.SetProperty(string propertyName, string value)
    {
        FormattableString insertSql;
        var database = _context.Database;
        return database.ExecuteSql($"UPDATE `property` SET `Value` = {value}` Where `Key` == {propertyName}") != 0
            || database.ExecuteSql(insertSql = $"INSERT INTO `property` (`Value`, `Key`) VALUES ({value}, {propertyName})") != 0
            || (database.ExecuteSql($"CREATE TABLE `property`(`Key` varchar(100) NOT NULL, `Value` varchar(100) NOT NULL, PRIMARY KEY (`Key`)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci") != 0
                && database.ExecuteSql(insertSql) != 0);
    }
}