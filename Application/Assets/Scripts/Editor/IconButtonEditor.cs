 using UnityEditor;
 using UnityEngine.UI;

[CustomEditor(typeof(IconButton))]
public class IconButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IconButton targetButton = (IconButton)target;

        targetButton.icon = (Image)EditorGUILayout.ObjectField("Icon:", targetButton.icon, typeof(Image), true);
        targetButton.normalIconColor = EditorGUILayout.ColorField("Normal icon color", targetButton.normalIconColor);
        targetButton.pressedIconColor = EditorGUILayout.ColorField("Pressed icon color", targetButton.pressedIconColor);
        targetButton.disabledIconColor = EditorGUILayout.ColorField("Disabled icon color", targetButton.disabledIconColor);
    }
}
