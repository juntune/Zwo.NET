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
[XmlInclude(typeof(Ramp))]
[XmlInclude(typeof(IntervalsT))]
public abstract class WorkoutSegment
{
    private int _cadence;
    private bool _cadenceSpecified;

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

    /// <summary>
    /// Controls whether the Duration attribute is written. Segments that derive their
    /// length from other attributes (such as <see cref="IntervalsT"/>) override this.
    /// </summary>
    public virtual bool ShouldSerializeDuration() => true;

    /// <summary>
    /// Total length of the segment in seconds, including any repeats.
    /// </summary>
    [XmlIgnore]
    public virtual decimal TotalDuration => Duration;

    /// <summary>
    /// Optional target cadence in RPM. Only written when explicitly set.
    /// </summary>
    [XmlAttribute(AttributeName = "Cadence")]
    public int Cadence
    {
        get => _cadence;
        set
        {
            SegmentValidation.ValidateCadence(value, nameof(Cadence));
            _cadence = value;
            _cadenceSpecified = true;
        }
    }

    public bool ShouldSerializeCadence() => _cadenceSpecified;

    /// <summary>
    /// Optional on-screen messages shown during the segment.
    /// </summary>
    [XmlElement(ElementName = "textevent")]
    public List<TextEvent> TextEvents { get; set; } = new();
}

public class TextEvent
{
    private int _timeOffset;
    private string _message = string.Empty;

    /// <summary>
    /// Seconds from the start of the segment at which the message is shown.
    /// </summary>
    [XmlAttribute(AttributeName = "timeoffset")]
    public int TimeOffset
    {
        get => _timeOffset;
        set
        {
            if (value < 0)
                throw new ArgumentException("TimeOffset must be non-negative");
            _timeOffset = value;
        }
    }

    [XmlAttribute(AttributeName = "message")]
    public string Message
    {
        get => _message;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Message cannot be null or empty");
            _message = value;
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

[XmlRoot(ElementName = "Ramp")]
public class Ramp : WorkoutSegment
{
    private double _powerLow;
    private double _powerHigh;

    [XmlAttribute(AttributeName = "PowerLow")]
    public double PowerLow
    {
        get => _powerLow;
        set
        {
            SegmentValidation.ValidatePower(value, nameof(PowerLow));
            _powerLow = value;
        }
    }

    [XmlAttribute(AttributeName = "PowerHigh")]
    public double PowerHigh
    {
        get => _powerHigh;
        set
        {
            SegmentValidation.ValidatePower(value, nameof(PowerHigh));
            _powerHigh = value;
        }
    }
}

/// <summary>
/// Repeated on/off intervals, e.g. 5 x (3 min @ 110% / 2 min @ 50%).
/// The segment length is derived from Repeat, OnDuration and OffDuration,
/// so no Duration attribute is written.
/// </summary>
[XmlRoot(ElementName = "IntervalsT")]
public class IntervalsT : WorkoutSegment
{
    private int _repeat;
    private decimal _onDuration;
    private decimal _offDuration;
    private double _onPower;
    private double _offPower;
    private int _cadenceResting;
    private bool _cadenceRestingSpecified;

    [XmlAttribute(AttributeName = "Repeat")]
    public int Repeat
    {
        get => _repeat;
        set
        {
            if (value < 1)
                throw new ArgumentException("Repeat must be at least 1");
            _repeat = value;
        }
    }

    [XmlAttribute(AttributeName = "OnDuration")]
    public decimal OnDuration
    {
        get => _onDuration;
        set
        {
            if (value <= 0)
                throw new ArgumentException("OnDuration must be greater than 0 seconds");
            _onDuration = value;
        }
    }

    [XmlAttribute(AttributeName = "OffDuration")]
    public decimal OffDuration
    {
        get => _offDuration;
        set
        {
            if (value <= 0)
                throw new ArgumentException("OffDuration must be greater than 0 seconds");
            _offDuration = value;
        }
    }

    [XmlAttribute(AttributeName = "OnPower")]
    public double OnPower
    {
        get => _onPower;
        set
        {
            SegmentValidation.ValidatePower(value, nameof(OnPower));
            _onPower = value;
        }
    }

    [XmlAttribute(AttributeName = "OffPower")]
    public double OffPower
    {
        get => _offPower;
        set
        {
            SegmentValidation.ValidatePower(value, nameof(OffPower));
            _offPower = value;
        }
    }

    /// <summary>
    /// Optional cadence in RPM for the off (recovery) part of each repeat.
    /// </summary>
    [XmlAttribute(AttributeName = "CadenceResting")]
    public int CadenceResting
    {
        get => _cadenceResting;
        set
        {
            SegmentValidation.ValidateCadence(value, nameof(CadenceResting));
            _cadenceResting = value;
            _cadenceRestingSpecified = true;
        }
    }

    public bool ShouldSerializeCadenceResting() => _cadenceRestingSpecified;

    public override bool ShouldSerializeDuration() => false;

    [XmlIgnore]
    public override decimal TotalDuration => Repeat * (OnDuration + OffDuration);
}

[XmlRoot(ElementName = "workout")]
public class Workout
{
    [XmlElement("Warmup", typeof(Warmup))]
    [XmlElement("SteadyState", typeof(SteadyState))]
    [XmlElement("FreeRide", typeof(FreeRide))]
    [XmlElement("Cooldown", typeof(Cooldown))]
    [XmlElement("Ramp", typeof(Ramp))]
    [XmlElement("IntervalsT", typeof(IntervalsT))]
    public List<WorkoutSegment> Segments { get; set; } = new();

    /// <summary>
    /// Total workout length in seconds, including interval repeats.
    /// </summary>
    [XmlIgnore]
    public decimal TotalDuration => Segments.Sum(segment => segment.TotalDuration);
}

internal static class SegmentValidation
{
    public const double MaxPower = 7.0;
    public const int MaxCadence = 200;

    public static void ValidatePower(double value, string propertyName)
    {
        if (!double.IsFinite(value) || value < 0 || value > MaxPower)
            throw new ArgumentException($"{propertyName} must be between 0.0 and 7.0");
    }

    public static void ValidateCadence(int value, string propertyName)
    {
        if (value < 1 || value > MaxCadence)
            throw new ArgumentException($"{propertyName} must be between 1 and {MaxCadence} rpm");
    }
}