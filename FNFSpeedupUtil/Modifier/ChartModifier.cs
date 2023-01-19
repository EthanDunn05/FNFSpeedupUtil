﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.Modifier;

/// <summary>
/// Used for editing charts.
/// TODO: Replace with using JSON object serialization
/// </summary>
public class ChartModifier
{
    /// <summary>
    /// Path to the chart file
    /// </summary>
    private string ChartPath { get; }

    /// <summary>
    /// The Json contents of the chart
    /// </summary>
    public JObject Chart { get; }

    public ChartModifier(string chartPath)
    {
        ChartPath = chartPath;
        
        var chartFileContents = File.ReadAllText(ChartPath);
        Chart = JObject.Parse(chartFileContents);
    }

    /// <summary>
    /// Sets the scroll speed of a song.
    /// </summary>
    /// <param name="scrollSpeed">The new scroll speed</param>
    public void SetScrollSpeed(double scrollSpeed)
    {
        Console.WriteLine($"Modifying chart: {Path.GetFileName(ChartPath)}");
        var song = Chart["song"]!;
        
        // Set the new scroll speed
        TryModifyJsonObjectProperty(song, "speed", speed => 
            (JValue) scrollSpeed);
        
        // Write the chart!
        File.WriteAllText(ChartPath, Chart.ToString(Formatting.Indented));
    }

    /// <summary>
    /// Modifies the speed of the chart by the given multiplier
    /// </summary>
    /// <param name="multiplier">The value to multiply the speed by</param>
    public void ModifySpeed(double multiplier)
    {
        Console.WriteLine($"Modifying chart: {Path.GetFileName(ChartPath)}");
        var song = Chart["song"]!;

        // Modify song bpm
        TryModifyJsonObjectProperty(song, "bpm", bpm =>
            (JValue) ((double) bpm * multiplier));

        var currentBpm = (double) song["bpm"]!;

        // Read the sections
        var sections = song["notes"]!;
        foreach (var section in sections)
        {
            ModifySectionSpeed(section, multiplier, currentBpm);
        }

        // Modify the events
        var events = song["events"];
        if (events != null)
        {
            foreach (var eventData in (JArray) events)
            {
                ModifyEvent((JArray) eventData, multiplier);
            }
        }

        // Write the chart!
        File.WriteAllText(ChartPath, Chart.ToString(Formatting.Indented));
    }
    
    private static void ModifyEvent(JContainer eventData, double multiplier)
    {
        var time = (double) eventData[0];
        eventData[0] = time / multiplier;
    }

    private static void ModifySectionSpeed(JToken section, double multiplier, double currentBpm)
    {
        // Modify section bpm
        TryModifyJsonObjectProperty(section, "bpm", bpm =>
        {
            var newBpm = (double) bpm * multiplier;
            currentBpm = newBpm;
            return (JValue) newBpm;
        });


        // Every section has notes even if it's empty
        var notes = (JArray) section["sectionNotes"]!;
        foreach (var noteToken in notes)
        {
            var note = (JArray) noteToken;
            ModifyNoteSpeed(note, multiplier, currentBpm);
        }
    }

    private static void ModifyNoteSpeed(JContainer note, double multiplier, double currentBpm)
    {
        const int timeIndex = 0;
        const int posIndex = 1;
        const int sustainIndex = 2;

        // Event notes are on strum -1
        var isEvent = (int) note[posIndex]! == -1;

        // Modify note time
        TryModifyJsonArrayElement(note, timeIndex, strumTime
            => (JValue) ((double) strumTime / multiplier));

        // Modify sustains
        // Events can't be sustains so they use index 2 for event data
        if (!isEvent)
            TryModifyJsonArrayElement(note, sustainIndex, holdTime =>
            {
                // This process is to keep the sustains quantized
                // Floor is used because ending early is better than late
                var stepLength = 60 / currentBpm * 1000 / 4;

                var holdBeats = (int) ((int) holdTime / stepLength);
                var newHoldBeats = Math.Floor(holdBeats / multiplier);

                var extraTime = (double) holdTime % stepLength / multiplier;
                var newHoldTime = (int) (newHoldBeats * stepLength + extraTime);
                return (JValue) newHoldTime;
            });
    }

    private static void TryModifyJsonObjectProperty(JToken parent, string childName, Func<JValue, JValue> modification)
    {
        try
        {
            var child = (JValue) parent[childName]!;
            parent[childName] = modification(child);
        }
        catch (Exception e)
        {
            if (!(e.GetType() == typeof(ArgumentNullException)))
            {
                Console.WriteLine($"WARNING: Failed to modify property [{childName}] ({parent[childName]}) of: {parent.Path}");
#if DEBUG
                Console.WriteLine(e);
#endif
            }
        }
    }

    private static void TryModifyJsonArrayElement(JContainer parent, int index, Func<JValue, JValue> modification)
    {
        try
        {
            var child = (JValue) parent[index]!;
            parent[index] = modification(child);
        }
        catch (Exception e)
        {
            if (!(e.GetType() == typeof(ArgumentNullException)))
            {
                Console.WriteLine($"WARNING: Failed to modify element [{index}] ({parent[index]}) of: {parent.Path}");
#if DEBUG
                Console.WriteLine(e);
#endif
            }
        }
    }
}