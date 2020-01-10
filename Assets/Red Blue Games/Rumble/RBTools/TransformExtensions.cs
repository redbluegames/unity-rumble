namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Extension methods to Transform class
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Gets all children transforms, depth first.
        /// </summary>
        /// <param name="transform">Transform to find children in.</param>
        /// <returns>All children transforms, depth first.</returns>
        public static List<Transform> GetAllChildrenDepthFirst(this Transform transform)
        {
            var children = new List<Transform>();
            for (int i = 0; i < transform.childCount; ++i)
            {
                children.AddRange(transform.GetChild(i).GetAllChildrenDepthFirst());
            }

            children.Add(transform);

            return children;
        }
    }
}
