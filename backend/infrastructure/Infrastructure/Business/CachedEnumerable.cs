using System.Collections;

namespace AppointmentScheduler.Infrastructure.Business;

internal class CachedEnumerable<T> : IEnumerable<T>, IEnumerable, ICloneable
{
    internal readonly object _k;
    internal readonly IEnumerable<T> _q;
    internal IReadOnlyList<T> _l;

    internal CachedEnumerable(IEnumerable<T> query, IReadOnlyList<T> list = null)
    {
        _k = new();
        _q = query ?? throw new ArgumentNullException(nameof(query));
        _l = list is IList<T> ? list : [];
    }

    object ICloneable.Clone() => new CachedEnumerable<T>(_q);

    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

    internal class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
        private CachedEnumerable<T> _e;
        private IEnumerator<T> _r;
        private int _c;

        internal Enumerator(CachedEnumerable<T> enumerable)
        {
            _e = enumerable;
            _r = enumerable._q.GetEnumerator();
            _c = -1;
        }

        T IEnumerator<T>.Current
        {
            get
            {
                if (_e == null) throw new InvalidOperationException("Enumerator has been disposed.");
                if ((~_c) == 0) throw new InvalidOperationException("Enumerator has not been started.");
                if (_c >= _e._l.Count) throw new InvalidOperationException("Enumerator has been stopped.");
                return _e._l[_c];
            }
        }

        object IEnumerator.Current => ((IEnumerator<T>)this).Current;

        void IDisposable.Dispose()
        {
            _e = null;
            _c = 0;
            _r.Dispose();
        }

        bool IEnumerator.MoveNext()
        {
            if (_e == null) throw new InvalidOperationException("Enumerator had been disposed.");
            bool result = _r.MoveNext();
            lock (_e._k)
            {
                ++_c;
                if (_e._l is IList<T> list && !list.IsReadOnly)
                {
                    if (!result) _e._l = list.AsReadOnly();
                    else if (_c >= list.Count) list.Add(_r.Current);
                }
            }
            return result;
        }

        void IEnumerator.Reset()
        {
            _c = -1;
            _r.Reset();
        }
    }
}