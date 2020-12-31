using UnityEngine;

namespace RedBlueGames.Rumble
    {
    /// <summary>
    /// A source for Rumble. This handles positional and lifetime information for a RumbleSource
    /// </summary>
    public class RumbleSource : MonoBehaviour
    {
        [Tooltip("The Info used to define the attributes of this Rumble.")]
        // TODO: [EmbeddedInspector]
        [SerializeField]
        private RumbleInfo info;

        private float timeElapsed;

        public bool IsAlive => !IsDead;

        public bool IsDead => timeElapsed >= Info.Lifetime;

        /// <summary>
        /// Gets or sets the Info used to define the attributes of this Rumble.
        /// </summary>
        /// <value>The Info.</value>
        public RumbleInfo Info => this.info;

        public void Initialize(RumbleInfo info)
        {
            this.timeElapsed = 0.0f;
            this.info = info;
        }

        public void Tick(float deltaTime)
        {
            this.timeElapsed += deltaTime;
        }

        /// <summary>
        /// Evaluates the values of the rumble for a given listener position.
        /// </summary>
        /// <returns>The rumble.</returns>
        /// <param name="listenerPosition">Listener position.</param>
        public Rumble EvaluateRumble(Vector3 listenerPosition)
        {
            // Assume zero when the parent or self is inactive
            if (!this.gameObject.activeInHierarchy)
            {
                return Rumble.Zero;
            }

            // When disabled, assume zero rumble
            if (!this.enabled)
            {
                return Rumble.Zero;
            }

            // If no info is yet assigned, assume zero rumble
            if (this.Info == null)
            {
                return Rumble.Zero;
            }
            
            return RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(
                this.Info,
                (listenerPosition - this.transform.position).sqrMagnitude,
                this.timeElapsed);
        }

        private Rumble GetRumbleIntensityCenteredOnSource()
        {
            return this.EvaluateRumble(transform.position);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "RumbleIcon.png", true);

            if (this.GetRumbleIntensityCenteredOnSource() == Rumble.Zero)
            {
                return;
            }

            var intensityEstimate = this.GetRumbleIntensityCenteredOnSource().ForceFeedback.LeftMotor;
            float alpha = Mathf.Lerp(0.05f, 0.1f, intensityEstimate);
            UnityEditor.Handles.color = new Color(1.0f, 1.0f, 0.0f, alpha);
            float sphereRadius = this.Info != null ? this.Info.Radius : 0.0f;
            UnityEditor.Handles.DrawSolidDisc(this.transform.position, Vector3.forward, sphereRadius);
        }
#endif

        /* Structs, Sub-Classes =================================================================================================== */
    }
}