using UnityEngine;

namespace RedBlueGames.Rumble
{
    public class CircleBounds : IRumbleBounds
    {
        public Vector3 WorldCenter { get; }
        
        private float Radius { get; }

        public CircleBounds(Vector3 worldCenter, float radius)
        {
            WorldCenter = worldCenter;
            Radius = radius;
        }

        public float GetPercentFromCenter(Vector3 position)
        {
            return (position - WorldCenter).magnitude / Radius;
        }

        public void DrawGizmo()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.DrawSolidDisc(WorldCenter, Vector3.forward, Radius);
#endif
        }
    }
}