using System.Linq.Expressions;
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
    private ISchedulerService _scheduler;
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

    private IEnumerable<TEntity> GetEntitiesBy<TEntity, TKey>(
        int skip, int take, string orderByProperty, bool descending,
        string whereProperty, object andValue, bool areEqual
    ) where TEntity : class, IBehavioralEntity where TKey : class
    {
        var para = Expression.Parameter(typeof(TKey));
        Func<Expression, Expression, BinaryExpression> op = areEqual ? Expression.Equal : Expression.NotEqual;
        IQueryable<TKey> query = Set<TKey>();
        if (!string.IsNullOrWhiteSpace(whereProperty)) query = query.Where(Expression.Lambda<Func<TKey, bool>>(
            op(Expression.Property(para, whereProperty), Expression.Constant(andValue)), para));
        return query.OrderByPropertyName(orderByProperty, descending)
            .Skip(skip).Take(take).ToList().Select(GetEntityBySync<TEntity, TKey>);
    }

    private TEntity GetEntityBySync<TEntity, TKey>(TKey x) where TEntity : class, IBehavioralEntity
        where TKey : class => ((IRepository)this).GetEntityBy<TKey, TEntity>(x).WaitForResult();

    IEnumerable<TEntity> IRepository.GetEntities<TEntity>(
        int offset, int count, string orderByProperty, bool descending,
        string whereProperty, object andValue, bool areEqual)
    {
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
            return GetEntitiesBy<TEntity, Role>(offset, count, orderByProperty ?? nameof(Role.Name), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
            return GetEntitiesBy<TEntity, User>(offset, count, orderByProperty ?? nameof(User.FullName), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
            return GetEntitiesBy<TEntity, Doctor>(offset, count, orderByProperty ?? nameof(Doctor.Position), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
            return GetEntitiesBy<TEntity, Patient>(offset, count, orderByProperty ?? nameof(Patient.Phone), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IProfile)))
            return GetEntitiesBy<TEntity, Profile>(offset, count, orderByProperty ?? nameof(Profile.FullName), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IAppointment)))
            return GetEntitiesBy<TEntity, Appointment>(offset, count, orderByProperty ?? nameof(Appointment.Number), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IExamination)))
            return GetEntitiesBy<TEntity, Examination>(offset, count, orderByProperty ?? nameof(Examination.Diagnostic), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDiagnosticService)))
            return GetEntitiesBy<TEntity, DiagnosticService>(offset, count, orderByProperty ?? nameof(DiagnosticService.Name), descending, whereProperty, andValue, areEqual);
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPrescription)))
            return GetEntitiesBy<TEntity, Prescription>(offset, count, orderByProperty ?? nameof(Prescription.Description), descending, whereProperty, andValue, areEqual);
        else return Enumerable.Empty<TEntity>();
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
        else if (typeof(TService).IsAssignableFrom(typeof(ISchedulerService)))
        {
            ISchedulerService scheduler = _scheduler;
            scheduler ??= new SchedulerServiceImpl(this);
            _scheduler = scheduler;
            return (TService)scheduler;
        }
        throw new InvalidOperationException("This repository does not included service " + typeof(TService).FullName);
    }

    async Task<TEntity> IRepository.GetEntityBy<TKey, TEntity>(TKey key)
    {
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
        {
            if (key is Role role || (key is uint id
                    || key is string sk && uint.TryParse(sk, out id))
                && (role = await FindAsync<Role>(id)) != null)
                return (TEntity)await this.Initialize((IRole)new RoleImpl(role));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IUser)))
        {
            IUser user;
            if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor))
                && (user = await ((IRepository)this).GetEntityBy<TKey, IDoctor>(key)) != null)
                return (TEntity)user;
            if (typeof(TEntity).IsAssignableFrom(typeof(IPatient))
                && (user = await ((IRepository)this).GetEntityBy<TKey, IPatient>(key)) != null)
                return (TEntity)user;
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
        {
            User user;
            Doctor doctor;
            if (key is User user_ && (doctor = await FindAsync<Doctor>((user = user_).Id)) != null
                || key is Doctor doctor_ && (user = await FindAsync<User>((doctor = doctor_).Id)) != null
                || (key is uint id || key is string sk && uint.TryParse(sk, out id))
                && (user = await FindAsync<User>(id)) != null && (doctor = await FindAsync<Doctor>(id)) != null)
                return (TEntity)await this.Initialize((IDoctor)new DoctorImpl(user, doctor,
                    await ((IRepository)this).GetEntityBy<uint, IRole>(user.RoleId) ?? await RoleImpl.GetDefault(this)));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
        {
            User user;
            Patient patient;
            if (key is User user_ && (patient = await FindAsync<Patient>((user = user_).Id)) != null
                || key is Patient patient_ && (user = await FindAsync<User>((patient = patient_).Id)) != null
                || (key is uint id || key is string sk && uint.TryParse(sk, out id))
                && (user = await FindAsync<User>(id)) != null && (patient = await FindAsync<Patient>(id)) != null)
                return (TEntity)await this.Initialize((IPatient)new PatientImpl(user, patient,
                    await ((IRepository)this).GetEntityBy<uint, IRole>(user.RoleId) ?? await RoleImpl.GetDefault(this)));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IProfile)))
        {
            if (key is Profile profile || (key is uint id
                    || key is string sk && uint.TryParse(sk, out id))
                && (profile = await FindAsync<Profile>(id)) != null)
                return (TEntity)await this.Initialize((IProfile)new ProfileImpl(profile,
                    await ((IRepository)this).GetEntityBy<uint, IPatient>(profile.PatientId)));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IAppointment)))
        {
            if (key is Appointment appointment || (key is uint id
                    || key is string sk && uint.TryParse(sk, out id))
                && (appointment = await FindAsync<Appointment>(id)) != null)
                return (TEntity)await this.Initialize((IAppointment)new AppointmentImpl(
                    appointment, await ((IRepository)this).GetEntityBy<uint, IDoctor>(appointment.DoctorId)));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IExamination)))
        {
            if (key is Examination examination || (key is uint id
                    || key is string sk && uint.TryParse(sk, out id))
                && (examination = await FindAsync<Examination>(id)) != null)
                return (TEntity)await this.Initialize((IExamination)new ExaminationImpl(
                    examination, await ((IRepository)this).GetEntityBy<uint, IAppointment>(examination.AppointmentId)));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IDiagnosticService)))
        {
            if (key is DiagnosticService diagsv) goto diagsv;
            if (key is ExaminationService exdiag) goto exdiag;
            if (key is uint id || key is string sk && uint.TryParse(sk, out id))
            {
                if ((diagsv = await FindAsync<DiagnosticService>(id)) != null) goto diagsv;
                if ((exdiag = await FindAsync<ExaminationService>(id)) != null) goto exdiag;
            }
            else return null;
            diagsv:
            return (TEntity)await this.Initialize((IDiagnosticService)new DiagnosticServiceImpl(diagsv));
        exdiag:
            if ((diagsv = await FindAsync<DiagnosticService>(exdiag.DiagnosticServiceId)) != null)
                return (TEntity)await this.Initialize((IDiagnosticService)
                    new DiagnosticServiceImpl(diagsv, exdiag,
                        await ((IRepository)this).GetEntityBy<uint, IDoctor>(exdiag.DoctorId)));
        }
        else if (typeof(TEntity).IsAssignableFrom(typeof(IPrescription)))
        {
            if (key is Prescription prescription || (key is uint id
                    || key is string sk && uint.TryParse(sk, out id))
                && (prescription = await FindAsync<Prescription>(id)) != null)
                return (TEntity)await this.Initialize((IPrescription)new PrescriptionImpl(prescription));
        }
        // TODO: ...more Impl
        return null;
    }

    async Task<TEntity> IRepository.ObtainEntity<TEntity>()
    {
        if (typeof(TEntity).IsAssignableFrom(typeof(IRole)))
        {
            var role = new Role();
            if (!await this.IdGenerated(role, nameof(Role.Id))) return null;
            return (TEntity)await this.Initialize((IRole)new RoleImpl(role));
        }
        if (typeof(TEntity).IsAssignableFrom(typeof(IDoctor)))
        {
            var user = new User();
            if (!await this.IdGenerated(user, nameof(User.Id))) return null;
            var doctor = new Doctor { Id = user.Id };
            var role = await RoleImpl.GetDefault(this);
            return (TEntity)await this.Initialize((IUser)new DoctorImpl(user, doctor, role));
        }
        if (typeof(TEntity).IsAssignableFrom(typeof(IPatient)))
        {
            var user = new User();
            if (!await this.IdGenerated(user, nameof(User.Id))) return null;
            var patient = new Patient { Id = user.Id };
            var role = await RoleImpl.GetDefault(this);
            return (TEntity)await this.Initialize((IUser)new PatientImpl(user, patient, role));
        }
        if (typeof(TEntity).IsAssignableFrom(typeof(IDiagnosticService)))
        {
            var diagnosticService = new DiagnosticService();
            if (!await this.IdGenerated(diagnosticService, nameof(DiagnosticService.Id))) return null;
            return (TEntity)await this.Initialize((IDiagnosticService)new DiagnosticServiceImpl(diagnosticService));
        }
        return null;
    }

    bool IRepository.TryGetKeyOf<TEntity, TKey>(TEntity entity, out TKey key)
    {
        if (entity is RoleImpl role)
        {
            var inner = role._role;
            if (inner is TKey k)
            {
                key = k;
                return true;
            }
            var id = inner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is UserImpl user)
        {
            var inner = user._user;
            if (inner is TKey ik)
            {
                key = ik;
                return true;
            }
            if (user is DoctorImpl doctor && doctor._doctor is TKey idk)
            {
                key = idk;
                return true;
            }
            if (user is PatientImpl patient && patient._patient is TKey ipk)
            {
                key = ipk;
                return true;
            }
            var id = inner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is ProfileImpl profile)
        {
            var inner = profile._profile;
            if (inner is TKey k)
            {
                key = k;
                return true;
            }
            var id = inner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is AppointmentImpl appointment)
        {
            var inner = appointment._appointment;
            if (inner is TKey k)
            {
                key = k;
                return true;
            }
            var id = inner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is ExaminationImpl examination)
        {
            var inner = examination._examination;
            if (inner is TKey k)
            {
                key = k;
                return true;
            }
            var id = inner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is PrescriptionImpl prescription)
        {
            var inner = prescription._prescription;
            if (inner is TKey k)
            {
                key = k;
                return true;
            }
            var id = inner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        else if (entity is DiagnosticServiceImpl diagnosticService)
        {
            var eInner = diagnosticService._exdiag;
            if (eInner is TKey ek)
            {
                key = ek;
                return true;
            }
            var dInner = diagnosticService._diagsv;
            if (dInner is TKey dk)
            {
                key = dk;
                return true;
            }
            var id = eInner != null ? eInner.Id : dInner.Id;
            if (id is TKey uk)
            {
                key = uk;
                return true;
            }
            if (id.ToString() is TKey sk)
            {
                key = sk;
                return true;
            }
        }
        // TODO: ...more Impl
        key = default;
        return false;
    }
}