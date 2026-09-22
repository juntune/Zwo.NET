using System.Xml.Serialization;

namespace ZwoDotNET;

[XmlRoot(ElementName = "workout_file")]
public class WorkoutFile
{
    [XmlElement(ElementName = "author")]
    public required string Author { get; set; }

    [XmlElement(ElementName = "category")]
    public required string Category { get; set; }

    [XmlElement(ElementName = "name")]
    public required string Name { get; set; }

    [XmlElement(ElementName = "description")]
    public required object Description { get; set; }

    // [XmlElement(ElementName = "tags")]
    // public object Tags { get; set; }

    [XmlElement(ElementName = "workout")]
    public required Workout Workout { get; set; }
}

[XmlInclude(typeof(Warmup))]
[XmlInclude(typeof(SteadyState))]
[XmlInclude(typeof(FreeRide))]
[XmlInclude(typeof(Cooldown))]
public abstract class WorkoutSegment
{
    [XmlAttribute(AttributeName = "Duration")]
    public int Duration { get; set; }
}

[XmlRoot(ElementName = "Warmup")]
public class Warmup : WorkoutSegment
{
    [XmlAttribute(AttributeName = "PowerLow")]
    public double PowerLow { get; set; }

    [XmlAttribute(AttributeName = "PowerHigh")]
    public double PowerHigh { get; set; }
}

[XmlRoot(ElementName = "SteadyState")]
public class SteadyState : WorkoutSegment
{
    [XmlAttribute(AttributeName = "Power")]
    public double Power { get; set; }
}

[XmlRoot(ElementName = "FreeRide")]
public class FreeRide : WorkoutSegment
{
    [XmlAttribute(AttributeName = "FlatRoad")]
    public int FlatRoad { get; set; }
}

[XmlRoot(ElementName = "Cooldown")]
public class Cooldown : WorkoutSegment
{
    [XmlAttribute(AttributeName = "PowerLow")]
    public double PowerLow { get; set; }

    [XmlAttribute(AttributeName = "PowerHigh")]
    public double PowerHigh { get; set; }
}

[XmlRoot(ElementName = "workout")]
public class Workout
{
    [XmlElement("Warmup", typeof(Warmup))]
    [XmlElement("SteadyState", typeof(SteadyState))]
    [XmlElement("FreeRide", typeof(FreeRide))]
    [XmlElement("Cooldown", typeof(Cooldown))]
    public List<WorkoutSegment> Segments { get; set; } = new();
}