#define DEMO

using AppointmentScheduler.Domain;
using AppointmentScheduler.Domain.Entities;
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
#if DEMO
        if (typeof(IUser).IsAssignableFrom(typeof(TEntity)))
        {
            if (key is string sk && sk == "testkey")
            {
                entity = (TEntity)(IUser)new DemoUserImpl();
                return true;
            }
        }
        entity = default;
        return false;
#else
        throw new NotImplementedException();
#endif
    }

    bool IRepository.TryGetKeyOf<TEntity, TKey>(TEntity entity, out TKey key)
    {
#if DEMO
        if (entity is DemoUserImpl demoUser)
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

#if DEMO

public class DemoUserImpl : IUser
{
    private class RoleImpl : IRole
    {
        public string Name { get => "Role"; set => throw new NotImplementedException(); }
        public string Description { get => "Description"; set => throw new NotImplementedException(); }

        public IEnumerable<Permission> Permissions => [Permission.Perm2, Permission.Perm3];

        public bool IsNameExisted => false;

        public bool IsNameValid => true;

        public bool IsDescriptionValid => true;

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool IsPermissionGranted(Permission permission)
        {
            return permission == Permission.Perm2 || permission == Permission.Perm3;
        }

        public bool SetPermissionGranted(Permission permission, bool granted = true)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }

    public string UserName { get => "test@company.com"; set => throw new NotImplementedException(); }
    public string Password { get => "HeLlo|12"; set => throw new NotImplementedException(); }
    public string FullName { get => "Test"; set => throw new NotImplementedException(); }
    public IRole Role { get => new RoleImpl(); set => throw new NotImplementedException(); }

    public bool IsUserNameExisted => false;

    public bool IsUserNameValid => true;

    public bool IsPasswordValid => true;

    public bool IsFullNameValid => true;

    public bool Delete()
    {
        throw new NotImplementedException();
    }

    public bool Update()
    {
        throw new NotImplementedException();
    }
}

#endif