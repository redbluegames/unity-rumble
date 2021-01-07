using UnityEngine;

namespace RedBlueGames.Rumble
{
    public class RumbleSpawner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The rumbleIntensity info to bind to the spawned rumbleIntensity.")]
        private RumbleInfo rumbleInfo;
        
        [SerializeField]
        [Tooltip("The rumbleIntensity info to bind to the spawned rumbleIntensity.")]
        private RumbleAsset rumbleAsset;

        [SerializeField]
        [Tooltip("The radius to use for the rumble source.")]
        private float radius;

        // Start is called before the first frame update
        void Start()
        {
            var rumble = RumbleManager.Instance.StartRumble(this.transform.position, this.rumbleAsset, this.radius);
            rumble.transform.SetParent(this.transform);
        }
    }
}
