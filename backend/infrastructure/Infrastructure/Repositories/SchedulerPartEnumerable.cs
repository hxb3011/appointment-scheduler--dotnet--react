using System.Collections;
using System.Collections.Generic;

using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class SchedulerPartEnumerable : IDisposable, IEnumerable<SchedulerPart>, IEnumerable, IEnumerator<SchedulerPart>, IEnumerator
{
    private ISchedulerService _s;
    private TimeOnly _b, _e;
    private uint _id = 0;
    private SchedulerPart _part;

    internal SchedulerPartEnumerable(ISchedulerService schedulerService)
        => _s = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));

    SchedulerPart IEnumerator<SchedulerPart>.Current
        => _part ??= new SchedulerPart() { Id = _id, Start = _b, End = _e };

    object IEnumerator.Current => ((IEnumerator<SchedulerPart>)this).Current;

    void IDisposable.Dispose()
    {
        _s = null;
        _id = 0;
        _part = null;
    }

    IEnumerator<SchedulerPart> IEnumerable<SchedulerPart>.GetEnumerator()
        => new SchedulerPartEnumerable(_s);

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<SchedulerPart>)this).GetEnumerator();

    bool IEnumerator.MoveNext()
    {
        _b = _id != 0 ? _e : _s.FirstStart;
        _e = _b.Add(_s.BigStepGap);
        if (_e > _s.LastEnd) return false;
        if (_e > _s.FirstEnd && _b < _s.LastStart)
        {
            _b = _s.LastStart;
            _e = _b.Add(_s.BigStepGap);
        }
        ++_id;
        _part = null;
        return true;
    }

    void IEnumerator.Reset()
    {
        _id = 0;
        _part = null;
    }

    /*
    static IEnumerable<SchedulerPart> GetSchedulerParts(byte[] permissions)
    {
        TimeOnly b, e, m;
        ISchedulerService s = this;
        uint id = 0;
        b = s.FirstStart;
        e = s.FirstEnd;
        while ((m = b.Add(s.BigStepGap)) <= e)
            yield return new() { Id = ++id, Start = b, End = b = m };
        b = s.LastStart;
        e = s.LastEnd;
        while ((m = b.Add(s.BigStepGap)) <= e)
            yield return new() { Id = ++id, Start = b, End = b = m };
    }
    */
}