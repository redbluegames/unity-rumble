namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// Info that describes Rumble. Rumble results in Force Feedback and Screenshake for a RumbleListener.
    /// </summary>
    [CreateAssetMenu()]
    public class RumbleInfo : ScriptableObject
    {
        public const float MinLifetime = 0.1f;
        public const float MinRadius = 0.0f;

        [SerializeField]
        private float lifetime = MinLifetime;

        [SerializeField]
        private float radius = MinRadius;

        [SerializeField]
        private RumbleIntensityMode intensityMode;

        [Tooltip("The intensity for the Rumble, which affects screen shake and force feedback.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float constantIntensity = 1.0f;

        [SerializeField]
        private IntensityCurveSettings intensityCurveSettings;

        [SerializeField]
        private RumbleFalloffFunction falloffFunction = RumbleFalloffFunction.Exponential;

        [SerializeField]
        private Rumble rumbleSettings;

        /// <summary>
        /// Rumble falloff function can be used to determine intensity as distance falls off.
        /// </summary>
        public enum RumbleFalloffFunction
        {
            /// <summary>
            /// No falloff. Intensity will be constant regardless of distance.
            /// </summary>
            None = 0,

            /// <summary>
            /// Linear falloff - linear with distance from center of rumble.
            /// </summary>
            Linear = 1,

            /// <summary>
            /// Exponential falloff - intensity is reduced with distance squared from the center.
            /// </summary>
            Exponential = 2
        }

        /// <summary>
        /// Rumble intensity mode describes the manner in which intensity is calculated over time.
        /// </summary>
        public enum RumbleIntensityMode
        {
            /// <summary>
            /// Intensity remains constant regardless of lifetime.
            /// </summary>
            Constant = 0,

            /// <summary>
            /// Intensity follows a curve, where Y axis is intensity from 0 to 1, and X axis is lifetime, from 0 to 1
            /// </summary>
            Curve = 1
        }

        /// <summary>
        /// Gets or sets the radius for the rumble. Nothing will be felt outside the radius.
        /// </summary>
        /// <value>The radius.</value>
        public float Radius
        {
            get => this.radius;
            set => this.radius = value;
        }

        /// <summary>
        /// Gets the intensity over lifetime.
        /// </summary>
        /// <value>The intensity over lifetime.</value>
        public RumbleIntensityMode IntensityOverLifetime => this.intensityMode;

        /// <summary>
        /// Gets or sets the intensity of the rumble when set to Constant.
        /// </summary>
        /// <value>The intensity.</value>
        public float ConstantIntensity => this.constantIntensity;

        /// <summary>
        /// Gets or sets the lifetime of the rumble.
        /// </summary>
        /// <value>The lifetime.</value>
        public float Lifetime
        {
            get => this.lifetime;
            set => this.lifetime = value;
        }

        /// <summary>
        /// Gets the falloff function, used to calculate intensity as distance from center increases.
        /// </summary>
        /// <value>The falloff function.</value>
        public RumbleFalloffFunction FalloffFunction
        {
            get => this.falloffFunction;
            set => this.falloffFunction = value;
        }

        /// <summary>
        /// Gets or sets the rumble settings for this rumble info.
        /// </summary>
        public Rumble RumbleSettings
        {
            get => this.rumbleSettings;
            set => this.rumbleSettings = value;
        }
        
        public static Rumble CalculateRumbleFromDistanceSquaredAtTime(RumbleInfo info, float distanceSquared, float time)
        {
            var rumble = CalculateRumbleAtTime(info, time);
            var scale = GetIntensityScalarForDistanceSquared(info, distanceSquared);
            return rumble * scale;
        }

        private static Rumble CalculateRumbleAtTime(RumbleInfo info, float time)
        {
            float rumbleIntensity = 0.0f;
            switch (info.IntensityOverLifetime)
            {
                case RumbleIntensityMode.Constant:
                    rumbleIntensity = time <= info.lifetime ? info.ConstantIntensity : 0.0f;
                    break;
                case RumbleIntensityMode.Curve:
                    float t = (time % info.intensityCurveSettings.Period) / info.intensityCurveSettings.Period;
                    if (info.intensityCurveSettings.Curve != null)
                    {
                        rumbleIntensity = info.intensityCurveSettings.Curve.Data.Evaluate(t);
                    }

                    break;
                default:
                    var errorMessage = string.Format(
                        "Unrecognized RumbleIntensityMode found on IntensityOverLifetime in RumbleInfo Mode: {0}",
                        info.IntensityOverLifetime);
                    Debug.LogError(errorMessage, info);
                    break;
            }

            return info.rumbleSettings * rumbleIntensity;
        }

        private static float GetIntensityScalarForDistanceSquared(RumbleInfo info, float distanceSquared)
        {
            float scaleFromDistance = 0.0f;
            bool listenerIsInRadius = distanceSquared <= Mathf.Pow(info.Radius, 2);
            if (!listenerIsInRadius)
            {
                return 0.0f;
            }

            switch (info.FalloffFunction)
            {
                case RumbleFalloffFunction.None:
                    scaleFromDistance = 1.0f;
                    break;
                case RumbleFalloffFunction.Linear:
                    if (info.Radius > 0.0f)
                    {
                        float distance = Mathf.Sqrt(distanceSquared);
                        float distanceT = Mathf.Clamp01(1.0f - (distance / info.Radius));
                        scaleFromDistance = Mathf.Lerp(0.0f, 1.0f, distanceT);
                    }
                    else
                    {
                        // If Radius is 0 and they are in the radius, they must be right on top of it. Return full intensity.
                        scaleFromDistance = 1.0f;
                    }

                    break;
                case RumbleFalloffFunction.Exponential:
                    if (info.Radius > 0.0f)
                    {
                        // Exponential falloff is y=(x-1)^2, where x is the distance towards the edge of the radius, where 1.0f is
                        // at the edge.
                        float distance = Mathf.Sqrt(distanceSquared);
                        float x = distance / info.Radius;
                        scaleFromDistance = Mathf.Clamp01(Mathf.Pow(x - 1.0f, 2.0f));
                    }
                    else
                    {
                        scaleFromDistance = 1.0f;
                    }

                    break;
                default:
                    Debug.Log($"Unrecognized RumbleFalloff mode, {info.FalloffFunction}, on RumbleInfo.", info);
                    break;
            }

            return scaleFromDistance;
        }

        /// <summary>
        /// Settings for a curve that defines Intensity
        /// </summary>
        [System.Serializable]
        public class IntensityCurveSettings
        {
            public const float MinPeriod = 0.01f;

            [Tooltip("A curve describing intensity over time, where intensity and time are normalized.")]
            [SerializeField]
            private Curve curve;

            [Tooltip("Flag whether or not to loop the intensity curve.")]
            [SerializeField]
            private bool isLooping;

            [Tooltip("The period for the intensity curve in seconds.")]
            [SerializeField]
            private float period;

            /// <summary>
            /// Gets or sets the curve that defines Intensity as a value from 0 to 1.0
            /// </summary>
            /// <value>The curve.</value>
            public Curve Curve
            {
                get => this.curve;
                set => this.curve = value;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this curve should loop.
            /// </summary>
            /// <value><c>true</c> if this curve is looping; otherwise, <c>false</c>.</value>
            public bool IsLooping
            {
                get => this.isLooping;
                set => this.isLooping = value;
            }

            /// <summary>
            /// Gets or sets the period of the curve in seconds.
            /// </summary>
            /// <value>The period in seconds.</value>
            public float Period
            {
                get => this.period;
                set => this.period = value;
            }
        }
    }
}