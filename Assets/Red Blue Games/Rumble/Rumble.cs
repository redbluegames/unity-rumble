using UnityEngine;

/// <summary>
/// Rumble describes and perform calculations for intensity for the motors and screen shake.
/// </summary>
[System.Serializable]
public struct Rumble : System.IEquatable<Rumble>
{
    /* Consts, Fields ======================================================================================================== */
    public static readonly Rumble Zero = new Rumble();

    [SerializeField]
    private ForceFeedbackIntensities forceFeedback;

    [SerializeField]
    private ScreenShakeIntensities screenShake;

    /* Enums  ============================================================================================================== */

    /* Constructors ======================================================================================================== */

    /// <summary>
    /// Initializes a new instance of the <see cref="Rumble"/> struct.
    /// </summary>
    /// <param name="forceFeedback">Force feedback.</param>
    /// <param name="screenShake">Screen shake.</param>
    public Rumble(ForceFeedbackIntensities forceFeedback, ScreenShakeIntensities screenShake)
    {
        this.forceFeedback = forceFeedback;
        this.screenShake = screenShake;
    }

    /* Properties =========================================================================================================== */

    /// <summary>
    /// Gets or sets the force feedback for this Rumble
    /// </summary>
    /// <value>The force feedback values.</value>
    public ForceFeedbackIntensities ForceFeedback
    {
        get => this.forceFeedback;
        set => this.forceFeedback = value;
    }

    /// <summary>
    /// Gets or sets the screen shake intensities
    /// </summary>
    /// <value>The screen shake.</value>
    public ScreenShakeIntensities ScreenShake
    {
        get => this.screenShake;
        set => this.screenShake = value;
    }

    /// <summary>>Comparse two <see cref="Rumble"/> for equality.</summary>
    /// <param name="lhs">lhs Rumble.</param>
    /// <param name="rhs">rhs Rumble</param>
    /// <returns>True if equal, false otherwise</returns>
    public static bool operator ==(Rumble lhs, Rumble rhs)
    {
        return lhs.Equals(rhs);
    }

    /// <summary>>Comparse two <see cref="Rumble"/> for inequality.</summary>
    /// <param name="lhs">lhs Rumble.</param>
    /// <param name="rhs">rhs Rumble</param>
    /// <returns>True if not equal, false otherwise</returns>
    public static bool operator !=(Rumble lhs, Rumble rhs)
    {
        return !lhs.Equals(rhs);
    }

    /// <summary>
    /// Adds two Rumble together, clamped to 0 and 1.
    /// </summary>
    /// <param name="lhs">Rumble for the left side of the operator</param>
    /// <param name="rhs">Rumble for the right side of the operator</param>
    /// <returns>"Returns the sum of the two Rumble</returns>
    public static Rumble operator +(Rumble lhs, Rumble rhs)
    {
        var result = new Rumble();

        // Add the force feedbacks
        result.forceFeedback = lhs.forceFeedback + rhs.forceFeedback;
        result.screenShake = lhs.screenShake + rhs.screenShake;

        return result;
    }

    /// <summary>
    /// Scales a Rumble by a scalar float. Intensity is clamped to 0 and 1.
    /// </summary>
    /// <param name="rumble">Rumble to scale.</param>
    /// <param name="scalar">Scalar to apply to rumble.</param>
    /// <returns>"Returns the scaled Rumble, calmped to 0 and 1.</returns>
    public static Rumble operator *(Rumble rumble, float scalar)
    {
        return Scale(rumble, scalar);
    }

    /// <summary>
    /// Scales a Rumble by a scalar float. Intensity is clamped to 0 and 1.
    /// </summary>
    /// <param name="scalar">Scalar to apply to rumble.</param>
    /// <param name="rumble">Rumble to scale.</param>
    /// <returns>"Returns the scaled Rumble, calmped to 0 and 1.</returns>
    public static Rumble operator *(float scalar, Rumble rumble)
    {
        return Scale(rumble, scalar);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Rumble"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Rumble"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="Rumble"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        if (obj is Rumble)
        {
            return this.Equals((Rumble)obj);
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Rumble"/> is equal to the current <see cref="Rumble"/>.
    /// </summary>
    /// <param name="other">The <see cref="Rumble"/> to compare with the current <see cref="Rumble"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Rumble"/> is equal to the current
    /// <see cref="Rumble"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(Rumble other)
    {
        return
            this.ForceFeedback == other.ForceFeedback &&
            this.ScreenShake == other.ScreenShake;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="Rumble"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
        return
            this.ForceFeedback.GetHashCode() ^
            this.ScreenShake.GetHashCode();
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="Rumble"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="Rumble"/>.</returns>
    public override string ToString()
    {
        return string.Format("[ForceFeedback:{0}, ScreenShake:{1}]", this.forceFeedback, this.screenShake);
    }

    private static Rumble Scale(Rumble rumble, float scalar)
    {
        var result = new Rumble();

        // Scale ForceFeedbacks
        result.ForceFeedback = rumble.ForceFeedback * scalar;
        result.ScreenShake = rumble.ScreenShake * scalar;

        return result;
    }
}