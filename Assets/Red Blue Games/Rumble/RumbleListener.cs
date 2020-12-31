namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// RumbleIntensity listeners handle applying force feedback and screen shake for rumbleIntensity, based on listener position.
    /// </summary>
    public class RumbleListener : MonoBehaviour
    {
        // Ignore warnings about lastAppliedRumbleIntensity not being used. It's just for debug inspection.
#pragma warning disable 0414
        private RumbleIntensity lastAppliedRumbleIntensity;
        private RumbleIntensity lastResultantRumbleIntensity;
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
        /// Create an instance of a RumbleListener, which applies rumbleIntensity to the specified player
        /// </summary>
        /// <param name="listeningPlayer">Listening player - the player to receive the rumbleIntensity.</param>
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

        private RumbleIntensity GetAggregateRumbleThisFrame()
        {
            var aggregatedRumble = RumbleIntensity.Zero;
            var rumbleSources = RumbleManager.Instance.ActiveRumbleSources;
            foreach (var rumbleSource in rumbleSources)
            {
                aggregatedRumble = aggregatedRumble + rumbleSource.EvaluateRumble(this.Position);
            }

            return aggregatedRumble;
        }

        private void ApplyRumble(RumbleIntensity rumbleIntensity, float forceFeedbackIntensity = 1.0f, float screenshakeIntensity = 1.0f)
        {
            // Assign last applied rumbleIntensity for debugging.
            this.lastAppliedRumbleIntensity = rumbleIntensity;

            // Modify the supplied RumbleIntensity based on whether the game is paused
            {
                /* TODO: Convert pausing force feedback to work outside of Sparklite
                if (TimeManager.Instance.IsPaused)
                {
                    // Zero-out the vibration if the game is paused so that it doesn't vibrate forever.
                    // NOTE: Not modifying screenshake so that the camera doesn't pop to Zero when pausing/unpausing.

                    // TODO: If we want some vibrations to play while paused (e.g. a confirmation
                    // vibration when changing the Setting in Game Options), then we can create a new
                    // 'PlayWhilePaused' field on RumbleInfo, but then we'd have to implement/change
                    // RumbleIntensity to operate off of real-time instead of game-time... or else that rumbleIntensity
                    // will still go forever when the game is paused...
                    rumbleIntensity.ForceFeedback = ForceFeedbackIntensities.Zero;
                } */
            }

            this.screenShakeResponder.ApplyScreenShake(rumbleIntensity.ScreenShake);
            this.forceFeedbackResponder.SetVibration(rumbleIntensity.ForceFeedback);

            this.lastResultantRumbleIntensity = rumbleIntensity;
        }
    }
}