using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[CustomEditor(typeof(SceneSelector))]
public class SceneDropdown : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneSelector dropdown = (SceneSelector)target;
        if (dropdown.sceneList.Count < SceneManager.sceneCountInBuildSettings)
        {
            FillList(dropdown);
        }

        GUIContent componentsList = new GUIContent("List");
        dropdown.selectedIndex = EditorGUILayout.Popup(componentsList, dropdown.selectedIndex, dropdown.sceneList.ToArray());

        //Debug.Log(dropdown.sceneList[dropdown.selectedIndex]);
    }
    public void FillList(SceneSelector dropdown)
    {
        dropdown.sceneList.Clear();
        foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                dropdown.sceneList.Add(name);
            }
        }
    }
}
