using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BachDang938DemoMenu
{
    [MenuItem("Tools/Bach Dang 938/Add Actions To Selected NgoQuyenChar")]
    public static void AddActionsToSelectedNgoQuyenChar()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogWarning("Select NgoQuyenChar in the Project window or Hierarchy first.");
            return;
        }

        if (EditorUtility.IsPersistent(selected))
        {
            GameObject instance = PrefabUtility.InstantiatePrefab(selected) as GameObject;
            if (instance == null)
            {
                instance = Object.Instantiate(selected);
            }

            instance.name = selected.name;
            Undo.RegisterCreatedObjectUndo(instance, "Create Ngo Quyen Character");
            selected = instance;
        }

        Undo.RecordObject(selected, "Add NgoQuyenCharActions");
        NgoQuyenCharActions actions = selected.GetComponent<NgoQuyenCharActions>();
        if (actions == null)
        {
            actions = Undo.AddComponent<NgoQuyenCharActions>(selected);
        }

        EditorUtility.SetDirty(actions);
        Selection.activeGameObject = selected;
        Debug.Log($"Added NgoQuyenCharActions to {selected.name}. Press Play, then use keys 1-5 to preview actions.");
    }

    [MenuItem("Tools/Bach Dang 938/Create Demo Scene")]
    public static void CreateDemoScene()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var demoRoot = new GameObject("Bach Dang 938 Demo");
        demoRoot.AddComponent<BachDang938Demo>();

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/BachDang938Demo.unity");
        Selection.activeGameObject = demoRoot;
    }
}
