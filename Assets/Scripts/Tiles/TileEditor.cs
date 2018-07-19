using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TileEditor : MonoBehaviour {

    [SerializeField, HideInInspector]
    public Tile.Tile_Type editor_type = Tile.Tile_Type.FLATLAND;
    [SerializeField, HideInInspector]
    public Player.Type editor_owner = Player.Type.NONE;

    [HideInInspector]
    public bool ready_to_delete = false;

    private void Update()
    {
        if (ready_to_delete && Selection.activeGameObject != gameObject)
            DestroyImmediate(gameObject);
    }
}

[CustomEditor(typeof(TileEditor)), CanEditMultipleObjects]
public class TileEditorGUI : Editor
{
    SerializedProperty s_type;
    SerializedProperty s_owner;

    void OnEnable()
    {
        s_type = serializedObject.FindProperty("editor_type");
        s_owner = serializedObject.FindProperty("editor_owner");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(s_type, new GUIContent("Tile Type"));
        EditorGUILayout.PropertyField(s_owner, new GUIContent("Owner"));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Update Tile"))
        {
            foreach (Object tar in targets)
            {
                Tile tile = null;
                TileEditor editor = tar as TileEditor;
                if (editor) tile = editor.GetComponent<Tile>();
                if (tile) tile.ChangeTile(editor.editor_type, editor.editor_owner);
                editor.ready_to_delete = true;
            }
            Selection.activeObject = null;
        }
    }
}