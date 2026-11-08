using System.Xml.Serialization;

namespace ZwoDotNET;

[XmlRoot(ElementName = "workout_file")]
public class WorkoutFile
{
    [XmlElement(ElementName = "author")]
    public string? Author { get; set; }

    [XmlElement(ElementName = "category")]
    public string? Category { get; set; }

    [XmlElement(ElementName = "name")]
    public required string Name { get; set; }

    [XmlElement(ElementName = "description")]
    public required string Description { get; set; }

    [XmlArray(ElementName = "tags")]
    [XmlArrayItem(ElementName = "tag")]
    public List<WorkoutTag>? Tags { get; set; }

    [XmlElement(ElementName = "workout")]
    public required Workout Workout { get; set; }
}

public class WorkoutTag
{
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; } = string.Empty;
}

[XmlInclude(typeof(Warmup))]
[XmlInclude(typeof(SteadyState))]
[XmlInclude(typeof(FreeRide))]
[XmlInclude(typeof(Cooldown))]
public abstract class WorkoutSegment
{
    private decimal _duration;

    [XmlAttribute(AttributeName = "Duration")]
    public decimal Duration
    {
        get => _duration;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Duration must be greater than 0 seconds");
            _duration = value;
        }
    }
}

[XmlRoot(ElementName = "Warmup")]
public class Warmup : WorkoutSegment
{
    private double _powerLow;
    private double _powerHigh;

    [XmlAttribute(AttributeName = "PowerLow")]
    public double PowerLow
    {
        get => _powerLow;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("PowerLow must be between 0.0 and 7.0");
            _powerLow = value;
        }
    }

    [XmlAttribute(AttributeName = "PowerHigh")]
    public double PowerHigh
    {
        get => _powerHigh;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("PowerHigh must be between 0.0 and 7.0");
            _powerHigh = value;
        }
    }
}

[XmlRoot(ElementName = "SteadyState")]
public class SteadyState : WorkoutSegment
{
    private double _power;
    private double _powerLow;
    private double _powerHigh;
    private bool _powerSpecified;
    private bool _powerLowSpecified;
    private bool _powerHighSpecified;

    [XmlAttribute(AttributeName = "Power")]
    public double Power
    {
        get => _power;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("Power must be between 0.0 and 7.0");
            _power = value;
            _powerSpecified = true;
        }
    }

    public bool ShouldSerializePower() => _powerSpecified;

    [XmlAttribute(AttributeName = "PowerLow")]
    public double PowerLow
    {
        get => _powerLow;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("PowerLow must be between 0.0 and 7.0");
            _powerLow = value;
            _powerLowSpecified = true;
        }
    }

    public bool ShouldSerializePowerLow() => _powerLowSpecified;

    [XmlAttribute(AttributeName = "PowerHigh")]
    public double PowerHigh
    {
        get => _powerHigh;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("PowerHigh must be between 0.0 and 7.0");
            _powerHigh = value;
            _powerHighSpecified = true;
        }
    }

    public bool ShouldSerializePowerHigh() => _powerHighSpecified;
}

[XmlRoot(ElementName = "FreeRide")]
public class FreeRide : WorkoutSegment
{
    private int _flatRoad;

    [XmlAttribute(AttributeName = "FlatRoad")]
    public int FlatRoad
    {
        get => _flatRoad;
        set
        {
            if (value < 0)
                throw new ArgumentException("FlatRoad must be non-negative");
            _flatRoad = value;
        }
    }
}

[XmlRoot(ElementName = "Cooldown")]
public class Cooldown : WorkoutSegment
{
    private double _powerLow;
    private double _powerHigh;

    [XmlAttribute(AttributeName = "PowerLow")]
    public double PowerLow
    {
        get => _powerLow;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("PowerLow must be between 0.0 and 7.0");
            _powerLow = value;
        }
    }

    [XmlAttribute(AttributeName = "PowerHigh")]
    public double PowerHigh
    {
        get => _powerHigh;
        set
        {
            if (!double.IsFinite(value) || value < 0 || value > 7.0)
                throw new ArgumentException("PowerHigh must be between 0.0 and 7.0");
            _powerHigh = value;
        }
    }
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