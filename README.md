1| # Zwo.NET
2| 
3| A .NET library for programmatically creating Zwift workout files (.zwo format). Build structured cycling workouts with different segment types including warmups, steady states, free rides, and cool[...]
4| 
5| [![Latest build](https://github.com/juntune/Zwo.NET/actions/workflows/build.yml/badge.svg)](https://github.com/juntune/Zwo.NET/actions/workflows/build.yml)
6| 
7| ## Features
8| 
9| - 🚴 **Easy Workout Creation**: Build Zwift workouts using a fluent, chainable API
10| - 📊 **Multiple Segment Types**: Support for Warmup, SteadyState, FreeRide, and Cooldown segments
11| - 💾 **XML Export**: Serialize workouts to standard Zwift .zwo XML, either in memory or directly to a file
12| - 🔧 **Strongly Typed**: Full type safety with C# classes for all workout components
13| - ⚡ **Modern C#**: Built on .NET 10.0 with nullable reference types and implicit usings
14| 
15| ## Installation
16| 
17| Add the Zwo.NET project to your solution
18| 
19| Nuget maybe coming later..
20| 
21| ## Quick Start
22| 
23| ```csharp
24| using ZwoDotNET;
25| 
26| // Create a new workout
27| var workout = new Zwo(
28|     Author: "Your Name",
29|     Category: "Intervals",
29|     Name: "Beginner Intervals",
29|     Description: "A beginner-friendly interval workout"
29| );
30| 
31| // Add a warmup segment (2 minutes, ramping from 30% to 50% power)
32| workout.AddWorkoutSegment(new Warmup
33| {
34|     Duration = 120,
35|     PowerLow = 0.3,
36|     PowerHigh = 0.5
37| });
38| 
39| // Add a steady state segment (5 minutes at 75% power)
40| workout.AddWorkoutSegment(new SteadyState
41| {
42|     Duration = 300,
43|     Power = 0.75
44| });
45| 
46| // Add a free ride segment (3 minutes)
47| workout.AddWorkoutSegment(new FreeRide
48| {
49|     Duration = 180,
50|     FlatRoad = 1
51| });
52| 
53| // Add a cooldown segment (2 minutes, ramping from 40% to 20% power)
54| workout.AddWorkoutSegment(new Cooldown
55| {
56|     Duration = 120,
57|     PowerLow = 0.4,
58|     PowerHigh = 0.2
59| });
60| 
61| // Get the ZWO XML without creating a file
62| var zwoXml = workout.ToXmlString();
63| 
64| // Or save the workout to a .zwo file
65| workout.SaveToFile("my_workout.zwo");
66| ```
67| 
68| ## Workout Segments
69| 
70| ### Warmup
71| A ramping segment that gradually increases power from one level to another.
72| 
73| ```csharp
74| new Warmup
75| {
76|     Duration = 120,        // Duration in seconds
77|     PowerLow = 0.3,        // Starting power (0.0 - 7.0)
78|     PowerHigh = 0.5        // Ending power (0.0 - 7.0)
79| }
80| ```
81| 
82| ### SteadyState
83| A constant power segment at a fixed intensity level.
84| 
85| ```csharp
86| new SteadyState
87| {
88|     Duration = 300,        // Duration in seconds
89|     Power = 1.2            // Power level (0.0 - 7.0; 1.2 is 120% of FTP)
90| }
91| ```
92| 
93| ### FreeRide
94| A free riding segment where the rider controls their own power.
95| 
96| ```csharp
97| new FreeRide
98| {
99|     Duration = 180,        // Duration in seconds
100|     FlatRoad = 1           // Road type (typically 1 for flat)
101| }
102| ```
103| 
104| ### Cooldown
105| A ramping segment that gradually decreases power.
106| 
107| ```csharp
108| new Cooldown
109| {
110|     Duration = 120,        // Duration in seconds
111|     PowerLow = 0.4,        // Starting power (0.0 - 7.0)
112|     PowerHigh = 0.2        // Ending power (0.0 - 7.0)
113| }
114| ```
115| 
116| ## API Reference
117| 
118| ### Zwo Class
119| 
120| #### Constructor
121| ```csharp
122| public Zwo(string author, string category, string name, string description)
123| ```
124| Creates a new workout with metadata.
125| 
126| #### Methods
127| 
128| **AddWorkoutSegment**
129| ```csharp
130| public Zwo AddWorkoutSegment(WorkoutSegment segment)
131| ```
132| Adds a workout segment to the workout. Returns `this` for method chaining.
133| 
134| **SaveToFile**
135| ```csharp
136| public void SaveToFile(string fileName)
137| ```
138| Serializes the workout to an XML file in Zwift .zwo format. Throws `ArgumentException` if fileName is invalid or `InvalidOperationException` if the save operation fails.
139| 
140| **ToXmlString**
141| ```csharp
142| public string ToXmlString()
| 143| ```
144| Serializes the workout to a ZWO XML string without writing a file. The XML declaration specifies UTF-8. `SaveToFile()` uses the same serializer, so both APIs produce the same workout document.
145| 
146| ```csharp
147| var zwoXml = workout.ToXmlString();
148| ```
149| 
150| ## Example: Complete Workout
151| 
152| ```csharp
153| var workout = new Zwo(
154|     author: "Cycling Coach",
155|     category: "Endurance",
156|     name: "Long Endurance Ride",
| 157|     description: "Build aerobic capacity with a long, steady ride"
158| );
159| 
160| workout
161|     .AddWorkoutSegment(new Warmup { Duration = 300, PowerLow = 0.4, PowerHigh = 0.6 })
162|     .AddWorkoutSegment(new SteadyState { Duration = 1800, Power = 0.65 })
163|     .AddWorkoutSegment(new SteadyState { Duration = 1200, Power = 0.7 })
164|     .AddWorkoutSegment(new Cooldown { Duration = 300, PowerLow = 0.5, PowerHigh = 0.3 });
165| 
166| workout.SaveToFile("endurance_ride.zwo");
167| ```
168| 
169| ## Output Format
170| 
171| Workouts are serialized as standard Zwift .zwo XML documents that can be written to a file with `SaveToFile()` and imported directly into Zwift, or returned in memory with `ToXmlString()` and sha[...]
172| 
173| ```xml
174| <?xml version="1.0" encoding="utf-8"?>
175| <workout_file>
176|   <author>Cycling Coach</author>
177|   <category>Endurance</category>
178|   <name>Long Endurance Ride</name>
179|   <description>Build aerobic capacity with a long, steady ride</description>
180|   <workout>
181|     <Warmup Duration="300" PowerLow="0.4" PowerHigh="0.6" />
182|     <SteadyState Duration="1800" Power="0.65" />
| 183|     <SteadyState Duration="1200" Power="0.7" />
184|     <Cooldown Duration="300" PowerLow="0.5" PowerHigh="0.3" />
185|   </workout>
186| </workout_file>
187| ```
188| 
189| ## Importing Workouts into Zwift
190| 
191| Once you've generated your .zwo files using Zwo.NET, you can import them into Zwift on your computer. Here's how:
192| 
193| ### On Mac
194| 
195| 1. Copy your .zwo file
196| 2. Open **Finder**
197| 3. Navigate to **Documents** > **Zwift** > **Workouts**
198| 4. Open your **Zwift ID folder** (numeric folder)
199| 5. Paste the .zwo file into the folder
200| 6. Restart Zwift to see your imported workout
201| 
202| ### On PC
203| 
204| 1. Copy your .zwo file
205| 2. Locate the **Z** icon in the notification area of the taskbar (select **˄** if needed to show hidden icons)
206| 3. Right-click the **Z** icon and select **Open Logs Folder**
207| 4. In the file path, select **Zwift**
208| 5. Select **Workouts**
209| 6. Open your **Zwift ID folder** (numeric folder)
210| 7. Paste the .zwo file into the folder
211| 8. Restart Zwift to see your imported workout
212| 
213| ### Important Notes
214| 
215| - **File Size Limit**: Files larger than 64 KB are only available on the computer used for import
216| - **Sync**: Imported workouts become available on all devices after restarting Zwift
217| - **Naming**: Avoid using special characters in your workout filenames to ensure smooth syncing
218| - **Finding Your Zwift ID**: See [Locating Your Zwift ID](https://support.zwift.com/en_us/locating-your-zwift-id-H1WiyxS_I) on the Zwift support site
219| 
220| ### Sharing Workouts
221| 
222| To share your generated workouts with other Zwifters, simply send them the .zwo file. They can place it in their Zwift ID folder using the same process above.
223| 
224| For more information, visit [Zwift's Custom Workouts Support Page](https://support.zwift.com/en_us/custom-workouts-ryGOTVEPs).
225| 
226| ## Requirements
227| 
228| - .NET 10.0 or later
229| - C# 12 or later (for nullable reference types)
230| 
231| ## License
232| 
233| This project is licensed under the [MIT License](LICENSE).
234| 
235| ## Contributing
236| 
237| Contributions are welcome! Please feel free to submit a pull request or open an issue.
238| 








































































































































































































































































































































































































































































