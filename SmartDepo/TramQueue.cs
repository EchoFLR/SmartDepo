using SmartDepoLib.Toolbelt;

namespace SmartDepoLib;

internal class TramQueue
{
    #region Creation
    private readonly SortedSet<Tram> _tramsWithMission;
    private readonly SortedSet<Tram> _tramsWithoutMission;
    private readonly object _lockWithMission = new();
    private readonly object _lockWithoutMission = new();

    private TramQueue(IEnumerable<Tram> trams)
    {
        var comparer = new TramIdComparer();
        _tramsWithMission = new SortedSet<Tram>(comparer);
        _tramsWithoutMission = new SortedSet<Tram>(comparer);

        foreach (var tram in trams)
        {
            if (tram.HasMission)
            {
                _tramsWithMission.Add(tram);
            }
            else
            {
                _tramsWithoutMission.Add(tram);
            }
        }
    }

    internal static TramQueue CreateQueue(IEnumerable<Tram> trams)
    {
        return new TramQueue(trams);
    }
    #endregion

    #region Interaction

    internal IEnumerable<Tram> GetTrams()
    {
        lock (_lockWithoutMission)
        {
            lock (_lockWithMission)
            {
                return _tramsWithoutMission.Concat(_tramsWithMission)
                                           .OrderBy(tram => tram.Id)
                                           .ToList();
            }
        }
    }   

    internal async Task AssignMission(string mission, Func<Tram?, Task> onComplete)
    {
        Tram? assignedTram = await Task.Run(() =>
        {
            lock (_lockWithoutMission)
            {
                if (_tramsWithoutMission.Count == 0)
                {
                    return null;
                }

                var tram = _tramsWithoutMission.Min;
                _tramsWithoutMission.Remove(tram);

                lock (_lockWithMission)
                {
                    tram.AssignMission(mission);
                    _tramsWithMission.Add(tram);
                }

                return tram;
            }
        });

        if (onComplete != null)
        {
            await onComplete(assignedTram);
        }
    }

    internal async Task ClearAsync()
    {
        await Task.Run(() =>
        {
            lock (_lockWithoutMission)
            {
                lock (_lockWithMission)
                {
                    _tramsWithoutMission.Clear();
                    _tramsWithMission.Clear();
                }
            }
        });
    }

    #endregion
}
