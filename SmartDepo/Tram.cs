namespace SmartDepoLib;

/// <summary>
/// Represents a tram in the depot.
/// </summary>
public class Tram
{
    /// <summary>
    /// Gets the unique identifier of the tram.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the name of the tram.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the tram has an assigned mission.
    /// </summary>
    public bool HasMission { get; private set; }

    /// <summary>
    /// Gets the current mission assigned to the tram, if any.
    /// </summary>
    public string? CurrentMission { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tram"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the tram.</param>
    /// <param name="name">The name of the tram.</param>
    /// <param name="currentMission">The current mission assigned to the tram, if any. Pass <c>null</c> if no mission is assigned.</param>
    public Tram(int id, string name, string? currentMission = null)
    {
        Id = id;
        Name = name;
        HasMission = currentMission != null;
        CurrentMission = currentMission;
    }

    /// <summary>
    /// Assigns a mission to the tram.
    /// </summary>
    /// <param name="mission">The mission to assign.</param>
    public void AssignMission(string mission)
    {
        HasMission = true;
        CurrentMission = mission;
    }
}
