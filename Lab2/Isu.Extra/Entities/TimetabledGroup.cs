using Isu.Entities;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Entities;

public class TimetabledGroup : Group, ITimetabled
{
    public TimetabledGroup(GroupName groupName, Stream<TimetabledGroup> stream)
        : base(groupName)
    {
        Timetable = null;
        Stream = stream;
    }

    public Timetable? Timetable { get; private set; }
    public Stream<TimetabledGroup> Stream { get; }
    public void AddTimetable(Timetable timetable)
    {
        Timetable = timetable;
    }
}