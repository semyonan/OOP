using Isu.Extra.Exceptions;
using Isu.Models;

namespace Isu.Extra.Models;
public class StreamName : IEquatable<StreamName>
{
    private const int MaxStreamNameLength = 3;
    private const int MinStreamNameLength = 2;
    private const int MaxStreamNumber = 5;
    private const int MinStreamNumber = 1;

    private char[] _facultyId = { 'A', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'Z' };
    private char[] _gradeId = { '3', '4' };
    private char[] _coursesNumber = { '1', '2', '3', '4' };

    public StreamName(string name, int number)
    {
        if (string.IsNullOrEmpty(name)
            || (name.Length is < MinStreamNameLength or > MaxStreamNameLength)
            || (number is < MinStreamNumber or > MaxStreamNumber))
        {
            throw new StreamException("Invalid StreamName");
        }

        if (_facultyId.Contains(name[0])
            && _gradeId.Contains(name[1])
            && _coursesNumber.Contains(name[2]))
        {
            GroupsType = GorupsTypeId.General;
        }
        else
        {
            GroupsType = GorupsTypeId.Jgofs;
        }

        Name = name;
        Number = number;
    }

    public enum GorupsTypeId
    {
        General,
        Jgofs,
    }

    public string Name { get; }
    public GorupsTypeId GroupsType { get; }
    public int Number { get; }

    public bool Equals(StreamName? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && GroupsType == other.GroupsType && Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StreamName)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, (int)GroupsType, Number);
    }
}