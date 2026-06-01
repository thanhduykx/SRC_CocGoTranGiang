using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BachDang938SceneBuilder
{
    private const string SceneFolder = "Assets/Scenes/BachDang938";
    private const string NgoQuyenAssetPath = "Assets/Characters/NgoQuyenChar.glb";

    [MenuItem("Tools/Bach Dang 938/Create Related Scenes")]
    public static void CreateRelatedScenes()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        EnsureSceneFolder();

        CreatePreparationScene();
        CreateRiverTrapScene();
        CreateBattleScene();
        CreateVictoryScene();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (!Application.isBatchMode)
        {
            EditorUtility.DisplayDialog(
                "Bach Dang 938",
                "Created 4 related scenes in Assets/Scenes/BachDang938.",
                "OK");
        }
    }

    [MenuItem("Tools/Bach Dang 938/Setup Current Scene Around NgoQuyenChar")]
    public static void SetupCurrentSceneAroundNgoQuyenChar()
    {
        GameObject existingRoot = GameObject.Find("Bach Dang 938 Current Scene Set");
        if (existingRoot != null)
        {
            if (!EditorUtility.DisplayDialog(
                    "Bach Dang 938",
                    "A Bach Dang environment already exists in this scene. Replace it?",
                    "Replace",
                    "Cancel"))
            {
                return;
            }

            Undo.DestroyObjectImmediate(existingRoot);
        }

        GameObject root = new GameObject("Bach Dang 938 Current Scene Set");
        Undo.RegisterCreatedObjectUndo(root, "Create Bach Dang 938 Current Scene Set");

        SetupCurrentSceneCameraAndLight();
        CreateCurrentSceneRiver(root.transform);
        CreateCurrentSceneStakes(root.transform);
        CreateCurrentSceneBoats(root.transform);
        CreateCurrentSceneSoldiers(root.transform);
        CreateCurrentSceneDecor(root.transform);
        PlaceNgoQuyenInCurrentScene();

        Selection.activeGameObject = root;
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        SceneView.lastActiveSceneView?.FrameSelected();
    }

    [MenuItem("Tools/Bach Dang 938/Create Scene 01 - Preparation")]
    public static void CreatePreparationScene()
    {
        Scene scene = BeginScene("01_BoiCanh_ChuanBi");
        SetupLighting(new Color(0.68f, 0.82f, 0.93f));
        CreateCamera(new Vector3(0f, 9f, -14f), new Vector3(50f, 0f, 0f));

        CreateGround("Village Ground", Vector3.zero, new Vector3(18f, 0.2f, 18f), new Color(0.28f, 0.48f, 0.23f));
        CreateRiverStrip(new Vector3(0f, 0.02f, 6.8f), new Vector3(18f, 0.08f, 3f), new Color(0.09f, 0.33f, 0.52f));

        for (int i = 0; i < 5; i++)
        {
            CreateTent(new Vector3(-5.5f + i * 2.4f, 0.35f, -2.6f + (i % 2) * 1.4f));
        }

        SetCharacterAction(
            CreateCharacter("NgoQuyenChar", new Vector3(0f, 0.75f, -4.8f), new Color(0.72f, 0.08f, 0.05f), true),
            NgoQuyenCharActions.ActionState.Command);

        for (int i = 0; i < 8; i++)
        {
            float x = -6f + i * 1.7f;
            CreateCharacter("Soldier", new Vector3(x, 0.55f, -0.4f + (i % 2) * 0.9f), new Color(0.55f, 0.09f, 0.06f), false);
        }

        CreateWoodPile(new Vector3(4.7f, 0.35f, 1.8f));
        CreateLabel("Canh 1: Ngo Quyen tap hop luc luong va chuan bi bai coc", new Vector3(0f, 2.4f, -6.8f), 0.22f);
        SaveScene(scene, "01_BoiCanh_ChuanBi");
    }

    [MenuItem("Tools/Bach Dang 938/Create Scene 02 - River Trap")]
    public static void CreateRiverTrapScene()
    {
        Scene scene = BeginScene("02_SongBachDang_BaiCoc");
        SetupLighting(new Color(0.62f, 0.78f, 0.9f));
        CreateCamera(new Vector3(0f, 13f, -16f), new Vector3(58f, 0f, 0f));

        CreateRiverEnvironment(lowTide: false);
        CreateStakeField(hidden: true);
        SetCharacterAction(
            CreateCharacter("NgoQuyenChar", new Vector3(-5.2f, 0.75f, -2.5f), new Color(0.72f, 0.08f, 0.05f), true),
            NgoQuyenCharActions.ActionState.Command);

        CreateLabel("Canh 2: Nuoc trieu len che kin bai coc tren song Bach Dang", new Vector3(0f, 2.2f, -7.2f), 0.21f);
        CreateLabel("Bai coc ngam", new Vector3(0f, 1.15f, 0.4f), 0.18f);
        SaveScene(scene, "02_SongBachDang_BaiCoc");
    }

    [MenuItem("Tools/Bach Dang 938/Create Scene 03 - Battle")]
    public static void CreateBattleScene()
    {
        Scene scene = BeginScene("03_GiaoChien_ThuyTrieuRut");
        SetupLighting(new Color(0.52f, 0.63f, 0.72f));
        CreateCamera(new Vector3(0f, 12f, -14f), new Vector3(56f, 0f, 0f));

        CreateRiverEnvironment(lowTide: true);
        CreateStakeField(hidden: false);

        for (int i = 0; i < 5; i++)
        {
            CreateBoat(new Vector3(-2.6f + i * 1.25f, 0.35f, -0.3f + i * 0.32f), stuck: true);
        }

        GameObject commander = CreateCharacter("NgoQuyenChar", new Vector3(-5f, 0.75f, -2.2f), new Color(0.72f, 0.08f, 0.05f), true);
        SetCharacterAction(commander, NgoQuyenCharActions.ActionState.Attack);

        for (int i = 0; i < 10; i++)
        {
            float side = i % 2 == 0 ? -1f : 1f;
            CreateCharacter("Ambush Soldier", new Vector3(side * (4.2f + i * 0.14f), 0.55f, -3f + i * 0.68f), new Color(0.62f, 0.06f, 0.04f), false);
        }

        CreateLabel("Canh 3: Thuy trieu rut, thuyen Nam Han mac coc, quan ta phan cong", new Vector3(0f, 2.25f, -7f), 0.2f);
        SaveScene(scene, "03_GiaoChien_ThuyTrieuRut");
    }

    [MenuItem("Tools/Bach Dang 938/Create Scene 04 - Victory")]
    public static void CreateVictoryScene()
    {
        Scene scene = BeginScene("04_ChienThang_DocLap");
        SetupLighting(new Color(0.78f, 0.88f, 0.96f));
        CreateCamera(new Vector3(0f, 8.5f, -12f), new Vector3(48f, 0f, 0f));

        CreateGround("Victory Ground", Vector3.zero, new Vector3(18f, 0.2f, 18f), new Color(0.3f, 0.52f, 0.25f));
        CreateRiverStrip(new Vector3(0f, 0.02f, 5.8f), new Vector3(18f, 0.08f, 3f), new Color(0.1f, 0.38f, 0.58f));
        CreateMonument(new Vector3(0f, 0.55f, -1.4f));

        GameObject commander = CreateCharacter("NgoQuyenChar", new Vector3(0f, 0.8f, -3.5f), new Color(0.72f, 0.08f, 0.05f), true);
        SetCharacterAction(commander, NgoQuyenCharActions.ActionState.Victory);

        for (int i = 0; i < 12; i++)
        {
            float x = -6.4f + i * 1.15f;
            CreateCharacter("Victory Soldier", new Vector3(x, 0.55f, -0.1f + Mathf.Abs(i - 6) * 0.18f), new Color(0.56f, 0.08f, 0.05f), false);
        }

        CreateLabel("Canh 4: Chien thang Bach Dang 938 mo ra ky nguyen doc lap", new Vector3(0f, 2.35f, -6.8f), 0.21f);
        SaveScene(scene, "04_ChienThang_DocLap");
    }

    private static Scene BeginScene(string sceneName)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = sceneName;
        return scene;
    }

    private static void SaveScene(Scene scene, string sceneName)
    {
        EnsureSceneFolder();
        EditorSceneManager.SaveScene(scene, $"{SceneFolder}/{sceneName}.unity");
    }

    private static void EnsureSceneFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }

        if (!AssetDatabase.IsValidFolder(SceneFolder))
        {
            AssetDatabase.CreateFolder("Assets/Scenes", "BachDang938");
        }
    }

    private static void SetupLighting(Color skyColor)
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.62f, 0.62f, 0.62f);

        GameObject sunObject = new GameObject("Directional Sun");
        Light sun = sunObject.AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.25f;
        sunObject.transform.rotation = Quaternion.Euler(48f, -32f, 8f);

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.backgroundColor = skyColor;
        }
    }

    private static void SetupCurrentSceneCameraAndLight()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            camera = cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
        }

        Undo.RecordObject(camera.transform, "Position Bach Dang Camera");
        camera.transform.position = new Vector3(0f, 9f, -13f);
        camera.transform.rotation = Quaternion.Euler(52f, 0f, 0f);
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.62f, 0.78f, 0.9f);

        Light light = Object.FindFirstObjectByType<Light>();
        if (light == null)
        {
            GameObject lightObject = new GameObject("Directional Light");
            light = lightObject.AddComponent<Light>();
        }

        Undo.RecordObject(light.transform, "Position Bach Dang Light");
        light.type = LightType.Directional;
        light.intensity = 1.25f;
        light.transform.rotation = Quaternion.Euler(48f, -32f, 8f);
    }

    private static void CreateCurrentSceneRiver(Transform root)
    {
        CreatePrimitive(PrimitiveType.Cube, "Left River Bank", root, new Vector3(-7.5f, -0.12f, 0f), new Vector3(7f, 0.25f, 20f), new Color(0.25f, 0.45f, 0.2f));
        CreatePrimitive(PrimitiveType.Cube, "Right River Bank", root, new Vector3(7.5f, -0.12f, 0f), new Vector3(7f, 0.25f, 20f), new Color(0.24f, 0.42f, 0.18f));
        CreatePrimitive(PrimitiveType.Cube, "Bach Dang River - Low Tide", root, new Vector3(0f, -0.24f, 0f), new Vector3(7.1f, 0.12f, 20f), new Color(0.05f, 0.22f, 0.34f));
        CreatePrimitive(PrimitiveType.Cube, "Muddy River Bed", root, new Vector3(0f, -0.34f, -1.2f), new Vector3(6.7f, 0.08f, 5.8f), new Color(0.28f, 0.22f, 0.15f));
    }

    private static void CreateCurrentSceneStakes(Transform root)
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                GameObject stake = CreatePrimitive(
                    PrimitiveType.Cylinder,
                    "Exposed Wooden Stake",
                    root,
                    new Vector3(-3.2f + col * 0.92f + (row % 2) * 0.35f, 0.08f, -2.2f + row * 0.82f),
                    new Vector3(0.12f, 1.05f, 0.12f),
                    new Color(0.32f, 0.17f, 0.07f));
                stake.transform.rotation = Quaternion.Euler(-15f, 0f, 0f);
            }
        }
    }

    private static void CreateCurrentSceneBoats(Transform root)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject boat = new GameObject("Nam Han Boat Stuck On Stakes");
            Undo.RegisterCreatedObjectUndo(boat, "Create Nam Han Boat");
            boat.transform.SetParent(root);
            boat.transform.position = new Vector3(-2.6f + i * 1.25f, 0.25f, -0.5f + i * 0.35f);
            boat.transform.rotation = Quaternion.Euler(8f, 0f, i % 2 == 0 ? -12f : 10f);

            CreatePrimitive(PrimitiveType.Cube, "Hull", boat.transform, Vector3.zero, new Vector3(1.2f, 0.25f, 2f), new Color(0.42f, 0.18f, 0.1f));
            CreatePrimitive(PrimitiveType.Cube, "Sail", boat.transform, new Vector3(0.18f, 0.78f, 0f), new Vector3(0.06f, 0.8f, 0.65f), new Color(0.9f, 0.83f, 0.63f));
        }
    }

    private static void CreateCurrentSceneSoldiers(Transform root)
    {
        for (int i = 0; i < 12; i++)
        {
            float side = i % 2 == 0 ? -1f : 1f;
            GameObject soldier = CreatePrimitive(
                PrimitiveType.Capsule,
                "Ngo Quyen Soldier Ambush",
                root,
                new Vector3(side * (4.1f + i * 0.08f), 0.48f, -4f + i * 0.62f),
                new Vector3(0.3f, 0.48f, 0.3f),
                new Color(0.6f, 0.06f, 0.04f));
            soldier.transform.rotation = Quaternion.Euler(0f, side < 0f ? 70f : -70f, 0f);
        }
    }

    private static void CreateCurrentSceneDecor(Transform root)
    {
        CreateCurrentSceneLabel(root, "Bach Dang 938 - thuy trieu rut, thuyen giac mac coc", new Vector3(0f, 2.2f, -7.5f), 0.2f);
        CreateCurrentSceneLabel(root, "Bai coc ngam lo ra", new Vector3(0f, 1.1f, -1.2f), 0.16f);

        for (int i = 0; i < 12; i++)
        {
            float x = i % 2 == 0 ? -4.7f : 4.7f;
            Vector3 pos = new Vector3(x, 0.35f, -8f + i * 1.35f);
            GameObject trunk = CreatePrimitive(PrimitiveType.Cylinder, "Tree Trunk", root, pos, new Vector3(0.16f, 0.75f, 0.16f), new Color(0.27f, 0.14f, 0.06f));
            CreatePrimitive(PrimitiveType.Sphere, "Tree Crown", root, pos + Vector3.up * 1.05f, Vector3.one * 0.95f, new Color(0.16f, 0.38f, 0.14f));
        }
    }

    private static void PlaceNgoQuyenInCurrentScene()
    {
        GameObject ngoQuyen = GameObject.Find("NgoQuyenChar");
        if (ngoQuyen == null)
        {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(NgoQuyenAssetPath);
            ngoQuyen = asset != null ? PrefabUtility.InstantiatePrefab(asset) as GameObject : GameObject.CreatePrimitive(PrimitiveType.Capsule);
            ngoQuyen.name = "NgoQuyenChar";
            Undo.RegisterCreatedObjectUndo(ngoQuyen, "Create NgoQuyenChar");
        }
        else
        {
            Undo.RecordObject(ngoQuyen.transform, "Place NgoQuyenChar");
        }

        ngoQuyen.transform.position = new Vector3(-4.8f, 0.1f, -3f);
        ngoQuyen.transform.rotation = Quaternion.Euler(0f, 65f, 0f);
        ngoQuyen.transform.localScale = Vector3.one;

        NgoQuyenCharActions actions = ngoQuyen.GetComponent<NgoQuyenCharActions>();
        if (actions == null)
        {
            actions = Undo.AddComponent<NgoQuyenCharActions>(ngoQuyen);
        }

        actions.SetStartAction(NgoQuyenCharActions.ActionState.Command);
        EditorUtility.SetDirty(actions);
    }

    private static GameObject CreatePrimitive(PrimitiveType type, string objectName, Transform parent, Vector3 position, Vector3 scale, Color color)
    {
        GameObject gameObject = GameObject.CreatePrimitive(type);
        Undo.RegisterCreatedObjectUndo(gameObject, $"Create {objectName}");
        gameObject.name = objectName;
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = position;
        gameObject.transform.localScale = scale;
        SetColor(gameObject, color);
        return gameObject;
    }

    private static void CreateCurrentSceneLabel(Transform root, string text, Vector3 position, float characterSize)
    {
        GameObject label = new GameObject("Scene Label");
        Undo.RegisterCreatedObjectUndo(label, "Create Scene Label");
        label.transform.SetParent(root);
        label.transform.position = position;
        label.transform.rotation = Quaternion.Euler(62f, 0f, 0f);
        TextMesh mesh = label.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.fontSize = 64;
        mesh.characterSize = characterSize;
        mesh.color = Color.black;
    }

    private static void CreateCamera(Vector3 position, Vector3 euler)
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.66f, 0.82f, 0.94f);
        cameraObject.transform.position = position;
        cameraObject.transform.rotation = Quaternion.Euler(euler);
    }

    private static void CreateRiverEnvironment(bool lowTide)
    {
        CreateGround("Left Bank", new Vector3(-7.4f, -0.15f, 0f), new Vector3(7.2f, 0.3f, 18f), new Color(0.25f, 0.45f, 0.2f));
        CreateGround("Right Bank", new Vector3(7.4f, -0.15f, 0f), new Vector3(7.2f, 0.3f, 18f), new Color(0.24f, 0.42f, 0.18f));
        CreateRiverStrip(new Vector3(0f, lowTide ? -0.28f : 0.04f, 0f), new Vector3(7.2f, 0.12f, 18f), lowTide ? new Color(0.05f, 0.22f, 0.34f) : new Color(0.1f, 0.38f, 0.6f));

        for (int i = 0; i < 12; i++)
        {
            float x = i % 2 == 0 ? -4.4f : 4.4f;
            CreateTree(new Vector3(x + Random.Range(-0.4f, 0.4f), 0.35f, -7.5f + i * 1.3f));
        }
    }

    private static void CreateStakeField(bool hidden)
    {
        float y = hidden ? -0.42f : 0.05f;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Transform stake = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
                stake.name = hidden ? "Hidden Stake" : "Exposed Stake";
                stake.position = new Vector3(-3.1f + col * 0.9f + (row % 2) * 0.32f, y, -2.2f + row * 0.78f);
                stake.rotation = Quaternion.Euler(-15f, 0f, 0f);
                stake.localScale = new Vector3(0.12f, hidden ? 0.65f : 1.05f, 0.12f);
                SetColor(stake.gameObject, new Color(0.32f, 0.17f, 0.07f));
            }
        }
    }

    private static GameObject CreateCharacter(string objectName, Vector3 position, Color color, bool addActions)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(NgoQuyenAssetPath);
        GameObject character = asset != null && objectName.Contains("NgoQuyen")
            ? PrefabUtility.InstantiatePrefab(asset) as GameObject
            : null;

        if (character == null)
        {
            character = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        }

        character.name = objectName;
        character.transform.position = position;
        character.transform.localScale = objectName.Contains("NgoQuyen") ? new Vector3(0.55f, 0.75f, 0.55f) : new Vector3(0.32f, 0.48f, 0.32f);
        SetColor(character, color);

        if (addActions && character.GetComponent<NgoQuyenCharActions>() == null)
        {
            character.AddComponent<NgoQuyenCharActions>();
        }

        return character;
    }

    private static void SetCharacterAction(GameObject character, NgoQuyenCharActions.ActionState actionState)
    {
        NgoQuyenCharActions actions = character.GetComponent<NgoQuyenCharActions>();
        if (actions == null)
        {
            actions = character.AddComponent<NgoQuyenCharActions>();
        }

        actions.SetStartAction(actionState);
        EditorUtility.SetDirty(actions);
    }

    private static void CreateBoat(Vector3 position, bool stuck)
    {
        GameObject root = new GameObject(stuck ? "Stuck Nam Han Boat" : "Nam Han Boat");
        root.transform.position = position;
        root.transform.rotation = Quaternion.Euler(stuck ? 8f : 0f, 0f, stuck ? -12f : 0f);

        GameObject hull = CreateGround("Hull", Vector3.zero, new Vector3(1.2f, 0.25f, 2f), new Color(0.42f, 0.18f, 0.1f));
        hull.transform.SetParent(root.transform, false);

        GameObject sail = CreateGround("Sail", new Vector3(0.18f, 0.78f, 0f), new Vector3(0.06f, 0.8f, 0.65f), new Color(0.9f, 0.83f, 0.63f));
        sail.transform.SetParent(root.transform, false);
    }

    private static void CreateTent(Vector3 position)
    {
        GameObject tent = CreateGround("Soldier Tent", position, new Vector3(1.5f, 0.7f, 1.2f), new Color(0.54f, 0.35f, 0.18f));
        tent.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
    }

    private static void CreateWoodPile(Vector3 position)
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject log = CreateGround("Wood For Stakes", position + new Vector3(0f, i * 0.13f, (i % 2) * 0.2f), new Vector3(1.6f, 0.12f, 0.12f), new Color(0.35f, 0.18f, 0.07f));
            log.transform.rotation = Quaternion.Euler(0f, i % 2 == 0 ? 12f : -10f, 0f);
        }
    }

    private static void CreateTree(Vector3 position)
    {
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.name = "Tree Trunk";
        trunk.transform.position = position;
        trunk.transform.localScale = new Vector3(0.18f, 0.8f, 0.18f);
        SetColor(trunk, new Color(0.27f, 0.14f, 0.06f));

        GameObject crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        crown.name = "Tree Crown";
        crown.transform.position = position + Vector3.up * 1.15f;
        crown.transform.localScale = new Vector3(1f, 1f, 1f);
        SetColor(crown, new Color(0.16f, 0.38f, 0.14f));
    }

    private static void CreateMonument(Vector3 position)
    {
        CreateGround("Monument Base", position, new Vector3(2.4f, 0.55f, 1.2f), new Color(0.55f, 0.55f, 0.5f));
        CreateGround("Victory Stone", position + Vector3.up * 1f, new Vector3(1.2f, 1.5f, 0.35f), new Color(0.62f, 0.62f, 0.58f));
        CreateLabel("Bach Dang 938", position + new Vector3(0f, 1.25f, -0.25f), 0.14f);
    }

    private static GameObject CreateGround(string objectName, Vector3 position, Vector3 scale, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = objectName;
        cube.transform.position = position;
        cube.transform.localScale = scale;
        SetColor(cube, color);
        return cube;
    }

    private static GameObject CreateRiverStrip(Vector3 position, Vector3 scale, Color color)
    {
        return CreateGround("Bach Dang River", position, scale, color);
    }

    private static void CreateLabel(string text, Vector3 position, float characterSize)
    {
        GameObject label = new GameObject("Scene Label");
        label.transform.position = position;
        label.transform.rotation = Quaternion.Euler(62f, 0f, 0f);
        TextMesh mesh = label.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.fontSize = 64;
        mesh.characterSize = characterSize;
        mesh.color = Color.black;
    }

    private static void SetColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            renderer.sharedMaterial.color = color;
        }
    }
}
