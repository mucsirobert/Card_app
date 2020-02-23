 using UnityEditor;
 using UnityEngine.UI;

[CustomEditor(typeof(IconToggle))]
public class IconToggleEditor : UnityEditor.UI.ToggleEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IconToggle targetToggle = (IconToggle)target;

        targetToggle.icon = (Image)EditorGUILayout.ObjectField("Icon:", targetToggle.icon, typeof(Image), true);
        targetToggle.normalIconColor = EditorGUILayout.ColorField("Normal icon color", targetToggle.normalIconColor);
        targetToggle.pressedIconColor = EditorGUILayout.ColorField("Pressed icon color", targetToggle.pressedIconColor);
        targetToggle.disabledIconColor = EditorGUILayout.ColorField("Disabled icon color", targetToggle.disabledIconColor);
    }
}
