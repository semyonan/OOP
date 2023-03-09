using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Stream<TGroup>
where TGroup : ITimetabled
{
    private const int MaxGroupNumber = 5;
    private readonly List<TGroup> _listOfGroups;

    public Stream(StreamName name)
    {
        _listOfGroups = new List<TGroup>();
        StreamName = name;
        Timetable = null;
    }

    public StreamName StreamName { get; }
    public Timetable? Timetable { get; private set; }
    public List<TGroup> ListOfGroups => _listOfGroups;

    public void AddTimetable(Timetable timetable)
    {
        Timetable = timetable;
    }

    public void AddGroup(TGroup group)
    {
        if (_listOfGroups.Contains(group))
        {
            throw new StreamException("Group is already in this group");
        }

        if (_listOfGroups.Count == MaxGroupNumber)
        {
            throw new StreamException("The maximum number of groups has been reached");
        }

        _listOfGroups.Add(group);
    }

    public void RemoveGroup(TGroup group)
    {
        if (!_listOfGroups.Contains(group))
        {
            throw new StreamException("Group doesn't contain this student");
        }

        _listOfGroups.Remove(group);
    }
}