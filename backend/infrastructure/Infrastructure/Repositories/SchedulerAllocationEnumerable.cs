using System.Collections;
using System.Collections.Generic;

using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class SchedulerAllocationEnumerable : IDisposable, IEnumerable<SchedulerAllocation>, IEnumerable, IEnumerator<SchedulerAllocation>, IEnumerator
{
    private ISchedulerService _s;
    private TimeOnly _sfe, _sle, _v;
    private uint _id = 0;
    private SchedulerAllocation _allocation;

    internal SchedulerAllocationEnumerable(ISchedulerService schedulerService)
    {
        _s = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
        _sfe = ScaledEndOf(_s.FirstEnd, _s.FirstStart);
        _sle = ScaledEndOf(_s.LastEnd, _s.LastStart);
    }

    SchedulerAllocation IEnumerator<SchedulerAllocation>.Current
        => _allocation ??= new SchedulerAllocation() { Id = _id, AtTime = _v };

    object IEnumerator.Current => ((IEnumerator<SchedulerAllocation>)this).Current;

    void IDisposable.Dispose()
    {
        _s = null;
        _id = 0;
        _allocation = null;
    }

    IEnumerator<SchedulerAllocation> IEnumerable<SchedulerAllocation>.GetEnumerator()
        => new SchedulerAllocationEnumerable(_s);

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<SchedulerAllocation>)this).GetEnumerator();

    bool IEnumerator.MoveNext()
    {
        if (_id != 0)
        {
            if (_v > _sle) return false;
            else _v = _v.Add(_s.StepGap);
        }
        else _v = _s.FirstStart;        
        if (_v > _sfe && _v < _s.LastStart)
            _v = _s.LastStart;
        ++_id;
        _allocation = null;
        return true;
    }

    void IEnumerator.Reset()
    {
        _id = 0;
        _allocation = null;
    }

    private TimeOnly ScaledEndOf(TimeOnly end, TimeOnly start)
    {
        TimeOnly b = start, e = end;
        e = e.Add(TimeSpan.FromMinutes(
            (b - e).TotalMinutes % _s.BigStepGap.TotalMinutes
        ));
        return e;
    }

    /*
    static IEnumerable<SchedulerAllocation> GetSchedulerAllocations(byte[] permissions)
    {
        TimeOnly b, e;
        ISchedulerService s = this;
        uint id = 0;
        b = s.FirstStart;
        e = ScaledEndOf(s.FirstEnd, b);
        for (; b < e; b = b.Add(s.StepGap))
            yield return new() { Id = ++id, AtTime = b };
        b = s.LastStart;
        e = ScaledEndOf(s.LastEnd, b);
        for (; b < e; b = b.Add(s.StepGap))
            yield return new() { Id = ++id, AtTime = b };
    }
    */
}