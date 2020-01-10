// TODO: Namespace this
using System.Collections.Generic;
using RedBlueGames.Tools;
using UnityEngine;

/// <summary>
/// Rumble manager is responsible for tracking outstanding rumbles, resolving their combined effect on each
/// listener, and applying it.
/// </summary>
public class RumbleManager : Singleton<RumbleManager>
{
    /* Consts, Fields ========================================================================================================= */

    private List<RumbleSource> activeRumbleSources;

    /* Enums ================================================================================================================== */

    /* Properties ============================================================================================================= */

    /// <summary>
    /// Gets the active rumble sources.
    /// </summary>
    /// <value>The active rumble sources.</value>
    public List<RumbleSource> ActiveRumbleSources
    {
        get
        {
            // Clean up any Rumbles that have destroyed
            this.activeRumbleSources.RemoveNullEntries();

            return this.activeRumbleSources;
        }
    }

    /* Methods ================================================================================================================ */

    /// <summary>
    /// Registers a RumbleSource to the tracked rumbles
    /// </summary>
    /// <param name="rumble">Rumble to register.</param>
    public void RegisterRumbleSource(RumbleSource rumble)
    {
        this.activeRumbleSources.Add(rumble);
    }

    /// <summary>
    /// Starts an instance of rumble at the given position.
    /// </summary>
    /// <returns>The spawned rumble.</returns>
    /// <param name="position">Position to spawn the rumble object.</param>
    /// <param name="rumbleInfo">Rumble info.</param>
    public RumbleSource StartRumble(Vector3 position, RumbleInfo rumbleInfo)
    {
        var rumble = this.SpawnRumble(rumbleInfo);
        rumble.transform.position = position;

        return rumble;
    }

    protected override void Awake()
    {
        base.Awake();

        this.activeRumbleSources = new List<RumbleSource>();
    }

    private RumbleSource SpawnRumble(RumbleInfo rumbleInfo)
    {
        var rumbleObject = new GameObject(string.Concat("[Rumble] ", rumbleInfo.name));
        var rumble = rumbleObject.AddComponent<RumbleSource>();
        rumble.Info = rumbleInfo;

        return rumble;
    }

    /* Structs, Sub-Classes =================================================================================================== */
}