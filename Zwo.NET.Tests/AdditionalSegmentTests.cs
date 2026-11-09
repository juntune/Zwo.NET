using System.Xml.Linq;
using ZwoDotNET;
using Workout = ZwoDotNET.Zwo;

namespace Zwo.NET.Tests;

[TestClass]
public sealed class AdditionalSegmentTests
{
    [TestMethod]
    public void IntervalsTSerializesRepeatAttributesWithoutDuration()
    {
        var workout = new Workout("Coach", "Intervals", "VO2", "5 x 3 min")
            .AddWorkoutSegment(new IntervalsT
            {
                Repeat = 5,
                OnDuration = 180,
                OffDuration = 120,
                OnPower = 1.1,
                OffPower = 0.5,
                Cadence = 95,
                CadenceResting = 85
            });

        var intervals = Segments(workout).Single();

        Assert.AreEqual("IntervalsT", intervals.Name.LocalName);
        Assert.IsNull(intervals.Attribute("Duration"));
        Assert.AreEqual("5", intervals.Attribute("Repeat")?.Value);
        Assert.AreEqual("180", intervals.Attribute("OnDuration")?.Value);
        Assert.AreEqual("120", intervals.Attribute("OffDuration")?.Value);
        Assert.AreEqual("1.1", intervals.Attribute("OnPower")?.Value);
        Assert.AreEqual("0.5", intervals.Attribute("OffPower")?.Value);
        Assert.AreEqual("95", intervals.Attribute("Cadence")?.Value);
        Assert.AreEqual("85", intervals.Attribute("CadenceResting")?.Value);
    }

    [TestMethod]
    public void RampSerializesPowerRange()
    {
        var workout = new Workout("Coach", "Intervals", "Ramp", "Ramp up")
            .AddWorkoutSegment(new Ramp { Duration = 600, PowerLow = 0.6, PowerHigh = 0.9 });

        var ramp = Segments(workout).Single();

        Assert.AreEqual("Ramp", ramp.Name.LocalName);
        Assert.AreEqual("600", ramp.Attribute("Duration")?.Value);
        Assert.AreEqual("0.6", ramp.Attribute("PowerLow")?.Value);
        Assert.AreEqual("0.9", ramp.Attribute("PowerHigh")?.Value);
    }

    [TestMethod]
    public void CadenceAndTextEventsAreOnlyWrittenWhenSet()
    {
        var withExtras = new SteadyState { Duration = 300, Power = 0.8, Cadence = 90 };
        withExtras.TextEvents.Add(new TextEvent { TimeOffset = 10, Message = "Settle in" });
        var plain = new SteadyState { Duration = 300, Power = 0.8 };

        var workout = new Workout("Coach", "Endurance", "Tempo", "Steady")
            .AddWorkoutSegment(withExtras)
            .AddWorkoutSegment(plain);

        var segments = Segments(workout);

        Assert.AreEqual("90", segments[0].Attribute("Cadence")?.Value);
        var textEvent = segments[0].Element("textevent");
        Assert.IsNotNull(textEvent);
        Assert.AreEqual("10", textEvent.Attribute("timeoffset")?.Value);
        Assert.AreEqual("Settle in", textEvent.Attribute("message")?.Value);

        Assert.IsNull(segments[1].Attribute("Cadence"));
        Assert.IsFalse(segments[1].HasElements);
    }

    [TestMethod]
    public void TotalDurationIncludesIntervalRepeats()
    {
        var workout = new Workout("Coach", "Intervals", "Mixed", "Mixed")
            .AddWorkoutSegment(new Warmup { Duration = 300, PowerLow = 0.4, PowerHigh = 0.7 })
            .AddWorkoutSegment(new IntervalsT { Repeat = 4, OnDuration = 60, OffDuration = 30, OnPower = 1.2, OffPower = 0.5 })
            .AddWorkoutSegment(new Cooldown { Duration = 240, PowerLow = 0.6, PowerHigh = 0.4 });

        Assert.AreEqual(900m, workout.Content.Workout.TotalDuration);
    }

    [TestMethod]
    public void NewSegmentPropertiesRejectInvalidValues()
    {
        var intervals = new IntervalsT();
        var ramp = new Ramp();
        var steady = new SteadyState();
        var textEvent = new TextEvent();

        Assert.Throws<ArgumentException>(() => intervals.Repeat = 0);
        Assert.Throws<ArgumentException>(() => intervals.OnDuration = 0);
        Assert.Throws<ArgumentException>(() => intervals.OffDuration = -1);
        Assert.Throws<ArgumentException>(() => intervals.OnPower = 7.01);
        Assert.Throws<ArgumentException>(() => intervals.OffPower = double.NaN);
        Assert.Throws<ArgumentException>(() => intervals.CadenceResting = 0);
        Assert.Throws<ArgumentException>(() => ramp.PowerLow = -0.1);
        Assert.Throws<ArgumentException>(() => ramp.PowerHigh = double.PositiveInfinity);
        Assert.Throws<ArgumentException>(() => steady.Cadence = 201);
        Assert.Throws<ArgumentException>(() => textEvent.TimeOffset = -1);
        Assert.Throws<ArgumentException>(() => textEvent.Message = " ");
    }

    private static XElement[] Segments(Workout workout)
    {
        var root = XDocument.Parse(workout.ToXmlString()).Root;
        Assert.IsNotNull(root);
        var workoutElement = root.Element("workout");
        Assert.IsNotNull(workoutElement);
        return workoutElement.Elements().ToArray();
    }
}
