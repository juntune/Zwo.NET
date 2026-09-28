using System.Globalization;
using System.Text;
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

    public string ToXmlString()
    {
        using var writer = new Utf8StringWriter();
        Serialize(writer);
        return writer.ToString();
    }

    public void SaveToFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));
        if (!string.Equals(Path.GetExtension(fileName), ".zwo", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("FileName must have a .zwo extension", nameof(fileName));

        var tempFileName = Path.GetTempFileName();

        try
        {
            using (var writer = new StreamWriter(tempFileName))
                Serialize(writer);

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

    private void Serialize(TextWriter writer)
    {
        var serializer = new XmlSerializer(typeof(WorkoutFile));
        serializer.Serialize(writer, Content);
    }

    private sealed class Utf8StringWriter : StringWriter
    {
        public Utf8StringWriter() : base(CultureInfo.InvariantCulture) { }

        public override Encoding Encoding => new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    }
}
