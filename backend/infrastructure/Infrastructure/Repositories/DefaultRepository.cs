#define DEMO

using AppointmentScheduler.Domain;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Infrastructure.Business;
using Infrastructure.Business;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace AppointmentScheduler.Infrastructure.Repositories;

public class DefaultRepository : DbContext, IRepository
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? database, server, user, password;
        if (string.IsNullOrWhiteSpace(database = "DB_DATABASE".Env())) database = "apomtschedsys";
        if (string.IsNullOrWhiteSpace(password = "DB_PASSWORD".Env())) password = "HeLlo|12";
        if (!uint.TryParse("DB_PORT".Env(), out uint port)) port = 3306;
        if (string.IsNullOrWhiteSpace(server = "DB_SERVER".Env())) server = "localhost";
        if (string.IsNullOrWhiteSpace(user = "DB_USERNAME".Env())) user = "apomtschedsys";
        optionsBuilder.UseMySQL(new MySqlConnectionStringBuilder
        {
            Database = database,
            Password = password,
            Port = port,
            Server = server,
            UserID = user,
        }.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Role>(MySQLEntitiesExtensions.BuildRoleEntity)
            .Entity<User>(MySQLEntitiesExtensions.BuildUserEntity)
            .Entity<Patient>(MySQLEntitiesExtensions.BuildPatientEntity)
            .Entity<Doctor>(MySQLEntitiesExtensions.BuildDoctorEntity)
            .Entity<Profile>(MySQLEntitiesExtensions.BuildProfileEntity)
            .Entity<Appointment>(MySQLEntitiesExtensions.BuildAppointmentEntity)
            .Entity<Examination>(MySQLEntitiesExtensions.BuildExaminationEntity)
            .Entity<DiagnosticService>(MySQLEntitiesExtensions.BuildDiagnosticServiceEntity)
            .Entity<ExaminationService>(MySQLEntitiesExtensions.BuildExaminationServiceEntity)
            .Entity<Prescription>(MySQLEntitiesExtensions.BuildPrescriptionEntity)
            .Entity<Medicine>(MySQLEntitiesExtensions.BuildMedicineEntity)
            .Entity<PrescriptionDetail>(MySQLEntitiesExtensions.BuildPrescriptionDetailEntity);
    }

    IEnumerable<TEntity> IRepository.GetEntities<TEntity>()
    {
        throw new NotImplementedException();
    }

    TService IRepository.GetService<TService>()
    {
        if (this is TService myService) return myService;
        if (typeof(IResourceManagerService).IsAssignableFrom(typeof(TService)))
        {
            return (TService)(IResourceManagerService)
                new ResourceManagerServiceImpl();
        }
        if (typeof(IConfigurationPropertiesService).IsAssignableFrom(typeof(TService)))
        {
            return (TService)(IConfigurationPropertiesService)
                new ConfigurationPropertiesServiceImpl(this);
        }
        throw new InvalidOperationException("This repository does not included service " + typeof(TService).FullName);
    }

    TEntity IRepository.ObtainEntity<TEntity>()
    {
        throw new NotImplementedException();
    }

    bool IRepository.TryGetEntityBy<TKey, TEntity>(TKey key, out TEntity entity)
    {
        if (typeof(IUser).IsAssignableFrom(typeof(TEntity)))
        {
            if (key is string sk && sk == "testkey")
            {
                entity = (TEntity)(IUser)new UserImpl();
                return true;
            }
        }
        else if (typeof(Appointment).IsAssignableFrom(typeof(TEntity)))
        {
            if (key is int sk)
            {
                entity = (TEntity)(IAppointment)new AppointmentImpl();
                return true;
            }
        }
        entity = default;
        return false;
    }

    bool IRepository.TryGetKeyOf<TEntity, TKey>(TEntity entity, out TKey key)
    {
#if DEMO
        if (entity is UserImpl demoUser)
        {
            string sk = "testkey";
            if (sk is TKey kk)
            {
                key = kk;
                return true;
            }
        }
        key = default;
        return false;
#else
        throw new NotImplementedException();
#endif
    }
}

