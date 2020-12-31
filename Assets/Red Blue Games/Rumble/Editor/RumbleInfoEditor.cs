namespace RedBlueGames.Rumble.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.AnimatedValues;
    using UnityEngine;

    /// <summary>
    /// Drawer for RumbleInfo.
    /// </summary>
    [CustomEditor(typeof(RumbleInfo))]
    public class RumbleInfoEditor : Editor
    {
        private SerializedProperty lifetimeProperty;
        private SerializedProperty radiusProperty;
        private SerializedProperty intensityModeProperty;
        private SerializedProperty constantIntensityProperty;
        private SerializedProperty intensityCurveProperty;
        private SerializedProperty intensityCurvePeriodProperty;
        private SerializedProperty isIntensityCurveLoopedProperty;
        private SerializedProperty falloffFunctionProperty;
        private SerializedProperty rumbleSettingsProperty;

        private AnimBool constantIntensityAnim = new AnimBool();
        private AnimBool curveIntensityAnim = new AnimBool();

        /// <summary>
        /// Gets the target, typed as RumbleInfo.
        /// </summary>
        /// <value>The target being drawn.</value>
        protected RumbleInfo Target
        {
            get
            {
                return (RumbleInfo)(object)target;
            }
        }

        /// <summary>
        /// Draw the Inspector for the RumbleInfo
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Pull the property values from the asset/object
            serializedObject.Update();

            EditorGUILayout.PropertyField(this.lifetimeProperty);
            EditorGUILayout.PropertyField(this.radiusProperty);
            EditorGUILayout.PropertyField(this.intensityModeProperty, new GUIContent("Intensity Over Time"));

            this.DrawGUIIntensitySettings();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.falloffFunctionProperty, new GUIContent("Falloff"));

            EditorGUILayout.Space();

            this.DrawDivider();

            EditorGUILayout.PropertyField(this.rumbleSettingsProperty, true);

            if (GUI.changed)
            {
                // Clamp values to their minimums
                this.radiusProperty.floatValue = Mathf.Max(this.radiusProperty.floatValue, RumbleInfo.MinRadius);
                this.lifetimeProperty.floatValue = Mathf.Max(this.lifetimeProperty.floatValue, RumbleInfo.MinLifetime);
                this.intensityCurvePeriodProperty.floatValue = Mathf.Clamp(
                    this.intensityCurvePeriodProperty.floatValue,
                    RumbleInfo.IntensityCurveSettings.MinPeriod,
                    float.MaxValue);

                // Mark Asset as dirty so that Save will immediately write changes to disk.
                EditorUtility.SetDirty(this.Target);
            }

            // Apply any GUI edited/changed property values back to the asset/object
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDivider()
        {
            GUILayout.Box(string.Empty, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }

        /// <summary>
        /// The Enable message. Called when inspector is deserialized.
        /// </summary>
        protected void OnEnable()
        {
            var intensityCurveSettingsProperty = serializedObject.FindProperty("intensityCurveSettings");

            this.lifetimeProperty = serializedObject.FindProperty("lifetime");
            this.radiusProperty = serializedObject.FindProperty("radius");
            this.intensityModeProperty = serializedObject.FindProperty("intensityMode");
            this.constantIntensityProperty = serializedObject.FindProperty("constantIntensity");
            this.intensityCurveProperty = intensityCurveSettingsProperty.FindPropertyRelative("curve");
            this.isIntensityCurveLoopedProperty = intensityCurveSettingsProperty.FindPropertyRelative("isLooping");
            this.intensityCurvePeriodProperty = intensityCurveSettingsProperty.FindPropertyRelative("period");
            this.falloffFunctionProperty = serializedObject.FindProperty("falloffFunction");

            this.rumbleSettingsProperty = serializedObject.FindProperty("rumbleIntensitySettings");

            // Initialize the animation tracking fields so that you don't see it animate every time you click on a RumbleIntensity.
            var intensityOverLifetime = (RumbleInfo.RumbleIntensityMode)this.intensityModeProperty.enumValueIndex;
            this.constantIntensityAnim.value = intensityOverLifetime == RumbleInfo.RumbleIntensityMode.Constant;
            this.curveIntensityAnim.value = intensityOverLifetime == RumbleInfo.RumbleIntensityMode.Curve;

            this.constantIntensityAnim.valueChanged.AddListener(this.Repaint);
            this.curveIntensityAnim.valueChanged.AddListener(this.Repaint);
        }

        /// <summary>
        /// The disable event.
        /// </summary>
        protected void OnDisable()
        {
            this.constantIntensityAnim.valueChanged.RemoveListener(this.Repaint);
            this.curveIntensityAnim.valueChanged.RemoveListener(this.Repaint);
        }

        private void DrawGUIIntensitySettings()
        {
            EditorGUI.indentLevel++;

            // Set targets for lerped floats that track fade animation
            var intensityOverLifetime = (RumbleInfo.RumbleIntensityMode)this.intensityModeProperty.enumValueIndex;
            this.constantIntensityAnim.target = intensityOverLifetime == RumbleInfo.RumbleIntensityMode.Constant;
            this.curveIntensityAnim.target = intensityOverLifetime == RumbleInfo.RumbleIntensityMode.Curve;

            switch (Target.IntensityOverLifetime)
            {
                case RumbleInfo.RumbleIntensityMode.Constant:
                    EditorGUILayout.BeginFadeGroup(this.constantIntensityAnim.faded);
                    EditorGUILayout.PropertyField(this.constantIntensityProperty, new GUIContent("Intensity"));
                    EditorGUILayout.EndFadeGroup();
                    break;
                case RumbleInfo.RumbleIntensityMode.Curve:
                    this.DrawGUIIntensityCurveSettings();
                    break;
                default:
                    Debug.LogError("Unrecognized RumbleIntensityMode found in RumbleInfoEditor for IntensityOverLifetime. " +
                        "Mode: " + Target.IntensityOverLifetime);
                    break;
            }

            EditorGUI.indentLevel--;
        }

        private void DrawGUIIntensityCurveSettings()
        {
            EditorGUILayout.BeginFadeGroup(this.curveIntensityAnim.faded);

            // Draw info explaining why Looping field is checked
            bool isLifetimeInfinite = float.IsInfinity(this.lifetimeProperty.floatValue);
            if (isLifetimeInfinite)
            {
                EditorGUILayout.HelpBox("IntensityCurve has been auto flagged as looping due to infinite Lifetime.", MessageType.None);
            }

            EditorGUILayout.PropertyField(this.intensityCurveProperty);

            // Draw Looping Field
            bool isLoopingFieldDisabled = isLifetimeInfinite || this.intensityCurveProperty.objectReferenceValue == null;
            if (isLifetimeInfinite)
            {
                // Force Looping set if lifetime is infinite, since an infinite period is invalid
                this.isIntensityCurveLoopedProperty.boolValue = true;
            }

            EditorGUI.BeginDisabledGroup(isLoopingFieldDisabled);
            EditorGUILayout.PropertyField(this.isIntensityCurveLoopedProperty, new GUIContent("Looping"));
            EditorGUI.EndDisabledGroup();

            // Draw Period Field
            bool isLooping = this.isIntensityCurveLoopedProperty.boolValue;
            bool isPeriodFieldDisabled = !isLooping || this.intensityCurveProperty.objectReferenceValue == null;
            EditorGUI.BeginDisabledGroup(isPeriodFieldDisabled);

            var periodGUIContent = new GUIContent("Period");
            if (isLifetimeInfinite)
            {
                // Don't draw a slider for period with Infinite lifetime, since there is no longer a good slider range
                EditorGUILayout.PropertyField(this.intensityCurvePeriodProperty, periodGUIContent);
            }
            else
            {
                EditorGUILayout.Slider(
                    this.intensityCurvePeriodProperty,
                    RumbleInfo.MinLifetime,
                    this.lifetimeProperty.floatValue,
                    periodGUIContent);

                // Always clamp period to lifetime - this fixes Period from exceeding lifetime when setting
                // Lifetime from infinite down to a finite number smaller than the period
                this.intensityCurvePeriodProperty.floatValue = Mathf.Min(
                    this.intensityCurvePeriodProperty.floatValue,
                    this.lifetimeProperty.floatValue);

                // If non-looping, force period to match lifetime
                if (!isLooping)
                {
                    this.intensityCurvePeriodProperty.floatValue = this.lifetimeProperty.floatValue;
                }
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndFadeGroup();
        }
    }
}