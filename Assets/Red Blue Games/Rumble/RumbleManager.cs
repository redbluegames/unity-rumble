using System.Collections.Generic;
using RedBlueGames.Tools;
using UnityEngine;
    
namespace RedBlueGames.Rumble
{
    /// <summary>
    /// Rumble manager is responsible for tracking outstanding rumbles, resolving their combined effect on each
    /// listener, and applying it.
    /// </summary>
    public class RumbleManager : Singleton<RumbleManager>
    {
        private List<RumbleSource> activeRumbleSources;

        /// <summary>
        /// Gets the active rumble sources.
        /// </summary>
        /// <value>The active rumble sources.</value>
        public IReadOnlyList<RumbleSource> ActiveRumbleSources
        {
            get
            {
                // Clean up any Rumbles that have destroyed
                this.activeRumbleSources.RemoveNullEntries();

                return this.activeRumbleSources;
            }
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
            RegisterRumbleSource(rumble);
            rumble.transform.position = position;

            return rumble;
        }

        public void DestroyRumble(RumbleSource source)
        {
            Destroy(source.gameObject);
            this.UnregisterRumbleSource(source);
        }

        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            this.activeRumbleSources = new List<RumbleSource>();
        }

        /// <summary>
        /// Unity's Update message. Updates every frame.
        /// </summary>
        protected void Update()
        {
            this.TickAndKillExpiredSources();
        }

        private RumbleSource SpawnRumble(RumbleInfo rumbleInfo)
        {
            var rumbleObject = new GameObject(string.Concat("[Rumble] ", rumbleInfo.name));
            var rumble = rumbleObject.AddComponent<RumbleSource>();
            rumble.Initialize(rumbleInfo);

            return rumble;
        }

        private void RegisterRumbleSource(RumbleSource rumble)
        {
            this.activeRumbleSources.Add(rumble);
        }

        private void UnregisterRumbleSource(RumbleSource rumble)
        {
            this.activeRumbleSources.Remove(rumble);
        }

        private void TickAndKillExpiredSources()
        {
            for (var i = this.activeRumbleSources.Count - 1; i >= 0; --i)
            {
                var rumbleSource = this.activeRumbleSources[i];
                rumbleSource.Tick(Time.deltaTime);

                if (rumbleSource.IsDead)
                {
                    DestroyRumble(rumbleSource);
                }
            }
        }

        /* Structs, Sub-Classes =================================================================================================== */
    }
}