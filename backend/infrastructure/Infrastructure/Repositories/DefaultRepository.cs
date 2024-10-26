#define DEMO

using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Business;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Repositories;

public class DefaultRepository : DbContext, IRepository
{
    private IResourceManagerService _resourceManager;
    private IConfigurationPropertiesService _configurationProperties;
    internal DefaultRepository(DbContextOptions options) : base(options) { }

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
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
            return (
                from role in Set<Role>()
                select new RoleImpl(role)
            ).Cast<TEntity>();
        throw new NotImplementedException();
    }

    async Task<TService> IRepository.GetService<TService>()
    {
        if (this is TService myService) return myService;
        if (typeof(TService).IsAssignableFrom(typeof(IResourceManagerService)))
        {
            IResourceManagerService resourceManager = _resourceManager;
            resourceManager ??= new ResourceManagerServiceImpl();
            _resourceManager = resourceManager;
            return (TService)resourceManager;
        }
        else if (typeof(TService).IsAssignableFrom(typeof(IConfigurationPropertiesService)))
        {
            IConfigurationPropertiesService configurationProperties = _configurationProperties;
            configurationProperties ??= new ConfigurationPropertiesServiceImpl(this);
            _configurationProperties = configurationProperties;
            return (TService)configurationProperties;
        }
        throw new InvalidOperationException("This repository does not included service " + typeof(TService).FullName);
    }

    async Task<TEntity> IRepository.GetEntityBy<TKey, TEntity>(TKey key)
    {
        TEntity entity;
#if DEMO
        if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
        {
            if (key is string sk && sk == "testkey")
            {
                entity = (TEntity)(IUser)new DemoUserImpl();
                return entity is not IRepositoryEntityInitializer initializer
                    || await initializer.Initilize(this) ? entity : null;
            }
        }
        return null;
#else
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var role = await FindAsync<Role>(id);
                if (role != null)
                {
                    entity = (TEntity)(IRole)new RoleImpl(role);
                    goto success;
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
        {
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
        {
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
        {
            var d = await ((IRepository)this).GetEntityBy<TKey, IDoctor>(key);
            if (d != null) return (TEntity)d;
            var p = await ((IRepository)this).GetEntityBy<TKey, IPatient>(key);
            if (p != null) return (TEntity)p;
        }
        return null;
    success:
        return entity is not IRepositoryEntityInitializer initializer
            || await initializer.Initilize(this) ? entity : null;
#endif
    }

    async Task<TEntity> IRepository.ObtainEntity<TEntity>()
    {
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
        {
            var role = new Role();
            return await this.IdGeneratedWrap(
                from r in Set<Role>()
                where r.Id == role.Id
                select r, role, nameof(Role.Id)
            ) ? (TEntity)(IRole)new RoleImpl(role) : null;
        }
        throw new NotImplementedException();
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

        public async Task<bool> IsNameExisted() => false;

        public bool IsNameValid => true;

        public bool IsDescriptionValid => true;

        public async Task<bool> Create()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        public bool IsPermissionGranted(Permission permission)
        {
            return permission == Permission.Perm2 || permission == Permission.Perm3;
        }

        public void SetPermissionGranted(Permission permission, bool granted = true)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update()
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

    public async Task<bool> Create()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update()
    {
        throw new NotImplementedException();
    }
}

#endif