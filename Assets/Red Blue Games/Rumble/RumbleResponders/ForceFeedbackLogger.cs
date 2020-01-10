namespace RedBlueGames.Rumble
{
    using UnityEngine;

    public class ForceFeedbackLogger : IForceFeedbackResponder
    {
        public void SetVibration(ForceFeedbackIntensities intensities)
        {
            Debug.Log($"Setting Vibration. Left: {intensities.LeftMotor}, Right: {intensities.RightMotor}");
        }
    }
}