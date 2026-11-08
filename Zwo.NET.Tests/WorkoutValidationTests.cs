using ZwoDotNET;
using Workout = ZwoDotNET.Zwo;

namespace Zwo.NET.Tests;

[TestClass]
public sealed class WorkoutValidationTests
{
    [TestMethod]
    [DataRow("author", null)]
    [DataRow("author", "")]
    [DataRow("author", " ")]
    [DataRow("category", null)]
    [DataRow("category", "")]
    [DataRow("category", " ")]
    [DataRow("name", null)]
    [DataRow("name", "")]
    [DataRow("name", " ")]
    [DataRow("description", null)]
    [DataRow("description", "")]
    [DataRow("description", " ")]
    public void ConstructorRejectsNullOrWhitespaceMetadata(string parameterName, string? invalidValue)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => CreateWorkoutWithInvalidMetadata(parameterName, invalidValue));

        Assert.AreEqual(parameterName, exception.ParamName);
    }

    [TestMethod]
    public void SegmentsPreserveFractionalDuration()
    {
        var segment = new SteadyState { Duration = 0.25m };

        Assert.AreEqual(0.25m, segment.Duration);
    }

    [TestMethod]
    public void AddWorkoutSegmentRejectsNull()
    {
        var workout = new Workout("Coach", "Intervals", "Tempo", "Steady effort");

        Assert.Throws<ArgumentNullException>(() => workout.AddWorkoutSegment(null!));
    }

    [TestMethod]
    public void SegmentsRequirePositiveDuration()
    {
        (string Name, WorkoutSegment Segment)[] segments =
        [
            (nameof(Warmup), new Warmup()),
            (nameof(SteadyState), new SteadyState()),
            (nameof(FreeRide), new FreeRide()),
            (nameof(Cooldown), new Cooldown())
        ];

        foreach (var (name, segment) in segments)
        {
            foreach (var duration in new[] { 0, -1 })
            {
                Assert.Throws<ArgumentException>(
                    () => segment.Duration = duration,
                    $"{name} should reject duration {duration}.");
            }

            segment.Duration = 1;
            Assert.AreEqual(1m, segment.Duration);
        }
    }

    [TestMethod]
    public void PowerPropertiesRejectNonFiniteAndOutOfRangeValues()
    {
        var warmup = new Warmup();
        var steadyState = new SteadyState();
        var cooldown = new Cooldown();
        (string Name, Action<double> SetValue)[] powerProperties =
        [
            ("Warmup.PowerLow", value => warmup.PowerLow = value),
            ("Warmup.PowerHigh", value => warmup.PowerHigh = value),
            ("SteadyState.Power", value => steadyState.Power = value),
            ("SteadyState.PowerLow", value => steadyState.PowerLow = value),
            ("SteadyState.PowerHigh", value => steadyState.PowerHigh = value),
            ("Cooldown.PowerLow", value => cooldown.PowerLow = value),
            ("Cooldown.PowerHigh", value => cooldown.PowerHigh = value)
        ];
        var invalidValues = new[]
        {
            double.NaN,
            double.PositiveInfinity,
            double.NegativeInfinity,
            -0.01,
            7.01
        };

        foreach (var (name, setValue) in powerProperties)
        {
            foreach (var value in invalidValues)
            {
                Assert.Throws<ArgumentException>(
                    () => setValue(value),
                    $"{name} should reject power {value}.");
            }

            setValue(0);
            setValue(7);
        }
    }

    [TestMethod]
    public void FlatRoadMustBeNonNegative()
    {
        var freeRide = new FreeRide();

        Assert.Throws<ArgumentException>(() => freeRide.FlatRoad = -1);
        freeRide.FlatRoad = 0;
        Assert.AreEqual(0, freeRide.FlatRoad);
    }

    private static Workout CreateWorkoutWithInvalidMetadata(string parameterName, string? invalidValue)
    {
        return parameterName switch
        {
            "author" => new Workout(invalidValue!, "Intervals", "Tempo", "Steady effort"),
            "category" => new Workout("Coach", invalidValue!, "Tempo", "Steady effort"),
            "name" => new Workout("Coach", "Intervals", invalidValue!, "Steady effort"),
            "description" => new Workout("Coach", "Intervals", "Tempo", invalidValue!),
            _ => throw new ArgumentOutOfRangeException(nameof(parameterName))
        };
    }
}
