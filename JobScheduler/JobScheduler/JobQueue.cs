using System.Collections;

namespace JobScheduler;

public class JobQueue : IEnumerable
{
    private Job[] _jobs;
    private int _count;

    public int Count => _count;

    public JobQueue(int initialCapacity = 4)
    {
        if (initialCapacity <= 0)
            initialCapacity = 4;

        _jobs = new Job[initialCapacity];
        _count = 0;
    }

    public void Enqueue(Job job)
    {
        if (_count == _jobs.Length)
        {
            Array.Resize(ref _jobs, _jobs.Length * 2);
        }

        _jobs[_count++] = job;
    }

    public IEnumerator GetEnumerator() => new JobQueueEnumerator(_jobs, _count);
    
    private struct JobQueueEnumerator : IEnumerator
    {
        private readonly Job[] _jobs;
        private readonly int _count;
        private int _index;
        private Job? _current;

        public JobQueueEnumerator(Job[] jobs, int count)
        {
            _jobs = jobs;
            _count = count;
            _index = -1;
            _current = null;
        }

        public object Current
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException("Enumerator is not positioned on a valid pending job.");

                return _current;
            }
        }

        public bool MoveNext()
        {
            while (true)
            {
                _index++;
                if (_index >= _count)
                {
                    _current = null;
                    return false;
                }

                var job = _jobs[_index];
                if (job != null && job.Status == JobStatus.Pending)
                {
                    _current = job;
                    return true;
                }
            }
        }

        public void Reset()
        {
            _index = -1;
            _current = null;
        }
    }
}