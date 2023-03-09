using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Service;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    [Fact]
    public void AddJgofs()
    {
        var isu = new IsuExtraService();

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);

        Assert.True(isu.GetJgofsStream(new StreamName("КБ", 3)).ListOfGroups.Count == 2);
    }

    [Fact]
    public void SetTimetable()
    {
        var isu = new IsuExtraService();

        isu.AddGroup(new GroupName("M32061"));

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);

        var practiceBuilder = new PracticeBuilder();
        var lectureBuilder = new LectureBuilder();

        Practice jgofsPractice = practiceBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .Group(isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))))
            .Build();

        Lecture jgofsLecture = lectureBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .GroupStream(isu.GetJgofsStream(new StreamName("КБ", 1)))
            .Build();

        Lecture groupLecture1 = lectureBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Lecture groupLecture2 = lectureBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(13, 30), new TimeOnly(15, 0)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Practice groupPractice1 = practiceBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        Practice groupPractice2 = practiceBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(11, 40), new TimeOnly(13, 10)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        isu.SetTimetableForJgofsStream(
            new StreamName("КБ", 1),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { jgofsLecture }) });

        isu.SetTimetableForJgofsGroup(
            new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Practice> { jgofsPractice }) });

        isu.SetTimetableForGroupStream(
            new StreamName("M32", 2),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { groupLecture1, groupLecture2 }) });

        isu.SetTimetableForGroup(
            new GroupName("M32061"),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Monday, Timetable.Week.Odd, new List<Practice> { groupPractice1, groupPractice2 }) });

        Timetable groupTimetable = isu.GetGroupTimetable(new GroupName("M32061"));
        Timetable jgofsGroupTimetable = isu.GetJgofsGroupTimetable(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")));

        Assert.Contains(groupPractice1, groupTimetable.DayTimetables[0].LessonsList);
        Assert.Contains(groupPractice2, groupTimetable.DayTimetables[0].LessonsList);
        Assert.Contains(groupLecture1, groupTimetable.DayTimetables[3].LessonsList);
        Assert.Contains(groupLecture2, groupTimetable.DayTimetables[3].LessonsList);
        Assert.Contains(jgofsPractice, jgofsGroupTimetable.DayTimetables[3].LessonsList);
        Assert.Contains(jgofsLecture, jgofsGroupTimetable.DayTimetables[3].LessonsList);
    }

    [Fact]
    public void SetTimetable_ThrowsException()
    {
        var isu = new IsuExtraService();

        isu.AddGroup(new GroupName("M32061"));

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);

        var practiceBuilder = new PracticeBuilder();
        var lectureBuilder = new LectureBuilder();

        Practice jgofsPractice = practiceBuilder
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .Group(isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))))
            .Build();

        Lecture jgofsLecture = lectureBuilder
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .GroupStream(isu.GetJgofsStream(new StreamName("КБ", 1)))
            .Build();

        isu.SetTimetableForJgofsStream(
            new StreamName("КБ", 1),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { jgofsLecture }) });

        Assert.Throws<IsuExtraServiceException>(() => isu.SetTimetableForJgofsGroup(
            new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Practice> { jgofsPractice }) }));
    }

    [Fact]
    public void EnrollStudentToJgofsThenRemove()
    {
        var isu = new IsuExtraService();

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);
        isu.AddGroup(new GroupName("M32061"));

        var practiceBuilder = new PracticeBuilder();
        var lectureBuilder = new LectureBuilder();

        Practice jgofsPractice = practiceBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .Group(isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))))
            .Build();

        Lecture jgofsLecture = lectureBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .GroupStream(isu.GetJgofsStream(new StreamName("КБ", 1)))
            .Build();

        Lecture groupLecture1 = lectureBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Lecture groupLecture2 = lectureBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(13, 30), new TimeOnly(15, 0)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Practice groupPractice1 = practiceBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        Practice groupPractice2 = practiceBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(11, 40), new TimeOnly(13, 10)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        isu.SetTimetableForJgofsStream(
            new StreamName("КБ", 1),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { jgofsLecture }) });

        isu.SetTimetableForJgofsGroup(
            new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Practice> { jgofsPractice }) });

        isu.SetTimetableForGroupStream(
            new StreamName("M32", 2),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { groupLecture1, groupLecture2 }) });

        isu.SetTimetableForGroup(
            new GroupName("M32061"),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Monday, Timetable.Week.Odd, new List<Practice> { groupPractice1, groupPractice2 }) });

        TimetabledGroup group = isu.GetGroup(new GroupName("M32061"));
        TimetabledStudent student = isu.AddStudent(group, "Аня Семенова");

        isu.EnrollStudentToJgofs(student, "КТУ", new JgofsName("КБ"));

        Assert.Contains(student, isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))).ListOfStudents);

        isu.RemoveStudentFromJgofsGroup(student);

        Assert.DoesNotContain(student, isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))).ListOfStudents);
    }

    [Fact]
    public void EnrollStudentToHisJgofs_ThrowsException()
    {
        var isu = new IsuExtraService();

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);
        isu.AddGroup(new GroupName("K32061"));

        var practiceBuilder = new PracticeBuilder();
        var lectureBuilder = new LectureBuilder();

        Practice jgofsPractice = practiceBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .Group(isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))))
            .Build();

        Lecture jgofsLecture = lectureBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .GroupStream(isu.GetJgofsStream(new StreamName("КБ", 1)))
            .Build();

        Lecture groupLecture1 = lectureBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("K32", 2)))
            .Build();

        Lecture groupLecture2 = lectureBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(13, 30), new TimeOnly(15, 0)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("K32", 2)))
            .Build();

        Practice groupPractice1 = practiceBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .Group(isu.GetGroup(new GroupName("K32061")))
            .Build();

        Practice groupPractice2 = practiceBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(11, 40), new TimeOnly(13, 10)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .Group(isu.GetGroup(new GroupName("K32061")))
            .Build();

        isu.SetTimetableForJgofsStream(
            new StreamName("КБ", 1),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
            { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { jgofsLecture }) });

        isu.SetTimetableForJgofsGroup(
            new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
             { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Practice> { jgofsPractice }) });

        isu.SetTimetableForGroupStream(
            new StreamName("K32", 2),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
             { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { groupLecture1, groupLecture2 }) });

        isu.SetTimetableForGroup(
            new GroupName("K32061"),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
             { new (Timetable.Day.Monday, Timetable.Week.Odd, new List<Practice> { groupPractice1, groupPractice2 }) });

        TimetabledGroup group = isu.GetGroup(new GroupName("K32061"));

        TimetabledStudent student = isu.AddStudent(group, "Аня Семенова");

        Assert.Throws<IsuExtraServiceException>(() => isu.EnrollStudentToJgofs(student, "КТУ", new JgofsName("КБ")));
    }

    [Fact]
    public void GetJgofsStreams()
    {
        var isu = new IsuExtraService();

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);
        isu.AddGroup(new GroupName("K32061"));

        Assert.True(isu.GetAllJgofsStreams(new JgofsName("КБ")) != null);
        Assert.True(isu.GetAllJgofsStreams(new JgofsName("КБ")).Count == 3);
    }

    [Fact]
    public void GetJgofsStudentList()
    {
        var isu = new IsuExtraService();

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);
        isu.AddGroup(new GroupName("M32061"));

        var practiceBuilder = new PracticeBuilder();
        var lectureBuilder = new LectureBuilder();

        Practice jgofsPractice = practiceBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .Group(isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))))
            .Build();

        Lecture jgofsLecture = lectureBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .GroupStream(isu.GetJgofsStream(new StreamName("КБ", 1)))
            .Build();

        Lecture groupLecture1 = lectureBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Lecture groupLecture2 = lectureBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(13, 30), new TimeOnly(15, 0)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Practice groupPractice1 = practiceBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        Practice groupPractice2 = practiceBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(11, 40), new TimeOnly(13, 10)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        isu.SetTimetableForJgofsStream(
            new StreamName("КБ", 1),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { jgofsLecture }) });

        isu.SetTimetableForJgofsGroup(
            new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Practice> { jgofsPractice }) });

        isu.SetTimetableForGroupStream(
            new StreamName("M32", 2),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { groupLecture1, groupLecture2 }) });

        isu.SetTimetableForGroup(
            new GroupName("M32061"),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Monday, Timetable.Week.Odd, new List<Practice> { groupPractice1, groupPractice2 }) });

        TimetabledGroup group = isu.GetGroup(new GroupName("M32061"));
        TimetabledStudent student = isu.AddStudent(group, "Аня Семенова");

        isu.EnrollStudentToJgofs(student, "КТУ", new JgofsName("КБ"));

        Assert.Contains(student, isu.GetJgofsGroupStudents(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))));
    }

    [Fact]
    public void GetStudnetWithOutJgofs()
    {
        var isu = new IsuExtraService();

        isu.AddJgofs(new JgofsName("КБ"), "КТУ", 3, 2);
        isu.AddGroup(new GroupName("M32061"));

        var practiceBuilder = new PracticeBuilder();
        var lectureBuilder = new LectureBuilder();

        Practice jgofsPractice = practiceBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .Group(isu.GetJgofsGroup(new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ"))))
            .Build();

        Lecture jgofsLecture = lectureBuilder
            .Reset()
            .Name("Основы кибербезопасности")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2233))
            .Teacher(new Teacher("Евгений Васильевич"))
            .GroupStream(isu.GetJgofsStream(new StreamName("КБ", 1)))
            .Build();

        Lecture groupLecture1 = lectureBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(8, 20), new TimeOnly(9, 50)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Lecture groupLecture2 = lectureBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(13, 30), new TimeOnly(15, 0)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .GroupStream(isu.GetGroupStream(new StreamName("M32", 2)))
            .Build();

        Practice groupPractice1 = practiceBuilder
            .Reset()
            .Name("Математика")
            .TimeInterval(new TimeInterval(new TimeOnly(10, 0), new TimeOnly(11, 30)))
            .ClassroomNumber(new ClassroomNumber(2234))
            .Teacher(new Teacher("Иван Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        Practice groupPractice2 = practiceBuilder
            .Reset()
            .Name("Информатика")
            .TimeInterval(new TimeInterval(new TimeOnly(11, 40), new TimeOnly(13, 10)))
            .ClassroomNumber(new ClassroomNumber(2235))
            .Teacher(new Teacher("Сергей Васильевич"))
            .Group(isu.GetGroup(new GroupName("M32061")))
            .Build();

        isu.SetTimetableForJgofsStream(
            new StreamName("КБ", 1),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { jgofsLecture }) });

        isu.SetTimetableForJgofsGroup(
            new JgofsGroupName("КБ 1.1", isu.GetFaculty("КТУ")),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Practice> { jgofsPractice }) });

        isu.SetTimetableForGroupStream(
            new StreamName("M32", 2),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>>
                { new (Timetable.Day.Thursday, Timetable.Week.Odd, new List<Lecture> { groupLecture1, groupLecture2 }) });

        isu.SetTimetableForGroup(
            new GroupName("M32061"),
            new List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>>
                { new (Timetable.Day.Monday, Timetable.Week.Odd, new List<Practice> { groupPractice1, groupPractice2 }) });

        TimetabledGroup group = isu.GetGroup(new GroupName("M32061"));

        TimetabledStudent studentWithJgofs = isu.AddStudent(group, "Аня Семенова");
        TimetabledStudent studentWithOutJgofs = isu.AddStudent(group, "Олег Иванов");

        isu.EnrollStudentToJgofs(studentWithJgofs, "КТУ", new JgofsName("КБ"));

        Assert.Contains(studentWithOutJgofs, isu.GetStudentsInGroupWithOutJgofs(new GroupName("M32061")));
        Assert.DoesNotContain(studentWithJgofs, isu.GetStudentsInGroupWithOutJgofs(new GroupName("M32061")));
    }
}
