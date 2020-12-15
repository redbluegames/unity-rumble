namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// ScreenShakeIntensities describe and perform calculations for intensity for the motors and screen.
    /// </summary>
    [System.Serializable]
    public struct ScreenShakeIntensities : System.IEquatable<ScreenShakeIntensities>
    {
        public static readonly ScreenShakeIntensities Zero = new ScreenShakeIntensities();

        [Tooltip("The strength of the shake on the X and Y axis. Defines a box about the source in which a point will be picked" +
            "to shake towards.")]
        [SerializeField]
        private Vector2 strength;

        [Tooltip("The vibrato of the shake defines how many points are picked in one camera shake cycle (which is " +
            "currently 0.2f seconds)")]
        [Range(1, 20)]
        [SerializeField]
        private int vibrato;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenShakeIntensities"/> struct.
        /// </summary>
        /// <param name="strength">Strength of the shake.</param>
        /// <param name="vibrato">Vibrato for the shake.</param>
        public ScreenShakeIntensities(Vector2 strength, int vibrato = 1)
        {
            this.strength = strength;
            this.vibrato = vibrato;
        }

        /// <summary>
        /// Gets or sets the strength of the shake on the X and Y axis. Defines a box about the source 
        /// in which a point will be picked to shake towards.
        /// </summary>
        /// <value>The strength.</value>
        public Vector2 Strength
        {
            get => this.strength;
            set => this.strength = value;
        }

        /// <summary>
        /// Gets or sets the vibrato for the screen shake. The vibrato of the shake defines how many points are 
        /// picked in one camera shake cycle (which is currently 0.2f seconds)
        /// </summary>
        /// <value>The vibrato.</value>
        public int Vibrato
        {
            get => this.vibrato;
            set => this.vibrato = value;
        }

        /// <summary>>Comparse two <see cref="ScreenShakeIntensities"/> for equality.</summary>
        /// <param name="lhs">lhs ScreenShakeIntensities.</param>
        /// <param name="rhs">rhs ScreenShakeIntensities</param>
        /// <returns>True if equal, false otherwise</returns>
        public static bool operator ==(ScreenShakeIntensities lhs, ScreenShakeIntensities rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>>Comparse two <see cref="ScreenShakeIntensities"/> for inequality.</summary>
        /// <param name="lhs">lhs ScreenShakeIntensities.</param>
        /// <param name="rhs">rhs ScreenShakeIntensities</param>
        /// <returns>True if not equal, false otherwise</returns>
        public static bool operator !=(ScreenShakeIntensities lhs, ScreenShakeIntensities rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Adds two ScreenShakeIntensities together, clamped to 0 and 1.
        /// </summary>
        /// <param name="lhs">ScreenShakeIntensities for the left side of the operator</param>
        /// <param name="rhs">ScreenShakeIntensities for the right side of the operator</param>
        /// <returns>"Returns the sum of the two ScreenShakeIntensities</returns>
        public static ScreenShakeIntensities operator +(ScreenShakeIntensities lhs, ScreenShakeIntensities rhs)
        {
            var result = new ScreenShakeIntensities();

            // Strength adds, Vibrato just takes whichever is greater
            result.Strength = lhs.Strength + rhs.Strength;
            result.Vibrato = Mathf.Max(lhs.Vibrato, rhs.Vibrato);

            return result;
        }

        /// <summary>
        /// Scales a ScreenShakeIntensities by a scalar float. Intensity is clamped to 0 and 1.
        /// </summary>
        /// <param name="intensities">Intensities to scale.</param>
        /// <param name="scalar">Scalar to apply to intensities.</param>
        /// <returns>"Returns the scaled ScreenShakeIntensities, calmped to 0 and 1.</returns>
        public static ScreenShakeIntensities operator *(ScreenShakeIntensities intensities, float scalar)
        {
            return Scale(intensities, scalar);
        }

        /// <summary>
        /// Scales a ScreenShakeIntensities by a scalar float. Intensity is clamped to 0 and 1.
        /// </summary>
        /// <param name="scalar">Scalar to apply to intensities.</param>
        /// <param name="intensities">Intensities to scale.</param>
        /// <returns>"Returns the scaled ScreenShakeIntensities, calmped to 0 and 1.</returns>
        public static ScreenShakeIntensities operator *(float scalar, ScreenShakeIntensities intensities)
        {
            return Scale(intensities, scalar);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="ScreenShakeIntensities"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="ScreenShakeIntensities"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="ScreenShakeIntensities"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ScreenShakeIntensities)
            {
                return this.Equals((ScreenShakeIntensities)obj);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ScreenShakeIntensities"/> is equal to the current <see cref="ScreenShakeIntensities"/>.
        /// </summary>
        /// <param name="other">The <see cref="ScreenShakeIntensities"/> to compare with the current <see cref="ScreenShakeIntensities"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ScreenShakeIntensities"/> is equal to the current
        /// <see cref="ScreenShakeIntensities"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ScreenShakeIntensities other)
        {
            return
                this.Strength == other.Strength &&
                this.Vibrato == other.Vibrato;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="ScreenShakeIntensities"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing
        /// algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return
                this.Strength.GetHashCode() ^
                this.Vibrato.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="ScreenShakeIntensities"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="ScreenShakeIntensities"/>.</returns>
        public override string ToString()
        {
            return string.Format(
                "[ScreenShakeIntensities: Strength={0}, Vibrato={1}",
                this.Strength,
                this.Vibrato);
        }

        private static ScreenShakeIntensities Scale(ScreenShakeIntensities intensities, float scalar)
        {
            var result = new ScreenShakeIntensities();

            result.Strength = intensities.Strength * scalar;

            // Vibrato shouldn't be affected as you approach a source.
            result.Vibrato = intensities.Vibrato;

            return result;
        }
    }
}