using UnityEngine;

namespace RedBlueGames.Rumble
    {
    /// <summary>
    /// A source for RumbleIntensity. This handles positional and lifetime information for a RumbleSource
    /// </summary>
    public class RumbleSource : MonoBehaviour
    {
        [Tooltip("The Info used to define the attributes of this RumbleIntensity.")]
        // TODO: [EmbeddedInspector]
        [SerializeField]
        private RumbleInfo info;
        
        [Tooltip("The Info used to define the attributes of this RumbleIntensity.")]
        // TODO: [EmbeddedInspector]
        [SerializeField]
        private RumbleAsset rumbleAsset;

        private CircleBounds circleBounds;

        private float timeElapsed;

        public bool IsAlive => !IsDead;

        public bool IsDead => timeElapsed >= Info.Lifetime;

        private IRumbleBounds Bounds => circleBounds;

        /// <summary>
        /// Gets or sets the Info used to define the attributes of this RumbleIntensity.
        /// </summary>
        /// <value>The Info.</value>
        public RumbleInfo Info => this.info;
        
        public void Initialize(RumbleAsset rumbleAsset, float radius)
        {
            this.timeElapsed = 0.0f;
            this.info = info;
            this.rumbleAsset = rumbleAsset;
            this.circleBounds = new CircleBounds(this.transform.position, radius);
        }

        public void Tick(float deltaTime)
        {
            this.timeElapsed += deltaTime;
        }

        /// <summary>
        /// Evaluates the values of the rumbleIntensity for a given listener position.
        /// </summary>
        /// <returns>The rumbleIntensity.</returns>
        /// <param name="listenerPosition">Listener position.</param>
        public RumbleIntensity EvaluateRumble(Vector3 listenerPosition)
        {
            // Assume zero when the parent or self is inactive
            if (!this.gameObject.activeInHierarchy)
            {
                return RumbleIntensity.Zero;
            }

            // When disabled, assume zero rumbleIntensity
            if (!this.enabled)
            {
                return RumbleIntensity.Zero;
            }

            // If no rumbleAsset is yet assigned, assume zero rumbleIntensity
            if (this.rumbleAsset == null)
            {
                return RumbleIntensity.Zero;
            }

            return rumbleAsset.CalculateRumbleFromPercentFromCenterAtTime(
                Bounds.GetPercentFromCenter(listenerPosition),
                this.timeElapsed
            );
        }

        private RumbleIntensity GetRumbleIntensityCenteredOnSource()
        {
            return this.EvaluateRumble(transform.position);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "RumbleIcon.png", true);

            if (this.GetRumbleIntensityCenteredOnSource() == RumbleIntensity.Zero)
            {
                return;
            }

            var intensityEstimate = this.GetRumbleIntensityCenteredOnSource().ForceFeedback.LeftMotor;
            float alpha = Mathf.Lerp(0.05f, 0.1f, intensityEstimate);
            UnityEditor.Handles.color = new Color(1.0f, 1.0f, 0.0f, alpha);
            Bounds.DrawGizmo();
        }
#endif

        /* Structs, Sub-Classes =================================================================================================== */
    }
}