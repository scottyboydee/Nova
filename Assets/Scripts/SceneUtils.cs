using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtils
{
    public static void SetSceneHierarchyActive(string sceneName, bool active)
    {
        Debug.Log("SetSceneHierarchyActive: " +  sceneName + " active: "+ active);

        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid())
        {
            Debug.LogError("SetSceneHierarchyActive: invalid scene: " + sceneName);
            return;
        }

        foreach (GameObject go in scene.GetRootGameObjects())
        {
            go.SetActive(active);
        }
    }
}
