namespace RedBlueGames.Tools
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Animation curve extensions.
    /// </summary>
    public static class AnimationCurveExtensions
    {
        /// <summary>
        /// Gets the time of the last key in the curve.
        /// </summary>
        /// <returns>The lifetime.</returns>
        /// <param name="curve">Curve to evaluate.</param>
        public static float GetLifetime(this AnimationCurve curve)
        {
            return curve.keys[curve.length - 1].time;
        }
    }
}