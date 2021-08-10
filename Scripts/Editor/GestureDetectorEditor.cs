using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

namespace DoubTech.ValemGestures
{
    [CustomEditor(typeof(GestureDetector))]
    public class GestureDetectorEditor : Editor
    {
        private string name;

        class Content
        {
            #region GUIContent
            #endregion

            #region Styles
            #endregion

            static Content()
            {
            // Style initialization

            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gesture = ((GestureDetector) target);
            GUILayout.BeginHorizontal();
            gesture.gestureName = EditorGUILayout.TextField("Gesture Name", gesture.gestureName);
            if (GUILayout.Button("Recognize", GUILayout.Width(75)))
            {
                gesture.RecordGesture(name);
            }
            GUILayout.EndHorizontal();
        }
    }
}
