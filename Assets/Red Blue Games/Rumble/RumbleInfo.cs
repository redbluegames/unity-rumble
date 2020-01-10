// TODO: Namespace this
using System.Collections;
using System.Collections.Generic;
using RedBlueGames.Tools;
using UnityEngine;

/// <summary>
/// Info that describes Rumble. Rumble results in Force Feedback and Screenshake for a RumbleListener.
/// </summary>
[CreateAssetMenu()]
public class RumbleInfo : ScriptableObject
{
    /* Consts, Fields ========================================================================================================= */
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
    private float constantIntensity;

    [SerializeField]
    private IntensityCurveSettings intensityCurveSettings;

    [SerializeField]
    private RumbleFalloffFunction falloffFunction = RumbleFalloffFunction.Exponential;

    [SerializeField]
    private Rumble rumbleSettings;

    /* Enums ================================================================================================================== */

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

    /* Properties ============================================================================================================= */

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
    public RumbleFalloffFunction FalloffFunction => this.falloffFunction;

    /// <summary>
    /// Gets or sets the rumble settings for this rumble info.
    /// </summary>
    public Rumble RumbleSettings
    {
        get => this.rumbleSettings;
        set => this.rumbleSettings = value;
    }

    /* Methods ================================================================================================================ */

    /// <summary>
    /// Calculates the rumble intensity for a given elapsed time.
    /// </summary>
    /// <returns>The rumble intensity at time.</returns>
    /// <param name="time">Elapsed time.</param>
    public Rumble CalculateRumbleAtTime(float time)
    {
        float t = (time % this.intensityCurveSettings.Period) / this.intensityCurveSettings.Period;
        float rumbleIntensity = 0.0f;
        switch (this.IntensityOverLifetime)
        {
            case RumbleInfo.RumbleIntensityMode.Constant:
                rumbleIntensity = this.ConstantIntensity;
                break;
            case RumbleInfo.RumbleIntensityMode.Curve:
                if (this.intensityCurveSettings.Curve != null)
                {
                    rumbleIntensity = this.intensityCurveSettings.Curve.Data.Evaluate(t);
                }

                break;
            default:
                var errorMessage = string.Format(
                                       "Unrecognized RumbleIntensityMode found on IntensityOverLifetime in RumbleInfo Mode: {0}",
                                       this.IntensityOverLifetime);
                Debug.LogError(errorMessage, this);
                break;
        }

        return this.rumbleSettings * rumbleIntensity;
    }

    /* Structs, Sub-Classes =================================================================================================== */

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