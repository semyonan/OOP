using Isu.Entities;

namespace Isu.Extra.Entities;

public class TimetabledStudent : Student
{
    public TimetabledStudent(string name, int id, TimetabledGroup genralGroup)
        : base(name, id, genralGroup)
    {
        JgofsGroup = null;
    }

    public JgofsGroup? JgofsGroup { get; private set; }

    public void AddJgofsGroup(JgofsGroup jgofsGroup)
    {
        JgofsGroup = jgofsGroup;
    }

    public void RemoveJgofsGroup()
    {
        JgofsGroup = null;
    }
}