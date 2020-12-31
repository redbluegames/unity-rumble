namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// RumbleIntensity describes and perform calculations for intensity for the motors and screen shake.
    /// </summary>
    [System.Serializable]
    public struct RumbleIntensity : System.IEquatable<RumbleIntensity>
    {
        public static readonly RumbleIntensity Zero = new RumbleIntensity();
        public static readonly RumbleIntensity One = new RumbleIntensity(ForceFeedbackIntensities.One, ScreenShakeIntensities.One);

        [SerializeField]
        private ForceFeedbackIntensities forceFeedback;

        [SerializeField]
        private ScreenShakeIntensities screenShake;

        /// <summary>
        /// Initializes a new instance of the <see cref="RumbleIntensity"/> struct.
        /// </summary>
        /// <param name="forceFeedback">Force feedback.</param>
        /// <param name="screenShake">Screen shake.</param>
        public RumbleIntensity(ForceFeedbackIntensities forceFeedback, ScreenShakeIntensities screenShake)
        {
            this.forceFeedback = forceFeedback;
            this.screenShake = screenShake;
        }

        /// <summary>
        /// Gets or sets the force feedback for this RumbleIntensity
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

        /// <summary>>Comparse two <see cref="RumbleIntensity"/> for equality.</summary>
        /// <param name="lhs">lhs RumbleIntensity.</param>
        /// <param name="rhs">rhs RumbleIntensity</param>
        /// <returns>True if equal, false otherwise</returns>
        public static bool operator ==(RumbleIntensity lhs, RumbleIntensity rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>>Comparse two <see cref="RumbleIntensity"/> for inequality.</summary>
        /// <param name="lhs">lhs RumbleIntensity.</param>
        /// <param name="rhs">rhs RumbleIntensity</param>
        /// <returns>True if not equal, false otherwise</returns>
        public static bool operator !=(RumbleIntensity lhs, RumbleIntensity rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Adds two RumbleIntensity together, clamped to 0 and 1.
        /// </summary>
        /// <param name="lhs">RumbleIntensity for the left side of the operator</param>
        /// <param name="rhs">RumbleIntensity for the right side of the operator</param>
        /// <returns>"Returns the sum of the two RumbleIntensity</returns>
        public static RumbleIntensity operator +(RumbleIntensity lhs, RumbleIntensity rhs)
        {
            var result = new RumbleIntensity();

            // Add the force feedbacks
            result.forceFeedback = lhs.forceFeedback + rhs.forceFeedback;
            result.screenShake = lhs.screenShake + rhs.screenShake;

            return result;
        }

        /// <summary>
        /// Scales a RumbleIntensity by a scalar float. Intensity is clamped to 0 and 1.
        /// </summary>
        /// <param name="rumbleIntensityto scale.</param>
        /// <param name="scalar">Scalar to apply to rumbleIntensity.</param>
        /// <returns>"Returns the scaled RumbleIntensity, calmped to 0 and 1.</returns>
        public static RumbleIntensity operator *(RumbleIntensity rumbleIntensity, float scalar)
        {
            return Scale(rumbleIntensity, scalar);
        }

        /// <summary>
        /// Scales a RumbleIntensity by a scalar float. Intensity is clamped to 0 and 1.
        /// </summary>
        /// <param name="scalar">Scalar to apply to rumbleIntensity.</param>
        /// <param name="rumbleIntensityto scale.</param>
        /// <returns>"Returns the scaled RumbleIntensity, calmped to 0 and 1.</returns>
        public static RumbleIntensity operator *(float scalar, RumbleIntensity rumbleIntensity)
        {
            return Scale(rumbleIntensity, scalar);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="RumbleIntensity"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="RumbleIntensity"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="RumbleIntensity"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RumbleIntensity)
            {
                return this.Equals((RumbleIntensity)obj);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="RumbleIntensity"/> is equal to the current <see cref="RumbleIntensity"/>.
        /// </summary>
        /// <param name="other">The <see cref="RumbleIntensity"/> to compare with the current <see cref="RumbleIntensity"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="RumbleIntensity"/> is equal to the current
        /// <see cref="RumbleIntensity"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(RumbleIntensity other)
        {
            return
                this.ForceFeedback == other.ForceFeedback &&
                this.ScreenShake == other.ScreenShake;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="RumbleIntensity"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return
                this.ForceFeedback.GetHashCode() ^
                this.ScreenShake.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="RumbleIntensity"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="RumbleIntensity"/>.</returns>
        public override string ToString()
        {
            return string.Format("[ForceFeedback:{0}, ScreenShake:{1}]", this.forceFeedback, this.screenShake);
        }

        private static RumbleIntensity Scale(RumbleIntensity rumbleIntensity, float scalar)
        {
            var result = new RumbleIntensity();

            // Scale ForceFeedbacks
            result.ForceFeedback = rumbleIntensity.ForceFeedback * scalar;
            result.ScreenShake = rumbleIntensity.ScreenShake * scalar;

            return result;
        }
    }
}