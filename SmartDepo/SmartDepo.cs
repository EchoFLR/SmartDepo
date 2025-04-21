using System.Reflection;
using Json.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
using SmartDepoLib.Toolbelt;

namespace SmartDepoLib;

/// <summary>
/// Represents a smart depot for managing trams and their missions.
/// </summary>
public class SmartDepo
{
    #region Creation
    private readonly TramQueue _tramQueue;
    private readonly ILogger<SmartDepo> _logger;
    /// <summary>
    /// Event triggered when a mission is assigned to a tram.
    /// </summary>
    public event Func<object, MissionAssignedEventArgs, Task>? MissionAssigned;

    private SmartDepo(TramQueue tramQueue, ILogger<SmartDepo> logger)
    {
        _tramQueue = tramQueue;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SmartDepo"/> class from the provided JSON string.
    /// </summary>
    /// <param name="tramJsonString">
    /// A JSON string containing tram data. The JSON must conform to the embedded schema.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="SmartDepo"/> class initialized with the tram data.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the JSON validation fails or if deserialization is unsuccessful.
    /// </exception>
    public static SmartDepo CreateDepo(string tramJsonString)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<SmartDepo>();

        logger.LogInformation("Creating SmartDepo instance from provided JSON.");
        var schemaJson = LoadEmbeddedSchema("SmartDepo.Schemas.tram_schema.json");
        logger.LogDebug("Loaded embedded schema.");

        var schema = JsonSchema.FromText(schemaJson);

        var jsonNode = JsonNode.Parse(tramJsonString)
            ?? throw new InvalidOperationException("Failed to parse JSON string.");

        var validationResults = schema.Evaluate(jsonNode);
        if (!validationResults.IsValid)
        {
            var errors = validationResults.Errors.Select(e => e.Value).ToList();
            throw new InvalidOperationException($"Invalid JSON format. Validation failed. Errors: {string.Join(", ", errors)}");
        }
        logger.LogInformation("JSON validation succeeded.");

        var trams = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Tram>>(tramJsonString)
                    ?? throw new InvalidOperationException("Failed to deserialize JSON.");
        logger.LogInformation("Deserialized {Count} trams.", trams.Count());

        var tramQueue = TramQueue.CreateQueue(trams);
        logger.LogInformation("SmartDepo instance created successfully.");
        return new SmartDepo(tramQueue, logger);
    }

    private static string LoadEmbeddedSchema(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
    #endregion

    #region Interaction
    /// <summary>
    /// Retrieves all trams currently in the depot.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Tram"/> objects representing the trams in the depot.
    /// </returns>
    public IEnumerable<Tram> GetTrams()
    {
        _logger.LogInformation("Retrieving trams from the depot.");
        return _tramQueue.GetTrams();
    }

    /// <summary>
    /// Assigns a mission to a tram and raises the MissionAssigned event.
    /// </summary>
    /// <param name="mission">The mission to assign.</param>
    public async Task AssignMissionAsync(string mission)
    {
        await _tramQueue.AssignMission(mission, async tram =>
        {
            if (tram != null)
            {
                _logger.LogInformation("Mission '{Mission}' assigned to Tram '{TramId}'.", mission, tram.Id);

                if (MissionAssigned != null)
                {
                    await MissionAssigned.Invoke(this, new MissionAssignedEventArgs(tram, mission));
                }
            }
            else
            {
                _logger.LogWarning("No available trams to assign the mission '{Mission}'.", mission);
            }
        });
    }

    /// <summary>
    /// Resets the depot by clearing all trams.
    /// </summary>
    public async Task ResetAsync()
    {
        _logger.LogInformation("Resetting the SmartDepo...");

        
        await Task.Run(async () =>
        {
            await _tramQueue.ClearAsync();
            _logger.LogInformation("Tram queue cleared.");
        });

        _logger.LogInformation("SmartDepo reset completed.");
    }
    #endregion
}
