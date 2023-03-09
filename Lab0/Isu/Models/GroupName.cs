using Isu.Exception;

namespace Isu.Models;

public class GroupName
{
    private const int MaxGroupNameLength = 6;
    private const int MinGroupNameLength = 5;

    private static readonly char[] FacultyId = { 'A', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'Z' };
    private static readonly char[] GradeId = { '3', '4' };
    private static readonly char[] CoursesNumber = { '1', '2', '3', '4' };
    private static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    public GroupName(string name)
    {
        if (string.IsNullOrEmpty(name)
            || (name.Length is > MaxGroupNameLength or < MinGroupNameLength)
            || (!FacultyId.Contains(name[0]))
            || (!GradeId.Contains(name[1]))
            || (!CoursesNumber.Contains(name[2]))
            || (!Numbers.Contains(name[3]))
            || (!Numbers.Contains(name[4])) || (name[4] == '0')
            || ((name.Length == MaxGroupNameLength) && (!Numbers.Contains(name[5]))))
        {
            throw new IsuException("Invalid GroupName");
        }

        Name = name;
        Faculty = name[0];
        Grade = name[1];
        CourseNumber = new CourseNumber(uint.Parse(name[2].ToString()));
        GroupNumber = 10 * uint.Parse(name[3].ToString());
        GroupNumber += uint.Parse(name[4].ToString());

        if (name.Length == MaxGroupNameLength)
        {
            SpecializationNumber = uint.Parse(name[5].ToString());
        }
    }

    public string Name { get; }
    public char Faculty { get; }
    public char Grade { get; }
    public CourseNumber CourseNumber { get; }
    public uint GroupNumber { get; }
    public uint? SpecializationNumber { get; }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is GroupName groupName)
        {
            return groupName.Name == Name;
        }

        return false;
    }
}