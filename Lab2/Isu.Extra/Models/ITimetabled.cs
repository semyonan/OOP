using Isu.Extra.Entities;

namespace Isu.Extra.Models;

public interface ITimetabled
{
    public Timetable? Timetable { get; }
    public abstract void AddTimetable(Timetable timetable);
}