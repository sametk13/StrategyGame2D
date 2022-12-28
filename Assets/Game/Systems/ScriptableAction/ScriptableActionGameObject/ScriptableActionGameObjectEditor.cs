using UnityEditor;
using UnityEngine;

namespace SKUtils.ScriptableSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableActionGameObject))]
    public class ScriptableActionGameObjectEditor : Editor
    {
        BuildingData actionGameObject;
        public override void OnInspectorGUI()
        {
            actionGameObject = (BuildingData)EditorGUILayout.ObjectField("Action Game Object", actionGameObject,typeof(BuildingData), true);

            ScriptableActionGameObject action = (ScriptableActionGameObject)target; // Parsing out the value that has been transferred

            if (GUILayout.Button("Call Action")) // Turning the context menu 'call action' into a button
            {
                action.CallAction(actionGameObject);
            }
        }
    }
#endif
}
