using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(ContextMenu))]
public class ContextMenuInspector : Editor
{
    private ReorderableList reorderableList;


    private ContextMenu contextMenu
    {
        get
        {
            return target as ContextMenu;
        }
    }

    private void OnEnable()
    {
        //reorderableList = new ReorderableList(contextMenu.menuItems, typeof(ContextMenuItem), true, true, true, true);

        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("menuItems"), true, true, true, true);

        // Add listeners to draw events
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;
        reorderableList.elementHeightCallback += ElementHeight;

        //reorderableList.onAddCallback += AddItem;
        //reorderableList.onAddDropdownCallback += AddDropdown;
        //reorderableList.onRemoveCallback += RemoveItem;

    }

    private float ElementHeight(int index)
    {
        return EditorGUI.GetPropertyHeight(serializedObject.FindProperty("menuItems").GetArrayElementAtIndex(index));
    }

    private void OnDisable()
    {
        // Make sure we don't get memory leaks etc.
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;
        reorderableList.elementHeightCallback -= ElementHeight;
        //reorderableList.onAddCallback -= AddItem;
        //reorderableList.onRemoveCallback -= RemoveItem;
    }

    /// <summary>
    /// Draws the header of the list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Menu Items");
    }

    /// <summary>
    /// Draws one element of the list (ListItemExample)
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        //PropTest item = list.tutorials[index];

        EditorGUI.BeginChangeCheck();
        //EditorGUI.PropertyField(rect, )
        /*item.boolValue = EditorGUI.Toggle(new Rect(rect.x, rect.y, 18, rect.height), item.boolValue);
        item.stringvalue = EditorGUI.TextField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item.stringvalue);*/
        var element = serializedObject.FindProperty("menuItems").GetArrayElementAtIndex(index);

        EditorGUI.indentLevel = 1;

        if (!element.isExpanded)
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 18, rect.height), element, new GUIContent(contextMenu.menuItems[index].name), true);
        else
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, new GUIContent(contextMenu.menuItems[index].name), true);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);

        }

        // If you are using a custom PropertyDrawer, this is probably better
        //EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }

   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }



   /* private void RemoveItem(ReorderableList list)
    {
        this.contextMenu.menuItems.RemoveAt(list.index);

        EditorUtility.SetDirty(target);
    }*/

}

