using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Org.BouncyCastle.Asn1.Cms;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class SchedulerServiceImpl : ISchedulerService
{
    private static readonly TimeOnly DefaultFirstStart = new(7, 30);
    private static readonly TimeOnly DefaultFirstEnd = new(11, 30);
    private static readonly TimeOnly DefaultLastStart = new(13, 0);
    private static readonly TimeOnly DefaultLastEnd = new(17, 0);
    private static readonly TimeSpan DefaultBigStepGap = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan DefaultStepGap = TimeSpan.FromMinutes(7);

    private static readonly int InitFlagFirstStart = 1 << 5;
    private static readonly int InitFlagFirstEnd = 1 << 4;
    private static readonly int InitFlagLastStart = 1 << 3;
    private static readonly int InitFlagLastEnd = 1 << 2;
    private static readonly int InitFlagBigStepGap = 1 << 1;
    private static readonly int InitFlagStepGap = 1 << 0;

    private static readonly string IdentifierPrefix = typeof(ISchedulerService).FullName;
    private static readonly string IdentifierFirstStart = IdentifierPrefix + ".FirstStart";
    private static readonly string IdentifierFirstEnd = IdentifierPrefix + ".FirstEnd";
    private static readonly string IdentifierLastStart = IdentifierPrefix + ".LastStart";
    private static readonly string IdentifierLastEnd = IdentifierPrefix + ".LastEnd";
    private static readonly string IdentifierBigStepGap = IdentifierPrefix + ".BigStepGap";
    private static readonly string IdentifierStepGap = IdentifierPrefix + ".StepGap";

    private int initFlags;
    private TimeOnly _firstStart;
    private TimeOnly _firstEnd;
    private TimeOnly _lastStart;
    private TimeOnly _lastEnd;
    private TimeSpan _bigStepGap;
    private TimeSpan _stepGap;
    private IEnumerable<SchedulerPart> _parts;
    private IEnumerable<SchedulerAllocation> _allocations;

    private readonly IRepository _repository;
    private readonly IConfigurationPropertiesService _configuration;
    internal SchedulerServiceImpl(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _configuration = repository.GetService<IConfigurationPropertiesService>().WaitForResult();
    }

    TimeOnly ISchedulerService.FirstStart
    {
        get => GetField(ref _firstStart, InitFlagFirstStart, IdentifierFirstStart, DefaultFirstStart);
        set => SetField(ref _firstStart, InitFlagFirstStart, IdentifierFirstStart, value);
    }

    TimeOnly ISchedulerService.FirstEnd
    {
        get => GetField(ref _firstEnd, InitFlagFirstEnd, IdentifierFirstEnd, DefaultFirstEnd);
        set => SetField(ref _firstEnd, InitFlagFirstEnd, IdentifierFirstEnd, value);
    }

    TimeOnly ISchedulerService.LastStart
    {
        get => GetField(ref _lastStart, InitFlagLastStart, IdentifierLastStart, DefaultLastStart);
        set => SetField(ref _lastStart, InitFlagLastStart, IdentifierLastStart, value);
    }

    TimeOnly ISchedulerService.LastEnd
    {
        get => GetField(ref _lastEnd, InitFlagLastEnd, IdentifierLastEnd, DefaultLastEnd);
        set => SetField(ref _lastEnd, InitFlagLastEnd, IdentifierLastEnd, value);
    }

    TimeSpan ISchedulerService.BigStepGap
    {
        get => GetField(ref _bigStepGap, InitFlagBigStepGap, IdentifierBigStepGap, DefaultBigStepGap);
        set => SetField(ref _bigStepGap, InitFlagBigStepGap, IdentifierBigStepGap, value);
    }

    TimeSpan ISchedulerService.StepGap
    {
        get => GetField(ref _stepGap, InitFlagStepGap, IdentifierStepGap, DefaultStepGap);
        set => SetField(ref _stepGap, InitFlagStepGap, IdentifierStepGap, value);
    }

    IEnumerable<SchedulerPart> ISchedulerService.Parts
    {
        get
        {
            var parts = _parts;
            if (parts != null) return parts;
            lock (this)
            {
                if ((parts = _parts) != null) return parts;
                TimeOnly b, e, m;
                ISchedulerService s = this;
                List<SchedulerPart> list = new();
                uint id = 1;
                b = s.FirstStart;
                e = s.FirstEnd;
                while ((m = b.Add(s.BigStepGap)) <= e)
                    list.Add(new() { Id = id, Start = b, End = b = m });
                b = s.LastStart;
                e = s.LastEnd;
                while ((m = b.Add(s.BigStepGap)) <= e)
                    list.Add(new() { Id = id, Start = b, End = b = m });
                _parts = parts = list;
            }
            return parts;
        }
    }

    IEnumerable<SchedulerAllocation> ISchedulerService.Allocations
    {
        get
        {
            var allocations = _allocations;
            if (allocations != null) return allocations;
            lock (this)
            {
                if ((allocations = _allocations) != null) return allocations;
                TimeOnly b, e;
                ISchedulerService s = this;
                List<SchedulerAllocation> list = new();
                uint id = 1;
                b = s.FirstStart;
                e = ScaledEndOf(s.FirstEnd, b);
                for (; b < e; b = b.Add(s.StepGap))
                    list.Add(new() { Id = id, AtTime = b });
                b = s.LastStart;
                e = ScaledEndOf(s.LastEnd, b);
                for (; b < e; b = b.Add(s.StepGap))
                    list.Add(new() { Id = id, AtTime = b });
                _allocations = allocations = list;
            }
            return allocations;
        }
    }

    async Task<SchedulerAllocation> ISchedulerService.Allocate(IDoctor doctor, DateOnly date, TimeOnly start, TimeOnly end)
    {
        var now = DateTime.UtcNow;
        if (date <= DateOnly.FromDateTime(now)) return null;
        ISchedulerService s = this;
        if (start >= end) return null;
        if ((start < s.FirstStart || end > s.FirstEnd)
            && (start < s.LastStart || end > s.LastEnd)) return null;
        var query = (
            from ap in doctor.Appointments
            let dt = ap.AtTime
            let d = DateOnly.FromDateTime(dt)
            let t = TimeOnly.FromDateTime(dt)
            where d == date && start <= t && t < end
            orderby t ascending
            select t
        );
        var it = query.GetEnumerator();
        if (!it.MoveNext())
            return (
                from al in s.Allocations
                let t = al.AtTime
                where start <= t && t < end
                select al
            ).FirstOrDefault();
        TimeOnly last;
        do last = it.Current; while (it.MoveNext());
        last = last.Add(s.StepGap);
        return (
            from al in s.Allocations
            where al.AtTime == last
            select al
        ).FirstOrDefault();
    }

    async Task<SchedulerAllocation> ISchedulerService.Allocate(IDoctor doctor)
    {
        var now = DateTime.UtcNow;
        var dnow = DateOnly.FromDateTime(now);
        var tnow = TimeOnly.FromDateTime(now);

        var ait = (
            from ap in doctor.Appointments
            let dt = ap.AtTime
            let d = DateOnly.FromDateTime(dt)
            let t = TimeOnly.FromDateTime(dt)
            where d == dnow && t > tnow
            orderby t ascending
            select t
        ).GetEnumerator();

        ISchedulerService s = this;
        var bit = (
            from al in s.Allocations
            let t = al.AtTime
            where t > tnow
            orderby t ascending
            select al
        ).GetEnumerator();

        while (true)
        {
            if (!ait.MoveNext())
                return bit.MoveNext() ? bit.Current : null;
            if (!bit.MoveNext()) return null;
            var b = bit.Current;
            if (b.AtTime < ait.Current) return b;
        }
    }

    private TimeOnly ScaledEndOf(TimeOnly end, TimeOnly start)
    {
        ISchedulerService s = this;
        TimeOnly b = start, e = end;
        e = e.Add(TimeSpan.FromMinutes(
            (b - e).TotalMinutes % s.BigStepGap.TotalMinutes
        ));
        return e;
    }

    private TimeSpan GetField(ref TimeSpan field, int initFlag, string identifier, TimeSpan defaultValue)
    {
        if ((initFlags & initFlag) == 0)
        {
            initFlags |= initFlag;
            var prop = ((ulong)defaultValue.TotalMinutes).ToString();
            prop = _configuration.GetProperty(identifier, prop);
            if (ulong.TryParse(prop, out ulong u))
                return field = TimeSpan.FromMinutes(u);
            return field = defaultValue;
        }
        return field;
    }

    private TimeOnly GetField(ref TimeOnly field, int initFlag, string identifier, TimeOnly defaultValue)
    {
        if ((initFlags & initFlag) == 0)
        {
            initFlags |= initFlag;
            var prop = ((ulong)defaultValue.Hour * 60UL
                    + (ulong)defaultValue.Minute).ToString();
            prop = _configuration.GetProperty(identifier, prop);
            if (ulong.TryParse(prop, out ulong u) && u < 1440UL)
                return field = new TimeOnly((int)(u / 60), (int)(u % 60));
            return field = defaultValue;
        }
        return field;
    }

    private void SetField(ref TimeSpan field, int initFlag, string identifier, TimeSpan value)
    {
        field = value;
        _parts = null;
        _allocations = null;
        if ((initFlags & initFlag) == 0) initFlags |= initFlag;
        _configuration.SetProperty(identifier, ((ulong)value.TotalMinutes).ToString());
    }

    private void SetField(ref TimeOnly field, int initFlag, string identifier, TimeOnly value)
    {
        field = value;
        _parts = null;
        _allocations = null;
        if ((initFlags & initFlag) == 0) initFlags |= initFlag;
        _configuration.SetProperty(identifier, ((ulong)value.Hour * 60UL + (ulong)value.Minute).ToString());
    }
}