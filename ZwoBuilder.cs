using System.Xml.Serialization;

namespace ZwoDotNET;

public class Zwo
{
    public WorkoutFile Content { get; set; }
    public Zwo(string author, string category, string name, string description)
    {
        Content = new WorkoutFile
        {
            Author = author,
            Category = category,
            Name = name,
            Description = description,
            Workout = new Workout()
        };
    }

    public Zwo AddWorkoutSegment(WorkoutSegment segment)
    {
        Content.Workout.Segments.Add(segment);
        return this;
    }

    public bool SaveToFile(string fileName)
    {
        var serializer = new XmlSerializer(typeof(WorkoutFile));

        File.Delete(fileName);

        using (var writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, Content);
        }
        return true;
    }
}

