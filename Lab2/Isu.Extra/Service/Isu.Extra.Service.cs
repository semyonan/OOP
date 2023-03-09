using System.Collections.Immutable;
using System.Data;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Service;

public class IsuExtraService
{
    private const int MaxIsuId = 999999;
    private const int MaxGroupsInStreamNumber = 5;

    private readonly List<Faculty> _facultiesList = new List<Faculty>();
    private readonly List<Stream<JgofsGroup>> _jgofsStreamsGroupList = new List<Stream<JgofsGroup>>();
    private readonly List<Stream<TimetabledGroup>> _groupStreamList = new List<Stream<TimetabledGroup>>();
    private readonly List<JgofsGroup> _jgofsGroupList = new List<JgofsGroup>();
    private readonly List<TimetabledGroup> _groupList = new List<TimetabledGroup>();
    private readonly List<TimetabledStudent> _studentList = new List<TimetabledStudent>();
    private readonly List<Lesson> _lessonsList = new List<Lesson>();

    private int _isuId = 100000;

    public IsuExtraService()
    {
        SetDefaultFaculties();
    }

    public TimetabledGroup AddGroup(GroupName groupName)
    {
        if (GroupExists(groupName))
        {
            throw new IsuExtraServiceException("Group with this name already exists");
        }

        Stream<TimetabledGroup>? selectedStream = _groupStreamList
            .FirstOrDefault(x =>
                groupName.Name.Contains(x.StreamName.Name) && x.StreamName.Number == (groupName.GroupNumber / MaxGroupsInStreamNumber) + 1);

        if (selectedStream == null)
        {
            selectedStream = new Stream<TimetabledGroup>(
                new StreamName(groupName.Name.Substring(0, 3), (int)(groupName.GroupNumber / MaxGroupsInStreamNumber) + 1));

            _groupStreamList.Add(selectedStream);
        }

        var group = new TimetabledGroup(groupName, selectedStream);

        _groupList.Add(group);

        selectedStream.AddGroup(group);

        return group;
    }

    public TimetabledStudent AddStudent(TimetabledGroup group, string name)
    {
        TimetabledGroup? currentGroup = FindGroup(group.GroupName);

        if (currentGroup == null)
        {
            throw new IsuExtraServiceException("Group doesn't exists");
        }

        if (_isuId > MaxIsuId)
        {
            throw new IsuExtraServiceException("Maximum student ID has been reached");
        }

        var student = new TimetabledStudent(name, _isuId, currentGroup);
        _isuId++;

        currentGroup.AddStudent(student);

        _studentList.Add(student);

        return student;
    }

    public TimetabledStudent GetStudent(uint id)
    {
        TimetabledStudent? student = FindStudent(id);

        if (student == null)
        {
            throw new IsuExtraServiceException("Student with this ID doesn't exists");
        }

        return student;
    }

    public Faculty GetFaculty(string facultyName)
    {
        Faculty? faculty = FindFaculty(facultyName);

        if (faculty == null)
        {
            throw new IsuExtraServiceException("Faculty doesn't exists");
        }

        return faculty;
    }

    public JgofsGroup GetJgofsGroup(JgofsGroupName jgofsGroupName)
    {
        JgofsGroup? jgofsGroup = FindJgofsGroup(jgofsGroupName);

        if (jgofsGroup == null)
        {
            throw new IsuExtraServiceException("JgofsGroup doesn't exists");
        }

        return jgofsGroup;
    }

    public TimetabledGroup GetGroup(GroupName groupName)
    {
        TimetabledGroup? group = FindGroup(groupName);

        if (group == null)
        {
            throw new IsuExtraServiceException("Group doesn't exists");
        }

        return group;
    }

    public Stream<JgofsGroup> GetJgofsStream(StreamName streamName)
    {
        Stream<JgofsGroup>? stream = FindJgofsGroupStream(streamName);

        if (stream == null)
        {
            throw new IsuExtraServiceException("Stream doesn't exists");
        }

        return stream;
    }

    public Stream<TimetabledGroup> GetGroupStream(StreamName streamName)
    {
        Stream<TimetabledGroup>? stream = FindTimetabledGroupStream(streamName);

        if (stream == null)
        {
            throw new IsuExtraServiceException("Stream doesn't exists");
        }

        return stream;
    }

    public Timetable GetGroupTimetable(GroupName groupName)
    {
        TimetabledGroup? group = FindGroup(groupName);

        if (group == null)
        {
            throw new IsuExtraServiceException("Group doesn't exists");
        }

        var builder = new TimetableBuilder();
        Timetable timetable = builder.AddLessonsFromTimetable(group.Stream.Timetable)
            .AddLessonsFromTimetable(group.Timetable).Build();

        return timetable;
    }

    public Timetable GetJgofsGroupTimetable(JgofsGroupName jgofsGroupName)
    {
        JgofsGroup? jgofsGroup = FindJgofsGroup(jgofsGroupName);

        if (jgofsGroup == null)
        {
            throw new IsuExtraServiceException("JgofsGroup doesn't exists");
        }

        var builder = new TimetableBuilder();
        Timetable timetable = builder.AddLessonsFromTimetable(jgofsGroup.Stream.Timetable)
            .AddLessonsFromTimetable(jgofsGroup.Timetable).Build();

        return timetable;
    }

    public Timetable GetStudentsTimetable(uint id)
    {
        TimetabledStudent? selectedStudent = FindStudent(id);

        if (selectedStudent == null)
        {
            throw new IsuExtraServiceException("Student does't exists");
        }

        var studentsGroup = selectedStudent.Group as TimetabledGroup;

        var builder = new TimetableBuilder();
        Timetable timetable = builder.AddLessonsFromTimetable(selectedStudent.JgofsGroup?.Stream.Timetable)
            .AddLessonsFromTimetable(selectedStudent.JgofsGroup?.Timetable)
            .AddLessonsFromTimetable(studentsGroup?.Timetable)
            .AddLessonsFromTimetable(studentsGroup?.Timetable)
            .Build();

        return timetable;
    }

    public List<TimetabledStudent> GetJgofsGroupStudents(JgofsGroupName groupName)
    {
        JgofsGroup? selectedGroup = _jgofsGroupList.FirstOrDefault(x => x.GroupName.Equals(groupName));

        if (selectedGroup == null)
        {
            throw new IsuExtraServiceException("Group doesn't exists");
        }

        return selectedGroup.ListOfStudents.ToList();
    }

    public List<Stream<JgofsGroup>> GetAllJgofsStreams(JgofsName jgofsName)
    {
        var selectedStreams = _jgofsStreamsGroupList
            .Where(x => x.StreamName.Name == jgofsName.Name)
            .ToList();

        return selectedStreams;
    }

    public List<TimetabledStudent> GetStudentsInGroupWithOutJgofs(GroupName groupName)
    {
        TimetabledGroup? selectedGroup = FindGroup(groupName);

        if (selectedGroup == null)
        {
            throw new IsuExtraServiceException("Group doesn't exists");
        }

        return _studentList.Where(x => x.Group.Equals(selectedGroup) && (x.JgofsGroup == null)).ToList();
    }

    public void AddPractice(Practice practice)
    {
        Lesson? crossingLesson = _lessonsList.FirstOrDefault(x => (x.TimeInterval == practice.TimeInterval
                                                                  && ((x.Teacher == practice.Teacher)
                                                                      || (x.ClassroomNumber ==
                                                                          practice.ClassroomNumber))));
        if (crossingLesson != null)
        {
            throw new IsuExtraServiceException("Teacher or classroom is taken");
        }

        _lessonsList.Add(practice);
    }

    public void AddLecture(Lecture lecture)
    {
        Lesson? crossingLesson = _lessonsList.FirstOrDefault(x => (x.TimeInterval == lecture.TimeInterval
                                                                   && ((x.Teacher == lecture.Teacher)
                                                                       || (x.ClassroomNumber ==
                                                                           lecture.ClassroomNumber))));
        if (crossingLesson != null)
        {
            throw new IsuExtraServiceException("Teacher or classroom is taken");
        }

        _lessonsList.Add(lecture);
    }

    public void SetTimetableForGroup(GroupName groupName, List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>> lessons)
    {
        TimetabledGroup selectedGroup = GetGroup(groupName);

        Timetable groupTimetable = CreateTimetableForGroup(lessons);

        if ((selectedGroup.Stream.Timetable != null) && !selectedGroup.Stream.Timetable.LessonCrossingCheck(groupTimetable))
        {
            throw new IsuExtraServiceException("Group timetable can not cross with stream timetable");
        }

        selectedGroup.AddTimetable(groupTimetable);
    }

    public void SetTimetableForJgofsGroup(JgofsGroupName jgofsGroupName, List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>> lessons)
    {
        JgofsGroup selectedGroup = GetJgofsGroup(jgofsGroupName);

        Timetable jgofsGroupTimetable = CreateTimetableForGroup(lessons);

        if ((selectedGroup.Stream.Timetable != null) && !selectedGroup.Stream.Timetable.LessonCrossingCheck(jgofsGroupTimetable))
        {
            throw new IsuExtraServiceException("JgofsGroup timetable can not cross with jgofs stream timetable");
        }

        selectedGroup.AddTimetable(jgofsGroupTimetable);
    }

    public void SetTimetableForGroupStream(StreamName streamName,  List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>> lessons)
    {
        if (streamName.GroupsType != StreamName.GorupsTypeId.General)
        {
            throw new IsuExtraServiceException("Wrong stream name");
        }

        Stream<TimetabledGroup> selectedStream = GetGroupStream(streamName);

        AddStreamTimetable(selectedStream, lessons);
    }

    public void SetTimetableForJgofsStream(StreamName streamName, List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>> lessons)
    {
        if (streamName.GroupsType != StreamName.GorupsTypeId.Jgofs)
        {
            throw new IsuExtraServiceException("Wrong stream name");
        }

        Stream<JgofsGroup> selectedStream = GetJgofsStream(streamName);

        AddStreamTimetable(selectedStream, lessons);
    }

    public void AddJgofs(JgofsName jgofsName, string facultyName, uint streamsNumber, uint groupsNumber)
    {
        Faculty faculty = GetFaculty(facultyName);

        faculty.AddJgofs(jgofsName);

        for (int i = 1; i <= streamsNumber; ++i)
        {
            var stream = new Stream<JgofsGroup>(new StreamName(jgofsName.Name, i));
            _jgofsStreamsGroupList.Add(stream);
            for (int j = 1; j <= groupsNumber; j++)
            {
                stream.AddGroup(AddJgofsGroup(new JgofsGroupName(string.Concat(jgofsName.ToString(), ' ', i, '.', j), faculty), stream));
            }
        }
    }

    public JgofsGroup EnrollStudentToJgofs(TimetabledStudent student, string facultyName, JgofsName jgofsName)
    {
        if (!StudentExists(student))
        {
            throw new IsuExtraServiceException("Student doesn't exists");
        }

        if (!JgofsExists(jgofsName))
        {
            throw new IsuExtraServiceException("Jgofs doesn't exists");
        }

        Faculty faculty = GetFaculty(facultyName);

        if (!faculty.JgofsNames.Contains(jgofsName))
        {
            throw new IsuExtraServiceException("Wrong faculty or jgofsName");
        }

        if (faculty.GeneralGroupsFacultyIds.Contains(student.Group.GroupName.Faculty))
        {
            throw new IsuExtraServiceException("Student can't enroll to jgofs of his faculty");
        }

        var studentGroup = student.Group as TimetabledGroup;

        if (studentGroup == null)
        {
            throw new IsuExtraServiceException("Student doesn't have a group");
        }

        if (studentGroup.Timetable == null)
        {
            throw new IsuExtraServiceException("Group doesn't have actual Timetable");
        }

        var selectedGroups = _jgofsGroupList.Where(x => x.GroupName.JgofsName.Equals(jgofsName)).ToList();

        JgofsGroup? suitableJgofsGroup = selectedGroups.FirstOrDefault(x => x.Timetable != null && x.Stream.Timetable != null
                                                                            && x.Timetable.LessonCrossingCheck(studentGroup.Timetable)
                                                                            && x.Stream.Timetable.LessonCrossingCheck(studentGroup.Timetable)
                                                                            && x.Vacancy > 0);

        if (suitableJgofsGroup == null)
        {
            throw new IsuExtraServiceException("There is no suitable group for you");
        }

        suitableJgofsGroup.AddStudent(student);
        student.AddJgofsGroup(suitableJgofsGroup);

        return suitableJgofsGroup;
    }

    public void RemoveStudentFromJgofsGroup(TimetabledStudent student)
    {
        if (!StudentExists(student))
        {
            throw new IsuExtraServiceException("Student doesn't exists");
        }

        if (student.JgofsGroup == null)
        {
            throw new IsuExtraServiceException("Student doesn't have any jgofs");
        }

        student.JgofsGroup.RemoveStudent(student);

        student.RemoveJgofsGroup();
    }

    private bool GroupExists(GroupName groupName) => _groupList.Exists(x => x.GroupName.Equals(groupName));
    private TimetabledGroup? FindGroup(GroupName groupName) => _groupList.FirstOrDefault(x => x.GroupName.Equals(groupName));
    private bool JgofsGroupExists(JgofsGroupName jgofsGroupName) => _jgofsGroupList.Exists(x => x.GroupName.Equals(jgofsGroupName));
    private JgofsGroup? FindJgofsGroup(JgofsGroupName jgofsGroupName) => _jgofsGroupList.FirstOrDefault(x => x.GroupName.Equals(jgofsGroupName));
    private Stream<TimetabledGroup>? FindTimetabledGroupStream(StreamName streamName) =>
        _groupStreamList.FirstOrDefault(x => x.StreamName.Equals(streamName));
    private Stream<JgofsGroup>? FindJgofsGroupStream(StreamName streamName) =>
        _jgofsStreamsGroupList.FirstOrDefault(x => x.StreamName.Equals(streamName));
    private Faculty? FindFaculty(string facultyName) => _facultiesList.FirstOrDefault(x => x.Name == facultyName);
    private bool StudentExists(TimetabledStudent student) => _studentList.Contains(student);
    private TimetabledStudent? FindStudent(uint id) => _studentList.FirstOrDefault(x => x.Id == id);
    private bool JgofsExists(JgofsName jgofsName) => _facultiesList.Exists(x => x.JgofsNames.Contains(jgofsName));
    private void SetDefaultFaculties()
    {
        var faculty = new Faculty("ТИНТ");
        faculty.AddGeneralGroupsFacultyId('M');
        _facultiesList.Add(faculty);

        faculty = new Faculty("КТУ");
        faculty.AddGeneralGroupsFacultyId('K');
        _facultiesList.Add(faculty);
    }

    private JgofsGroup AddJgofsGroup(JgofsGroupName jgofsGroupName, Stream<JgofsGroup> stream)
    {
        if (JgofsGroupExists(jgofsGroupName))
        {
            throw new IsuExtraServiceException("JgofsGroup with this name already exists");
        }

        var jgofsGroup = new JgofsGroup(jgofsGroupName, stream);
        _jgofsGroupList.Add(jgofsGroup);

        return jgofsGroup;
    }

    private Timetable CreateTimetableForGroup(List<Tuple<Timetable.Day, Timetable.Week, List<Practice>>> lessons)
    {
        var builder = new TimetableBuilder();

        foreach (var dayLessons in lessons)
        {
            builder.AddDayPractices(dayLessons.Item1, dayLessons.Item2, dayLessons.Item3);

            foreach (Practice practice in dayLessons.Item3)
            {
                AddPractice(practice);
            }
        }

        return builder.Build();
    }

    private void AddStreamTimetable<T>(Stream<T> stream, List<Tuple<Timetable.Day, Timetable.Week, List<Lecture>>> lessons)
        where T : ITimetabled
    {
        var builder = new TimetableBuilder();

        foreach (var dayLessons in lessons)
        {
            builder.AddDayLectures(dayLessons.Item1, dayLessons.Item2, dayLessons.Item3);

            foreach (Lecture lecture in dayLessons.Item3)
            {
                AddLecture(lecture);
            }
        }

        Timetable groupStreamTimetable = builder.Build();

        if (stream.ListOfGroups.Exists(x => (x.Timetable != null)
                                            && !x.Timetable.LessonCrossingCheck(groupStreamTimetable)))
        {
            throw new IsuExtraServiceException("Stream timetable can not cross with group timetable");
        }

        stream.AddTimetable(groupStreamTimetable);
    }
}