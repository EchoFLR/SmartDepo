using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartDepoLib.Toolbelt;
internal class TramJsonConverter : JsonConverter<Tram>
{
    public override Tram Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        int id = 0;
        string name = string.Empty;
        string? currentMission = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString()!;
                reader.Read();

                switch (propertyName)
                {
                    case "Id":
                        id = reader.GetInt32();
                        break;
                    case "Name":
                        name = reader.GetString()!;
                        break;
                    case "CurrentMission":
                        currentMission = reader.GetString();
                        break;
                }
            }
        }

        return new Tram(id, name, currentMission ?? null);
    }

    public override void Write(Utf8JsonWriter writer, Tram value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Id", value.Id);
        writer.WriteString("Name", value.Name);
        writer.WriteString("CurrentMission", value.CurrentMission);
        writer.WriteEndObject();
    }
}

internal class TramIdComparer : IComparer<Tram>
{
    public int Compare(Tram? x, Tram? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        return x.Id.CompareTo(y.Id);
    }
}

/// <summary>
/// Represents the event data for a mission assignment event.
/// </summary>
public class MissionAssignedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the tram associated with the mission assignment.
    /// </summary>
    public Tram Tram { get; }

    /// <summary>
    /// Gets the mission assigned to the tram.
    /// </summary>
    public string Mission { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissionAssignedEventArgs"/> class
    /// with the specified tram and mission details.
    /// </summary>
    /// <param name="tram">The tram associated with the mission assignment.</param>
    /// <param name="mission">The mission assigned to the tram.</param>
    public MissionAssignedEventArgs(Tram tram, string mission)
    {
        Tram = tram;
        Mission = mission;
    }
}