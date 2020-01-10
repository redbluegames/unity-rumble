namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// Rumble listeners handle applying force feedback and screen shake for rumble, based on listener position.
    /// </summary>
    public class RumbleListener : MonoBehaviour
    {
        /* Consts, Fields ========================================================================================================= */
        private const float MinimumScreenShakeCooldown = 0.15f;

        // private Player listeningPlayer;

        private float lastRumbleTimeStamp;
        private float currentShakeCooldown;

        // Ignore warnings about lastAppliedRumble not being used. It's just for debug inspection.
#pragma warning disable 0414
        private Rumble lastAppliedRumble;
        private Rumble lastResultantRumble;
#pragma warning restore 0414

        /* Enums ================================================================================================================== */

        /* Properties ============================================================================================================= */

        /// <summary>
        /// Gets the position of the listener.
        /// </summary>
        /// <value>The position.</value>
        public Vector3 Position => this.transform.position;

        public float ScreenShakeMultiplier { get; set; }
        public float ForceFeedbackMultiplier { get; set; }

        /* Methods ================================================================================================================ */

        /// <summary>
        /// Create an instance of a RumbleListener, which applies rumble to the specified player
        /// </summary>
        /// <param name="listeningPlayer">Listening player - the player to receive the rumble.</param>
        /// <returns>The created RumbleListener</returns>
        public static RumbleListener Create()//Player listeningPlayer)
        {
            var listenerGameObject = new GameObject("[RumbleListener]");
            var listener = listenerGameObject.AddComponent<RumbleListener>();
            //listener.ListeningPlayer = listeningPlayer;

            return listener;
        }

        /// <summary>
        /// Unity's Update message
        /// </summary>
        protected void Update()
        {
            var rumble = this.GetAggregateRumbleThisFrame();

            //var rumbleIntensity = Player.Primary.User.Profile.GetFloat(Keys.VibrationAnalog, Defaults.VibrationAnalog);
            //var screenShakeIntensity = Player.Primary.User.Profile.GetFloat(Keys.ScreenshakeAnalog, Defaults.ScreenshakeAnalog);
            this.ApplyRumble(rumble, this.ForceFeedbackMultiplier, this.ScreenShakeMultiplier);
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

            // Modify the supplied Rumble based on User preferences (use Primary Player b/c Companion player doesn't have a User)
            {
                rumble.ForceFeedback = rumble.ForceFeedback * rumbleIntensity;
                rumble.ScreenShake = rumble.ScreenShake * screenshakeIntensity;
            }

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

            // Apply Force Feedback
            {
                /* TODO: Apply Force Feedback agnostic of input tech stack
                var allJoysticksOfPlayer = this.ListeningPlayer.Input.controllers.Joysticks;
                var joystickToVibrate = this.ListeningPlayer.Input.controllers.GetLastActiveController(Rewired.ControllerType.Joystick);

                // Note: foreach iteration of their collection allocates memory. Avoid with a normal for loop.
                for (var i = 0; i < allJoysticksOfPlayer.Count; ++i)
                {
                    var joystick = allJoysticksOfPlayer[i];
                    if (joystick.supportsVibration)
                    {
                        if (rumble.ForceFeedback == ForceFeedbackIntensities.Zero || joystick != joystickToVibrate)
                        {
                            joystick.StopVibration();
                        }
                        else
                        {
                            joystick.SetVibration(rumble.ForceFeedback.LeftMotor, rumble.ForceFeedback.RightMotor);
                        }
                    }
                }*/
            }

            // Apply ScreenShake
            {
                var isScreenShakeCooldownComplete = Time.time >= this.lastRumbleTimeStamp + this.currentShakeCooldown;
                if (rumble.ScreenShake != ScreenShakeIntensities.Zero && isScreenShakeCooldownComplete)
                {
                    /* TODO: Apply ScreenShake agnostic of camera tech
                    // Randomize delay between shakes to make the shake seem less like a pattern
                    this.currentShakeCooldown = MinimumScreenShakeCooldown * Random.Range(1.0f, 1.5f);

                    // Extend shake duration because shake does a dampened lerp between positions, making it pretty
                    // obvious when each shake occurs. This helps cut into that a bit, at the expense of 
                    // creating additional vibrations (cause you get a shake on a shake)
                    var shakeDuration = this.currentShakeCooldown * Random.Range(1.0f, 1.0f);
                    Com.LuisPedroFonseca.ProCamera2D.ProCamera2DShake.Instance.Shake(
                        shakeDuration,
                        rumble.ScreenShake.Strength,
                        rumble.ScreenShake.Vibrato,
                        smoothness: 0.0f);

                    this.lastRumbleTimeStamp = Time.time;
                    */
                }
            }

            this.lastResultantRumble = rumble;
        }

        /* Structs, Sub-Classes =================================================================================================== */
    }
}