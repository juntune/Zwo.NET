# Zwo.NET

A .NET library for programmatically creating Zwift workout files (.zwo format). Build structured cycling workouts with different segment types including warmups, steady states, free rides, and cooldowns.

[![Latest build](https://github.com/juntune/Zwo.NET/actions/workflows/build.yml/badge.svg)](https://github.com/juntune/Zwo.NET/actions/workflows/build.yml)

## Features

- 🚴 Easy workout creation with a fluent, chainable API
- 📊 Support for Warmup, SteadyState, FreeRide, and Cooldown segments
- 💾 XML export to standard Zwift .zwo files or an in-memory XML string
- 🔧 Strongly typed C# models for workout metadata and segments
- ⚡ Built for .NET 10 with nullable reference types enabled

## Installation

Add the project to your solution:

```bash
git clone https://github.com/juntune/Zwo.NET.git
```

Then reference the project in your .NET app, or add it to your solution via Visual Studio / `dotnet sln add`.

NuGet packaging may be added in the future.

## Quick Start

```csharp
using ZwoDotNET;

var workout = new Zwo(
    author: "Your Name",
    category: "Intervals",
    name: "Beginner Intervals",
    description: "A beginner-friendly interval workout"
);

workout.AddWorkoutSegment(new Warmup
{
    Duration = 120,
    PowerLow = 0.3,
    PowerHigh = 0.5
});

workout.AddWorkoutSegment(new SteadyState
{
    Duration = 300,
    Power = 0.75
});

workout.AddWorkoutSegment(new FreeRide
{
    Duration = 180,
    FlatRoad = 1
});

workout.AddWorkoutSegment(new Cooldown
{
    Duration = 120,
    PowerLow = 0.4,
    PowerHigh = 0.2
});

var zwoXml = workout.ToXmlString();
workout.SaveToFile("my_workout.zwo");
```

The generated XML looks like this:

```xml
<?xml version="1.0" encoding="utf-8"?>
<workout_file>
  <author>Your Name</author>
  <category>Intervals</category>
  <name>Beginner Intervals</name>
  <description>A beginner-friendly interval workout</description>
  <workout>
    <Warmup Duration="120" PowerLow="0.3" PowerHigh="0.5" />
    <SteadyState Duration="300" Power="0.75" />
    <FreeRide Duration="180" FlatRoad="1" />
    <Cooldown Duration="120" PowerLow="0.4" PowerHigh="0.2" />
  </workout>
</workout_file>
```

## Supported Workout Segments

### Warmup

A ramping segment that increases power over time.

```csharp
new Warmup
{
    Duration = 120,
    PowerLow = 0.3,
    PowerHigh = 0.5
};
```

### SteadyState

A fixed-power segment.

```csharp
new SteadyState
{
    Duration = 300,
    Power = 1.2
};
```

### FreeRide

A free-riding segment, commonly used for controlled endurance efforts.

```csharp
new FreeRide
{
    Duration = 180,
    FlatRoad = 1
};
```

### Cooldown

A ramping segment that decreases power over time.

```csharp
new Cooldown
{
    Duration = 120,
    PowerLow = 0.4,
    PowerHigh = 0.2
};
```

## API Overview

### Zwo

```csharp
public Zwo(string author, string category, string name, string description)
```

Creates a workout with metadata.

#### Methods

```csharp
public Zwo AddWorkoutSegment(WorkoutSegment segment)
```
Adds a workout segment and returns the same instance for fluent chaining.

```csharp
public string ToXmlString()
```
Serializes the workout to a ZWO XML string.

```csharp
public void SaveToFile(string fileName)
```
Writes the workout to a `.zwo` file.

## Example: Full Workout

```csharp
var workout = new Zwo(
    author: "Cycling Coach",
    category: "Endurance",
    name: "Long Endurance Ride",
    description: "Build aerobic capacity with a long, steady ride"
);

workout
    .AddWorkoutSegment(new Warmup { Duration = 300, PowerLow = 0.4, PowerHigh = 0.6 })
    .AddWorkoutSegment(new SteadyState { Duration = 1800, Power = 0.65 })
    .AddWorkoutSegment(new SteadyState { Duration = 1200, Power = 0.7 })
    .AddWorkoutSegment(new Cooldown { Duration = 300, PowerLow = 0.5, PowerHigh = 0.3 });

workout.SaveToFile("endurance_ride.zwo");
```

## Importing Workouts into Zwift

Once your `.zwo` file has been generated, you can import it into Zwift on your computer.

### On Mac

1. Copy your `.zwo` file.
2. Open Finder.
3. Navigate to Documents > Zwift > Workouts.
4. Open your Zwift ID folder.
5. Paste the file into the folder.
6. Restart Zwift.

### On PC

1. Copy your `.zwo` file.
2. Open the Zwift icon in the taskbar notification area.
3. Open the Logs folder.
4. Navigate to Zwift > Workouts.
5. Open your Zwift ID folder.
6. Paste the file into the folder.
7. Restart Zwift.

### Notes

- Imported workouts sync to all devices after restarting Zwift.
- Avoid special characters in workout filenames.

## Requirements

- .NET 10.0 or later
- C# 12 or later

## License

This project is licensed under the [MIT License](LICENSE).

## Contributing

Contributions are welcome. Please feel free to submit a pull request or open an issue.
