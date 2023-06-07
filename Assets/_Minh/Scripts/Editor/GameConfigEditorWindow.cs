using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameConfigEditorWindow : OdinMenuEditorWindow {
    [MenuItem("Tools/Editor Windows/Game Config Editor Window")]
    private static void Open() {
        GameConfigEditorWindow window = GetWindow<GameConfigEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
    }
    protected override OdinMenuTree BuildMenuTree() {
        OdinMenuTree tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;
        tree.Add("Global Config/Stat Cost Config", StatCostConfig.Instance);
        return tree;
    }
    protected override void OnBeginDrawEditors() {
        //base.OnBeginDrawEditors();
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save Data"))) {
                EditorUtility.SetDirty(StatCostConfig.Instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
    static private T[] GetAllRoomDataAssets<T>(int world) where T : ScriptableObject {
        string foderPath = "Assets/AssetDatas/Rooms/W" + world;
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { foderPath });
        T[] dataAssets = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            dataAssets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return dataAssets;
    }
}
