namespace RedBlueGames.Rumble
{
    using UnityEngine;

    /// <summary>
    /// ForceFeedback intensities describe and perform calculations for intensity for the motors and screen.
    /// </summary>
    [System.Serializable]
    public struct ForceFeedbackIntensities : System.IEquatable<ForceFeedbackIntensities>
    {
        public static readonly ForceFeedbackIntensities Zero = new ForceFeedbackIntensities();

        [Tooltip("The strength of the left motor on the controller. On XBox One this feels more bass-y.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float leftMotor;

        [Tooltip("The strength of the right motor on the controller.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float rightMotor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForceFeedbackIntensities"/> struct.
        /// </summary>
        /// <param name="leftMotorIntesitity">Left motor intesitity.</param>
        /// <param name="rightMotorIntensity">Right motor intensity.</param>
        public ForceFeedbackIntensities(float leftMotorIntesitity, float rightMotorIntensity)
        {
            this.leftMotor = leftMotorIntesitity;
            this.rightMotor = rightMotorIntensity;

            // VS doesn't allow using 'this' before all of its fields have been set, so run the values thru the Properties now
            this.LeftMotor = this.leftMotor;
            this.RightMotor = this.rightMotor;
        }

        /// <summary>
        /// Gets or sets the left motor intensity.
        /// </summary>
        /// <value>The left motor intensity.</value>
        public float LeftMotor
        {
            get
            {
                return this.leftMotor;
            }

            set
            {
                // Motor levels can never be greater than 1 or less than 0
                this.leftMotor = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// Gets or sets the right motor intensity.
        /// </summary>
        /// <value>The right motor intensity.</value>
        public float RightMotor
        {
            get
            {
                return this.rightMotor;
            }

            set
            {
                // Motor levels can never be greater than 1 or less than 0
                this.rightMotor = Mathf.Clamp01(value);
            }
        }

        /// <summary>>Comparse two <see cref="ForceFeedbackIntensities"/> for equality.</summary>
        /// <param name="lhs">lhs ForceFeedbackIntensities.</param>
        /// <param name="rhs">rhs ForceFeedbackIntensities</param>
        /// <returns>True if equal, false otherwise</returns>
        public static bool operator ==(ForceFeedbackIntensities lhs, ForceFeedbackIntensities rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>>Comparse two <see cref="ForceFeedbackIntensities"/> for inequality.</summary>
        /// <param name="lhs">lhs ForceFeedbackIntensities.</param>
        /// <param name="rhs">rhs ForceFeedbackIntensities</param>
        /// <returns>True if not equal, false otherwise</returns>
        public static bool operator !=(ForceFeedbackIntensities lhs, ForceFeedbackIntensities rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Adds two ForceFeedbackIntensities together, clamped to 0 and 1.
        /// </summary>
        /// <param name="lhs">ForceFeedbackIntensities for the left side of the operator</param>
        /// <param name="rhs">ForceFeedbackIntensities for the right side of the operator</param>
        /// <returns>"Returns the sum of the two ForceFeedbackIntensities</returns>
        public static ForceFeedbackIntensities operator +(ForceFeedbackIntensities lhs, ForceFeedbackIntensities rhs)
        {
            var result = new ForceFeedbackIntensities();

            result.LeftMotor = lhs.LeftMotor + rhs.LeftMotor;
            result.RightMotor = lhs.RightMotor + rhs.RightMotor;

            return result;
        }

        /// <summary>
        /// Scales a ForceFeedbackIntensities by a scalar float. Intensity is clamped to 0 and 1.
        /// </summary>
        /// <param name="intensities">Intensities to scale.</param>
        /// <param name="scalar">Scalar to apply to intensities.</param>
        /// <returns>"Returns the scaled ForceFeedbackIntensities, calmped to 0 and 1.</returns>
        public static ForceFeedbackIntensities operator *(ForceFeedbackIntensities intensities, float scalar)
        {
            return Scale(intensities, scalar);
        }

        /// <summary>
        /// Scales a ForceFeedbackIntensities by a scalar float. Intensity is clamped to 0 and 1.
        /// </summary>
        /// <param name="scalar">Scalar to apply to intensities.</param>
        /// <param name="intensities">Intensities to scale.</param>
        /// <returns>"Returns the scaled ForceFeedbackIntensities, calmped to 0 and 1.</returns>
        public static ForceFeedbackIntensities operator *(float scalar, ForceFeedbackIntensities intensities)
        {
            return Scale(intensities, scalar);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="ForceFeedbackIntensities"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="ForceFeedbackIntensities"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="ForceFeedbackIntensities"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ForceFeedbackIntensities)
            {
                return this.Equals((ForceFeedbackIntensities)obj);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ForceFeedbackIntensities"/> is equal to the current <see cref="ForceFeedbackIntensities"/>.
        /// </summary>
        /// <param name="other">The <see cref="ForceFeedbackIntensities"/> to compare with the current <see cref="ForceFeedbackIntensities"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ForceFeedbackIntensities"/> is equal to the current
        /// <see cref="ForceFeedbackIntensities"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ForceFeedbackIntensities other)
        {
            return
                Mathf.Approximately(this.LeftMotor, other.LeftMotor) &&
                Mathf.Approximately(this.RightMotor, other.RightMotor);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="ForceFeedbackIntensities"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return
                this.LeftMotor.GetHashCode() ^
                this.RightMotor.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="ForceFeedbackIntensities"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="ForceFeedbackIntensities"/>.</returns>
        public override string ToString()
        {
            return string.Format("[LeftMotor={0}, RightMotor={1}]", this.LeftMotor, this.RightMotor);
        }

        private static ForceFeedbackIntensities Scale(ForceFeedbackIntensities intensities, float scalar)
        {
            var result = new ForceFeedbackIntensities();

            // Rumble Intensities can never exceed 1.0
            result.LeftMotor = intensities.LeftMotor * scalar;
            result.RightMotor = intensities.RightMotor * scalar;

            return result;
        }
    }
}