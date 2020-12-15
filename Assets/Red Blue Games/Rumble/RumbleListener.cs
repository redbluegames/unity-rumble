namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// Rumble listeners handle applying force feedback and screen shake for rumble, based on listener position.
    /// </summary>
    public class RumbleListener : MonoBehaviour
    {
        // Ignore warnings about lastAppliedRumble not being used. It's just for debug inspection.
#pragma warning disable 0414
        private Rumble lastAppliedRumble;
        private Rumble lastResultantRumble;
#pragma warning restore 0414

        private IForceFeedbackResponder forceFeedbackResponder;

        private IScreenShakeResponder screenShakeResponder;

        public float ScreenShakeMultiplier { get; set; }
        public float ForceFeedbackMultiplier { get; set; }

        /// <summary>
        /// Gets the position of the listener.
        /// </summary>
        /// <value>The position.</value>
        private Vector3 Position => this.transform.position;

        /// <summary>
        /// Create an instance of a RumbleListener, which applies rumble to the specified player
        /// </summary>
        /// <param name="listeningPlayer">Listening player - the player to receive the rumble.</param>
        /// <returns>The created RumbleListener</returns>
        public static RumbleListener Create(IForceFeedbackResponder forceFeedbackResponder, IScreenShakeResponder screenShakeResponder)//Player listeningPlayer)
        {
            var listenerGameObject = new GameObject("[RumbleListener]");
            var listener = listenerGameObject.AddComponent<RumbleListener>();
            listener.ForceFeedbackMultiplier = 1.0f;
            listener.ScreenShakeMultiplier = 1.0f;

            listener.forceFeedbackResponder = forceFeedbackResponder;
            listener.screenShakeResponder = screenShakeResponder;

            return listener;
        }

        protected void Update()
        {
            // Ideally this is applied after player input / movement, but before camera.
            this.ApplyAggregatedRumbleThisFrame();
        }

        private void ApplyAggregatedRumbleThisFrame()
        {
            var aggregatedRumble = this.GetAggregateRumbleThisFrame();

            // Apply multipliers for preferences
            aggregatedRumble.ForceFeedback *= this.ForceFeedbackMultiplier;
            aggregatedRumble.ScreenShake *= this.ScreenShakeMultiplier;

            this.ApplyRumble(aggregatedRumble);
        }

        private Rumble GetAggregateRumbleThisFrame()
        {
            var aggregatedRumble = Rumble.Zero;
            var rumbleSources = RumbleManager.Instance.ActiveRumbleSources;
            foreach (var rumbleSource in rumbleSources)
            {
                aggregatedRumble = aggregatedRumble + rumbleSource.EvaluateRumble(this.Position);
            }

            return aggregatedRumble;
        }

        private void ApplyRumble(Rumble rumble, float rumbleIntensity = 1.0f, float screenshakeIntensity = 1.0f)
        {
            // Assign last applied rumble for debugging.
            this.lastAppliedRumble = rumble;

            // Modify the supplied Rumble based on whether the game is paused
            {
                /* TODO: Convert pausing force feedback to work outside of Sparklite
                if (TimeManager.Instance.IsPaused)
                {
                    // Zero-out the vibration if the game is paused so that it doesn't vibrate forever.
                    // NOTE: Not modifying screenshake so that the camera doesn't pop to Zero when pausing/unpausing.

                    // TODO: If we want some vibrations to play while paused (e.g. a confirmation
                    // vibration when changing the Setting in Game Options), then we can create a new
                    // 'PlayWhilePaused' field on RumbleInfo, but then we'd have to implement/change
                    // Rumble to operate off of real-time instead of game-time... or else that rumble
                    // will still go forever when the game is paused...
                    rumble.ForceFeedback = ForceFeedbackIntensities.Zero;
                } */
            }

            this.screenShakeResponder.ApplyScreenShake(rumble.ScreenShake);
            this.forceFeedbackResponder.SetVibration(rumble.ForceFeedback);

            this.lastResultantRumble = rumble;
        }
    }
}