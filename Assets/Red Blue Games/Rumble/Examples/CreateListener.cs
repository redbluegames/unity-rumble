using System.Collections;
using System.Collections.Generic;
using RedBlueGames.Rumble;
using UnityEngine;

public class CreateListener : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var listener = RumbleListener.Create(new ForceFeedbackLogger(), new ScreenShakeLogger());
        listener.ForceFeedbackMultiplier = 1.0f;
        listener.ScreenShakeMultiplier = 1.0f;
    }
}
