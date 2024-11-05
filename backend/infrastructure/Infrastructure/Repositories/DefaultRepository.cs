// #define DEMO

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
    public DefaultRepository(DbContextOptions options) : base(options) { }

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
                select GetEntityBy<uint, IRole>(role.Id, role)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
            return (
                from doctor in Set<Doctor>()
                select GetEntityBy<uint, IDoctor>(doctor.Id, doctor)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
            return (
                from patient in Set<Patient>()
                select GetEntityBy<uint, IPatient>(patient.Id, patient)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
            return (
                from user in Set<User>()
                select GetEntityBy<uint, IUser>(user.Id, user)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IProfile)))
            return (
                from profile in Set<Profile>()
                select GetEntityBy<uint, IProfile>(profile.Id, profile)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IAppointment)))
            return (
                from appointment in Set<Appointment>()
                select GetEntityBy<uint, IAppointment>(appointment.Id, appointment)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IExamination)))
            return (
                from examination in Set<Examination>()
                select GetEntityBy<uint, IExamination>(examination.Id, examination)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDiagnosticService)))
            return (
                from diagsv in Set<DiagnosticService>()
                select GetEntityBy<uint, IDiagnosticService>(diagsv.Id, diagsv)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPrescription)))
            return (
                from prescription in Set<Prescription>()
                select GetEntityBy<uint, IPrescription>(prescription.Id, prescription)
                    .WaitForResult(Timeout.Infinite, default)
            ).Cast<TEntity>();
        return Enumerable.Empty<TEntity>();
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

    Task<TEntity> IRepository.GetEntityBy<TKey, TEntity>(TKey key) => GetEntityBy<TKey, TEntity>(key, null);

    private async Task<TEntity> GetEntityBy<TKey, TEntity>(TKey key, object row = null) where TEntity : class, IBehavioralEntity
    {
#if DEMO
        if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
        {
            if (key is string sk && sk == "testkey") return (TEntity)await
                this.Initialize((IUser)new DemoUserImpl());
        }
        return null;
#else
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var role = row as Role ?? await FindAsync<Role>(id);
                if (role != null) return (TEntity)await
                    this.Initialize((IRole)new RoleImpl(role));
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var user = row as User ?? await FindAsync<User>(id);
                if (user != null)
                {
                    var doctor = row as Doctor ?? await FindAsync<Doctor>(id);
                    if (doctor != null)
                    {
                        var irole = await GetEntityBy<uint, IRole>(user.RoleId, null);
                        irole ??= await RoleImpl.GetDefault(this);
                        IDoctor idoctor = new DoctorImpl(user, doctor, irole);
                        return (TEntity)await this.Initialize(idoctor);
                    }
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var user = row as User ?? await FindAsync<User>(id);
                if (user != null)
                {
                    var patient = row as Patient ?? await FindAsync<Patient>(id);
                    if (patient != null)
                    {
                        var irole = await GetEntityBy<uint, IRole>(user.RoleId, null);
                        irole ??= await RoleImpl.GetDefault(this);
                        IPatient ipatient = new PatientImpl(user, patient, irole);
                        return (TEntity)await this.Initialize(ipatient);
                    }
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
        {
            var d = await GetEntityBy<TKey, IDoctor>(key, row);
            if (d != null) return (TEntity)d;
            var p = await GetEntityBy<TKey, IPatient>(key, row);
            if (p != null) return (TEntity)p;
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IProfile)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var profile = (Profile)row ?? await FindAsync<Profile>(id);
                if (profile != null)
                {
                    var patient = await GetEntityBy<uint, IPatient>(profile.PatientId, null);
                    IProfile iprofile = new ProfileImpl(profile, patient);
                    return (TEntity)await this.Initialize(iprofile);
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IAppointment)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var appointment = (Appointment)row ?? await FindAsync<Appointment>(id);
                if (appointment != null)
                {
                    var doctor = await GetEntityBy<uint, IDoctor>(appointment.DoctorId, null);
                    IAppointment iappointment = new AppointmentImpl(appointment, doctor);
                    return (TEntity)await this.Initialize(iappointment);
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IExamination)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var examination = (Examination)row ?? await FindAsync<Examination>(id);
                if (examination != null)
                {
                    var appointment = await GetEntityBy<uint, IAppointment>(examination.AppointmentId, null);
                    IExamination iexamination = new ExaminationImpl(examination, appointment);
                    return (TEntity)await this.Initialize(iexamination);
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDiagnosticService)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                IDiagnosticService iappointment;
                var diagsv = row as DiagnosticService ?? await FindAsync<DiagnosticService>(id);
                if (diagsv != null)
                {
                    iappointment = new DiagnosticServiceImpl(diagsv);
                    return (TEntity)await this.Initialize(iappointment);
                }
                var exdiag = row as ExaminationService ?? await FindAsync<ExaminationService>(id);
                if (exdiag != null)
                {
                    diagsv = await FindAsync<DiagnosticService>(exdiag.DiagnosticServiceId);
                    if (diagsv != null)
                    {
                        var doctor = await GetEntityBy<uint, IDoctor>(exdiag.DoctorId, null);
                        var examination = await GetEntityBy<uint, IExamination>(exdiag.ExaminationId, null);
                        iappointment = new DiagnosticServiceImpl(diagsv, exdiag, doctor, examination);
                        return (TEntity)await this.Initialize(iappointment);
                    }
                }
            }
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPrescription)))
        {
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                var prescription = (Prescription)row ?? await FindAsync<Prescription>(id);
                if (prescription != null)
                {
                    var examination = await GetEntityBy<uint, IExamination>(prescription.ExaminationId);
                    IPrescription iprescription = new PrescriptionImpl(prescription, examination);
                    return (TEntity)await this.Initialize(iprescription);
                }
            }
        }
        // TODO: ...more Impl
        return null;
#endif
    }

    async Task<TEntity> IRepository.ObtainEntity<TEntity>()
    {
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
        {
            var role = new Role();
            if (await this.IdGeneratedWrap(
                from r in Set<Role>()
                where r.Id == role.Id
                select r, role, nameof(Role.Id)
            )) return null;
            return (TEntity)await this.Initialize((IRole)new RoleImpl(role));
        }
        if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
        {
            var user = new User();
            if (!await this.IdGeneratedWrap(
                from r in Set<User>()
                where r.Id == user.Id
                select r, user, nameof(User.Id)
            )) return null;
            var doctor = new Doctor { Id = user.Id };
            var role = await RoleImpl.GetDefault(this);
            return (TEntity)await this.Initialize((IUser)new DoctorImpl(user, doctor, role));
        }
        if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
        {
            var user = new User();
            if (!await this.IdGeneratedWrap(
                from r in Set<User>()
                where r.Id == user.Id
                select r, user, nameof(User.Id)
            )) return null;
            var patient = new Patient { Id = user.Id };
            var role = await RoleImpl.GetDefault(this);
            return (TEntity)await this.Initialize((IUser)new PatientImpl(user, patient, role));
        }
        if (typeof(TEntity).IsAssignableFrom(typeof(IDiagnosticService)))
        {
            var diagnosticService = new DiagnosticService();
            if (!await this.IdGeneratedWrap(
                from ds in Set<DiagnosticService>()
                where ds.Id == diagnosticService.Id
                select ds, diagnosticService, nameof(DiagnosticService.Id)
            )) return null;
            return (TEntity)await this.Initialize((IDiagnosticService)new DiagnosticServiceImpl(diagnosticService));
        }
        return null;
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
        if (entity is RoleImpl role)
        {
            uint id = role._role.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is UserImpl user)
        {
            uint id = user._user.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is ProfileImpl profile)
        {
            uint id = profile._profile.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is AppointmentImpl appointment)
        {
            uint id = appointment._appointment.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is ExaminationImpl examination)
        {
            uint id = examination._examination.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is PrescriptionImpl prescription)
        {
            uint id = prescription._prescription.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is DiagnosticServiceImpl diagnosticService)
        {
            var e = diagnosticService._exdiag;
            uint id = e == null ? e.Id : diagnosticService._diagsv.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            else if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        // TODO: ...more Impl
        key = default;
        return false;
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

        event EventHandler IBehavioralEntity.Created
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IBehavioralEntity.Updated
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IBehavioralEntity.Deleted
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

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

        Task<bool> IRole.IsNameExisted()
        {
            throw new NotImplementedException();
        }

        bool IRole.IsPermissionGranted(Permission permission)
        {
            throw new NotImplementedException();
        }

        void IRole.SetPermissionGranted(Permission permission, bool granted)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBehavioralEntity.Create()
        {
            throw new NotImplementedException();
        }

        Task<bool> IBehavioralEntity.Update()
        {
            throw new NotImplementedException();
        }

        Task<bool> IBehavioralEntity.Delete()
        {
            throw new NotImplementedException();
        }
    }

    public string UserName { get => "test@company.com"; set => throw new NotImplementedException(); }
    public string Password { get => "HeLlo|12"; set => throw new NotImplementedException(); }
    public string FullName { get => "Test"; set => throw new NotImplementedException(); }
    public IRole Role { get => new RoleImpl(); set => throw new NotImplementedException(); }

    public bool IsUserNameValid => true;

    public bool IsPasswordValid => true;

    public bool IsFullNameValid => true;

    event EventHandler IBehavioralEntity.Created
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    event EventHandler IBehavioralEntity.Updated
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    event EventHandler IBehavioralEntity.Deleted
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

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

    Task<bool> IUser.IsUserNameExisted()
    {
        throw new NotImplementedException();
    }

    Task<bool> IBehavioralEntity.Create()
    {
        throw new NotImplementedException();
    }

    Task<bool> IBehavioralEntity.Update()
    {
        throw new NotImplementedException();
    }

    Task<bool> IBehavioralEntity.Delete()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangeRole(IRole role)
    {
        throw new NotImplementedException();
    }
}

#endif