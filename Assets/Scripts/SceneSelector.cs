using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    [HideInInspector]
    public int selectedIndex = 0;
    [HideInInspector]
    public List<string> sceneList = new List<string>();

    public string GetSelectedScene()
    {
        return sceneList[selectedIndex];
    }
}
