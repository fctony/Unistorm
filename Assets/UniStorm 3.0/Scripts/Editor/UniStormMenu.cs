using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UniStormMenu : MonoBehaviour {

    [MenuItem("Window/UniStorm/Create Weather System", false, 100)]
    static void InstantiateUniStorm()
    {
        Selection.activeObject = SceneView.currentDrawingSceneView;

        GameObject codeInstantiatedPrefab = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/UniStorm 3.0/Prefabs/System/UniStorm System.prefab", typeof(GameObject))) as GameObject;

        codeInstantiatedPrefab.name = "UniStorm System";
        codeInstantiatedPrefab.transform.position = new Vector3(0, 0, 0);
        Selection.activeGameObject = codeInstantiatedPrefab;
    }

    [MenuItem("Window/UniStorm/Documentation", false, 200)]
    static void Documentation()
    {
        Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit");
    }

    [MenuItem("Window/UniStorm/UniStorm API", false, 200)]
    static void UniStormAPI()
    {
        Application.OpenURL("https://docs.google.com/document/d/1uL_oMqHC_EduRGEnOihifwcpkQmWX9rubGw8qjkZ4b4/edit#heading=h.wdi1uoeswdyw");
    }

    [MenuItem("Window/UniStorm/Report a Bug or Request a Feature", false, 200)]
    static void ReportABug()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfx-7HTghi6ZUNaaat2pMXH46d71X6FhSnJPq6mr8uHRbXpwQ/viewform?usp=sf_link");
    }
}
