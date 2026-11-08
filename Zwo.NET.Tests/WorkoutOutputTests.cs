using System.Globalization;
using System.Xml.Linq;
using ZwoDotNET;
using Workout = ZwoDotNET.Zwo;

namespace Zwo.NET.Tests;

[TestClass]
public sealed class WorkoutOutputTests
{
    [TestMethod]
    public void ToXmlStringSerializesMetadataAndSegmentsInOrder()
    {
        const string name = "Ride & recover";
        const string description = "A steady workout";
        var workout = new Workout("Coach", "Intervals", name, description);

        var result = workout
            .AddWorkoutSegment(new Warmup { Duration = 120, PowerLow = 0.3, PowerHigh = 0.5 })
            .AddWorkoutSegment(new SteadyState { Duration = 300, Power = 0.75 })
            .AddWorkoutSegment(new FreeRide { Duration = 180, FlatRoad = 1 })
            .AddWorkoutSegment(new Cooldown { Duration = 120, PowerLow = 0.4, PowerHigh = 0.2 });

        Assert.AreSame(workout, result);

        var document = XDocument.Parse(workout.ToXmlString());
        var root = document.Root;
        Assert.IsNotNull(root);
        Assert.AreEqual("workout_file", root.Name.LocalName);
        Assert.AreEqual("utf-8", document.Declaration?.Encoding);
        CollectionAssert.AreEqual(
            new[] { "author", "category", "name", "description", "workout" },
            root.Elements().Select(element => element.Name.LocalName).ToArray());
        Assert.AreEqual("Coach", root.Element("author")?.Value);
        Assert.AreEqual("Intervals", root.Element("category")?.Value);
        Assert.AreEqual(name, root.Element("name")?.Value);
        Assert.AreEqual(description, root.Element("description")?.Value);

        var workoutXml = root.Element("workout");
        Assert.IsNotNull(workoutXml);
        var segments = workoutXml.Elements().ToArray();
        CollectionAssert.AreEqual(
            new[] { "Warmup", "SteadyState", "FreeRide", "Cooldown" },
            segments.Select(segment => segment.Name.LocalName).ToArray());

        var warmup = workoutXml.Element("Warmup");
        Assert.IsNotNull(warmup);
        Assert.AreEqual("120", warmup.Attribute("Duration")?.Value);
        Assert.AreEqual("0.3", warmup.Attribute("PowerLow")?.Value);
        Assert.AreEqual("0.5", warmup.Attribute("PowerHigh")?.Value);

        var steadyState = workoutXml.Element("SteadyState");
        Assert.IsNotNull(steadyState);
        Assert.AreEqual("300", steadyState.Attribute("Duration")?.Value);
        Assert.AreEqual("0.75", steadyState.Attribute("Power")?.Value);

        var freeRide = workoutXml.Element("FreeRide");
        Assert.IsNotNull(freeRide);
        Assert.AreEqual("180", freeRide.Attribute("Duration")?.Value);
        Assert.AreEqual("1", freeRide.Attribute("FlatRoad")?.Value);

        var cooldown = workoutXml.Element("Cooldown");
        Assert.IsNotNull(cooldown);
        Assert.AreEqual("120", cooldown.Attribute("Duration")?.Value);
        Assert.AreEqual("0.4", cooldown.Attribute("PowerLow")?.Value);
        Assert.AreEqual("0.2", cooldown.Attribute("PowerHigh")?.Value);
    }

    [TestMethod]
    public void ToXmlStringUsesInvariantDecimalFormatting()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");

            var workout = new Workout("Coach", "Intervals", "Tempo", "Steady effort")
                .AddWorkoutSegment(new SteadyState { Duration = 60, Power = 0.75 });

            var document = XDocument.Parse(workout.ToXmlString());
            Assert.AreEqual("0.75", document.Root?
                .Element("workout")?
                .Element("SteadyState")?
                .Attribute("Power")?.Value);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    [TestMethod]
    public void ToXmlStringPreservesTagsOptionalMetadataAndSteadyStatePowerRange()
    {
        var workout = new Workout("Coach", "Intervals", "Tempo", "Steady effort")
        {
            Content =
            {
                Author = null,
                Category = null,
                Tags = [new WorkoutTag { Name = "RECOVERY" }]
            }
        };
        workout.AddWorkoutSegment(new SteadyState
        {
            Duration = 0.25m,
            PowerLow = 0.55,
            PowerHigh = 0.75
        });

        var root = XDocument.Parse(workout.ToXmlString()).Root;
        Assert.IsNotNull(root);
        Assert.IsNull(root.Element("author"));
        Assert.IsNull(root.Element("category"));
        Assert.AreEqual("RECOVERY", root.Element("tags")?.Element("tag")?.Attribute("name")?.Value);

        var steadyState = root.Element("workout")?.Element("SteadyState");
        Assert.IsNotNull(steadyState);
        Assert.AreEqual("0.25", steadyState.Attribute("Duration")?.Value);
        Assert.AreEqual("0.55", steadyState.Attribute("PowerLow")?.Value);
        Assert.AreEqual("0.75", steadyState.Attribute("PowerHigh")?.Value);
        Assert.IsNull(steadyState.Attribute("Power"));
    }

    [TestMethod]
    public void SaveToFileWritesValidWorkoutXml()
    {
        var temporaryDirectory = Directory.CreateTempSubdirectory();
        try
        {
            var fileName = Path.Combine(temporaryDirectory.FullName, "workout.ZWO");
            var workout = new Workout("Coach", "Endurance", "Long ride", "An easy session")
                .AddWorkoutSegment(new SteadyState { Duration = 3600, Power = 0.65 });

            workout.SaveToFile(fileName);

            var document = XDocument.Load(fileName);
            Assert.AreEqual("Long ride", document.Root?.Element("name")?.Value);
            Assert.AreEqual("3600", document.Root?
                .Element("workout")?
                .Element("SteadyState")?
                .Attribute("Duration")?.Value);
            Assert.AreEqual("0.65", document.Root?
                .Element("workout")?
                .Element("SteadyState")?
                .Attribute("Power")?.Value);
        }
        finally
        {
            temporaryDirectory.Delete(recursive: true);
        }
    }

    [TestMethod]
    public void SaveToFileRejectsBlankNamesAndUnsupportedExtensions()
    {
        var workout = new Workout("Coach", "Intervals", "Tempo", "Steady effort");

        foreach (var invalidFileName in new string?[] { null, "", " ", "workout.xml" })
        {
            Assert.Throws<ArgumentException>(
                () => workout.SaveToFile(invalidFileName!),
                $"SaveToFile should reject '{invalidFileName}'.");
        }
    }
}
