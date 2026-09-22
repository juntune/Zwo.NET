using System.Xml.Serialization;

namespace ZwoDotNET;

public class Zwo
{
    public WorkoutFile Content { get; set; }
    public Zwo(string author, string category, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be null or empty", nameof(author));
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty", nameof(category));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty", nameof(description));

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
        if (segment == null)
            throw new ArgumentNullException(nameof(segment), "Workout segment cannot be null");
        Content.Workout.Segments.Add(segment);
        return this;
    }

    public void SaveToFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));

        var serializer = new XmlSerializer(typeof(WorkoutFile));
        var tempFileName = Path.GetTempFileName();

        try
        {
            using (var writer = new StreamWriter(tempFileName))
            {
                serializer.Serialize(writer, Content);
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.Move(tempFileName, fileName, overwrite: true);
        }
        catch (Exception ex)
        {
            if (File.Exists(tempFileName))
            {
                try
                {
                    File.Delete(tempFileName);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
            throw new InvalidOperationException($"Failed to save workout to file '{fileName}': {ex.Message}", ex);
        }
    }
}

