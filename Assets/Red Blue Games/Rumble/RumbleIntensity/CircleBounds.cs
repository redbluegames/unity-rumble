using UnityEngine;

namespace RedBlueGames.Rumble
{
    public class CircleBounds : IRumbleBounds
    {
        public Vector3 Center { get; }
        
        private float Radius { get; }

        public CircleBounds(Vector3 position, float radius)
        {
            Center = position;
            Radius = radius;
        }

        public float GetPercentFromCenter(Vector3 position)
        {
            return (position - Center).magnitude / Radius;
        }

        public void DrawGizmo()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.DrawSolidDisc(Center, Vector3.forward, Radius);
#endif
        }
    }
}