using System.Collections;
using System.Collections.Generic;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Business;

internal struct PermissionEnumerator : IEnumerable<Permission>, IEnumerable, IEnumerator<Permission>, IEnumerator, IDisposable
{
    private byte[] _permissions;
    private long _byteIndex, _bitIndex;

    public PermissionEnumerator(byte[] permissions)
    {
        _permissions = permissions;
        _byteIndex = -1;
    }

    public readonly Permission Current
    {
        get
        {
            if (_permissions == null) throw new InvalidOperationException("Enumerator has been disposed.");
            if ((~_byteIndex) == 0) throw new InvalidOperationException("Enumerator has not been started.");
            if (_byteIndex >= _permissions.LongLength) throw new InvalidOperationException("Enumerator has been stopped.");
            return (Permission)((((ulong)_byteIndex) << 3) | ((~(ulong)_bitIndex) & 7));
        }
    }

    readonly object IEnumerator.Current => Current;

    public readonly IEnumerator<Permission> GetEnumerator() => new PermissionEnumerator(_permissions);

    void IDisposable.Dispose()
    {
        _permissions = null;
        _byteIndex = 0;
        _bitIndex = 0;
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    bool IEnumerator.MoveNext()
    {
        if (_permissions == null) throw new InvalidOperationException("Enumerator had been disposed.");
        long x = _byteIndex, y = _bitIndex, l = _permissions.LongLength;
        if (x >= l) return false;
        if ((~x) != 0) goto eachbit;
        x = 0;
        y = 8;
    eachbyte:
        if (x >= l)
        {
            _byteIndex = x;
            _bitIndex = y;
            return false;
        }
        if (_permissions[x] == 0)
        {
            ++x;
            goto eachbyte;
        }
    eachbit:
        if (--y < 0)
        {
            ++x;
            y = 8;
            goto eachbyte;
        }
        if (((((ulong)_permissions[x]) >> (int)(y & 7)) & 1) == 0) goto eachbit;
        _byteIndex = x;
        _bitIndex = y;
        return true;
    }

    void IEnumerator.Reset()
    {
        _byteIndex = -1;
        _bitIndex = 0;
    }

    /*
    private static IEnumerable<Permission> GetPermissions(byte[] permissions)
    {
        for (long _byteIndex = 0, _length = permissions.LongLength; _byteIndex < _length; ++_byteIndex)
        {
            if (permissions[_byteIndex] == 0) continue;
            for (long _bitIndex = 8; (--_bitIndex) >= 0; )
            {
                if (((((ulong)permissions[_byteIndex]) >> (int)(_bitIndex & 7)) & 1) != 0)
                {
                    yield return (Permission)(((ulong)_byteIndex << 3) | (~(ulong)_bitIndex & 7));
                }
            }
        }
    }
    */
}