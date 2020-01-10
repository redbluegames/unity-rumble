using System.Collections;
using System.Collections.Generic;
using RedBlueGames.Tools;
using UnityEngine;

/// <summary>
/// A source for Rumble. This handles positional and lifetime information for a RumbleSource
/// </summary>
public class RumbleSource : MonoBehaviour
{
    /* Consts, Fields ========================================================================================================= */

    [Tooltip("The Info used to define the attributes of this Rumble.")]
    // TODO: [EmbeddedInspector]
    [SerializeField]
    private RumbleInfo info;

    private float timeElapsed;

    /* Enums ================================================================================================================== */

    /* Properties ============================================================================================================= */

    /// <summary>
    /// Gets or sets the Info used to define the attributes of this Rumble.
    /// </summary>
    /// <value>The Info.</value>
    public RumbleInfo Info
    {
        get
        {
            return this.info;
        }

        set
        {
            this.info = value;

            // Reset Lifetime when new info gets assigned
            this.timeElapsed = 0.0f;
        }
    }

    /* Methods ================================================================================================================ */

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

        float scaleFromDistance = this.GetIntensityScalarForListenerPosition(listenerPosition);

        // User is out of range of the rumble
        if (scaleFromDistance <= 0.0f)
        {
            return Rumble.Zero;
        }

        return this.GetCurrentRumble() * scaleFromDistance;
    }

    protected void Awake()
    {
        this.timeElapsed = 0.0f;
    }

    protected void Start()
    {
        RumbleManager.Instance.RegisterRumbleSource(this);
    }

    /// <summary>
    /// Unity's Update message. Updates every frame.
    /// </summary>
    protected void Update()
    {
        this.timeElapsed += Time.deltaTime;
        if (this.timeElapsed >= this.Info.Lifetime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private Rumble GetCurrentRumble()
    {
        if (this.Info == null)
        {
            return Rumble.Zero;
        }
        else
        {
            return this.Info.CalculateRumbleAtTime(this.timeElapsed);
        }
    }

    private float GetIntensityScalarForListenerPosition(Vector3 listenerPosition)
    {
        float scaleFromDistance = 0.0f;
        float distanceT = 1.0f;
        var directionToListener = listenerPosition - this.transform.position;
        bool listenerIsInRadius = directionToListener.sqrMagnitude < Mathf.Pow(this.Info.Radius, 2);
        if (!listenerIsInRadius)
        {
            return 0.0f;
        }

        switch (this.Info.FalloffFunction)
        {
            case RumbleInfo.RumbleFalloffFunction.None:
                scaleFromDistance = 1.0f;
                break;
            case RumbleInfo.RumbleFalloffFunction.Linear:
                if (this.Info.Radius > 0.0f)
                {
                    var distanceToListener = directionToListener.magnitude;
                    distanceT = Mathf.Clamp01(1.0f - (distanceToListener / this.Info.Radius));
                    scaleFromDistance = Mathf.Lerp(0.0f, 1.0f, distanceT);
                }
                else
                {
                    // If Radius is 0 and they are in the radius, they must be right on top of it. Return full intensity.
                    scaleFromDistance = 1.0f;
                }

                break;
            case RumbleInfo.RumbleFalloffFunction.Exponential:
                if (this.Info.Radius > 0.0f)
                {
                    // Exponential falloff is y=(x-1)^2, where x is the distance towards the edge of the radius, where 1.0f is
                    // at the edge.
                    var distanceToListener = directionToListener.magnitude;
                    float x = distanceToListener / this.Info.Radius;
                    scaleFromDistance = Mathf.Clamp01(Mathf.Pow(x - 1.0f, 2.0f));
                }
                else
                {
                    scaleFromDistance = 1.0f;
                }

                break;
            default:
                Debug.LogErrorFormat(this, "Unrecognized RumbleFalloff mode, {0}, on Rumble object.", this.Info.FalloffFunction);
                break;
        }

        return scaleFromDistance;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "RumbleIcon.png", true);

        if (this.GetCurrentRumble() == Rumble.Zero)
        {
            return;
        }

        var intensityEstimate = this.GetCurrentRumble().ForceFeedback.LeftMotor;
        float alpha = Mathf.Lerp(0.05f, 0.1f, intensityEstimate);
        UnityEditor.Handles.color = new Color(1.0f, 1.0f, 0.0f, alpha);
        float sphereRadius = this.Info != null ? this.Info.Radius : 0.0f;
        UnityEditor.Handles.DrawSolidDisc(this.transform.position, Vector3.forward, sphereRadius);
    }
#endif

    /* Structs, Sub-Classes =================================================================================================== */
}
