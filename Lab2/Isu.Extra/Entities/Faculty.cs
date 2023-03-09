using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Faculty : IEquatable<Faculty>
{
    private List<JgofsName> _jgofsNames = new List<JgofsName>();
    private List<char> _generalGroupsFacultyIds = new List<char>();

    private char[] _facultysId = { 'A', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'Z' };

    public Faculty(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new FacultyException("Invalid Faculty name");
        }

        Name = name;
    }

    public string Name { get; }
    public IReadOnlyList<JgofsName> JgofsNames => _jgofsNames.AsReadOnly();
    public IReadOnlyList<char> GeneralGroupsFacultyIds => _generalGroupsFacultyIds.AsReadOnly();

    public void AddJgofs(JgofsName name)
    {
        if (_jgofsNames.Contains(name))
        {
            throw new FacultyException("Jgof is already Exists");
        }

        _jgofsNames.Add(name);
    }

    public void AddGeneralGroupsFacultyId(char facultyId)
    {
        if (!_facultysId.Contains(facultyId))
        {
            throw new FacultyException("Invalid facultyId");
        }

        if (_generalGroupsFacultyIds.Contains(facultyId))
        {
            throw new FacultyException("FacultyId is already Exists");
        }

        _generalGroupsFacultyIds.Add(facultyId);
    }

    public bool Equals(Faculty? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Faculty)obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}