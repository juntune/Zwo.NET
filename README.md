# Zwo.NET

A .NET library for programmatically creating Zwift workout files (.zwo format). Build structured cycling workouts with different segment types including warmups, steady states, free rides, and cooldowns. All types might not be supported.

## Features

- 🚴 **Easy Workout Creation**: Build Zwift workouts using a fluent, chainable API
- 📊 **Multiple Segment Types**: Support for Warmup, SteadyState, FreeRide, and Cooldown segments
- 💾 **XML Export**: Automatically serialize workouts to standard Zwift .zwo XML format
- 🔧 **Strongly Typed**: Full type safety with C# classes for all workout components
- ⚡ **Modern C#**: Built on .NET 10.0 with nullable reference types and implicit usings

## Installation

Add the Zwo.NET project to your solution

Nuget maybe coming later..

## Quick Start

```csharp
using ZwoDotNET;

// Create a new workout
var workout = new Zwo(
    author: "Your Name",
    category: "Intervals",
    name: "Beginner Intervals",
    description: "A beginner-friendly interval workout"
);

// Add a warmup segment (2 minutes, ramping from 30% to 50% power)
workout.AddWorkoutSegment(new Warmup
{
    Duration = 120,
    PowerLow = 0.3,
    PowerHigh = 0.5
});

// Add a steady state segment (5 minutes at 75% power)
workout.AddWorkoutSegment(new SteadyState
{
    Duration = 300,
    Power = 0.75
});

// Add a free ride segment (3 minutes)
workout.AddWorkoutSegment(new FreeRide
{
    Duration = 180,
    FlatRoad = 1
});

// Add a cooldown segment (2 minutes, ramping from 40% to 20% power)
workout.AddWorkoutSegment(new Cooldown
{
    Duration = 120,
    PowerLow = 0.4,
    PowerHigh = 0.2
});

// Save to file
workout.SaveToFile("my_workout.zwo");
```

## Workout Segments

### Warmup
A ramping segment that gradually increases power from one level to another.

```csharp
new Warmup
{
    Duration = 120,        // Duration in seconds
    PowerLow = 0.3,        // Starting power (0.0 - 1.0)
    PowerHigh = 0.5        // Ending power (0.0 - 1.0)
}
```

### SteadyState
A constant power segment at a fixed intensity level.

```csharp
new SteadyState
{
    Duration = 300,        // Duration in seconds
    Power = 0.75           // Power level (0.0 - 1.0)
}
```

### FreeRide
A free riding segment where the rider controls their own power.

```csharp
new FreeRide
{
    Duration = 180,        // Duration in seconds
    FlatRoad = 1           // Road type (typically 1 for flat)
}
```

### Cooldown
A ramping segment that gradually decreases power.

```csharp
new Cooldown
{
    Duration = 120,        // Duration in seconds
    PowerLow = 0.4,        // Starting power (0.0 - 1.0)
    PowerHigh = 0.2        // Ending power (0.0 - 1.0)
}
```

## API Reference

### Zwo Class

#### Constructor
```csharp
public Zwo(string author, string category, string name, string description)
```
Creates a new workout with metadata.

#### Methods

**AddWorkoutSegment**
```csharp
public Zwo AddWorkoutSegment(WorkoutSegment segment)
```
Adds a workout segment to the workout. Returns `this` for method chaining.

**SaveToFile**
```csharp
public bool SaveToFile(string fileName)
```
Serializes the workout to an XML file in Zwift .zwo format.

## Example: Complete Workout

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

## Output Format

Workouts are saved as standard Zwift .zwo XML files that can be imported directly into Zwift or shared with other cyclists:

```xml
<?xml version="1.0" encoding="utf-8"?>
<workout_file>
  <author>Cycling Coach</author>
  <category>Endurance</category>
  <name>Long Endurance Ride</name>
  <description>Build aerobic capacity with a long, steady ride</description>
  <workout>
    <Warmup Duration="300" PowerLow="0.4" PowerHigh="0.6" />
    <SteadyState Duration="1800" Power="0.65" />
    <SteadyState Duration="1200" Power="0.7" />
    <Cooldown Duration="300" PowerLow="0.5" PowerHigh="0.3" />
  </workout>
</workout_file>
```

## Importing Workouts into Zwift

Once you've generated your .zwo files using Zwo.NET, you can import them into Zwift on your computer. Here's how:

### On Mac

1. Copy your .zwo file
2. Open **Finder**
3. Navigate to **Documents** > **Zwift** > **Workouts**
4. Open your **Zwift ID folder** (numeric folder)
5. Paste the .zwo file into the folder
6. Restart Zwift to see your imported workout

### On PC

1. Copy your .zwo file
2. Locate the **Z** icon in the notification area of the taskbar (select **˄** if needed to show hidden icons)
3. Right-click the **Z** icon and select **Open Logs Folder**
4. In the file path, select **Zwift**
5. Select **Workouts**
6. Open your **Zwift ID folder** (numeric folder)
7. Paste the .zwo file into the folder
8. Restart Zwift to see your imported workout

### Important Notes

- **File Size Limit**: Files larger than 64 KB are only available on the computer used for import
- **Sync**: Imported workouts become available on all devices after restarting Zwift
- **Naming**: Avoid using special characters in your workout filenames to ensure smooth syncing
- **Finding Your Zwift ID**: See [Locating Your Zwift ID](https://support.zwift.com/en_us/locating-your-zwift-id-H1WiyxS_I) on the Zwift support site

### Sharing Workouts

To share your generated workouts with other Zwifters, simply send them the .zwo file. They can place it in their Zwift ID folder using the same process above.

For more information, visit [Zwift's Custom Workouts Support Page](https://support.zwift.com/en_us/custom-workouts-ryGOTVEPs).

## Requirements

- .NET 10.0 or later
- C# 12 or later (for nullable reference types)

## License

MIT (See repository for more info)

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.
