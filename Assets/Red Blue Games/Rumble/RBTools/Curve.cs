using RedBlueGames.Tools;
using UnityEngine;

/// <summary>
/// Stores a Unity AnimationCurve as an asset
/// </summary>
public class Curve : ScriptableObject
{
    [SerializeField]
    private AnimationCurve curve;

    /// <summary>
    /// Gets or sets the curve stored by this CurveObject
    /// </summary>
    public AnimationCurve Data
    {
        get => this.curve;
        set => this.curve = value;
    }

    /// <summary>
    /// Gets the horizontal-axis value of the last point of this <see cref="Curve"/>
    /// </summary>
    public float Length
    {
        get
        {
            return this.Data.GetLifetime();
        }
    }

    /// <summary>
    /// Creates a curve with the specified Keyframes.
    /// </summary>
    /// <returns>The created curve.</returns>
    /// <param name="keys">Keyframes for the curve.</param>
    public static Curve CreateCurve(params Keyframe[] keys)
    {
        var createdCurve = ScriptableObject.CreateInstance<Curve>();
        var animCurve = new AnimationCurve(keys);
        createdCurve.curve = animCurve;
        return createdCurve;
    }
}