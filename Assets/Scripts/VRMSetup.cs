using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using VRM;
using VRMLoader;

public class VRMSetup : MonoBehaviour
{
    [SerializeField, Header("GUI")]
    Canvas m_canvas;

    [SerializeField]
    GameObject m_modalWindowPrefab;

    [SerializeField]
    Dropdown m_language;

    VRMImporterContext m_context;

    GameObject CurrentCamera;

    GameObject loadedVRM = null;

    public RuntimeAnimatorController VRMdefaltAnimationController;

    // Start is called before the first frame update
    void Start()
    {
        CurrentCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        GUI.matrix = Matrix4x4.TRS(
              Vector3.zero
            , Quaternion.identity
            , new Vector3(Screen.width/480f, Screen.height/360f, 1f)
            );

        if(GUI.Button(new Rect(10, 10, 50, 20), "Load VRM")) {
            if(loadedVRM != null)
                UnloadCurrentVRM();

            string[] pathes = StandaloneFileBrowser.OpenFilePanel("Load VRM", "", "vrm", false);
            Debug.Log("Pathes returned");
            SetupVRMLoaderUI(pathes);
        };
    }


    /// <summary>Create VRMLoaderUI and set callback</summary>
    ///
    // Those codes are imported from VRMLoaderUI sample
    // https://github.com/m2wasabi/VRMLoaderUI/blob/master/Assets/VRMLoaderUI/Example/Scripts/ModelLoaderLegacy.cs
    void SetupVRMLoaderUI(string[] pathes) {
        string path = "file:///" + pathes[0];

        // Load file
        var www = new WWW(path);

        m_context = new VRMImporterContext();
        m_context.ParseGlb(www.bytes);
        var meta = m_context.ReadMeta(true);

        // instantinate UI
        GameObject modalObject = Instantiate(m_modalWindowPrefab, m_canvas.transform) as GameObject;

        // // determine language
        // var modalLocale = modalObject.GetComponentInChildren<VRMPreviewLocale>();
        // modalLocale.SetLocale(m_language.captionText.text);

        // input VRM meta information to UI
        var modalUI = modalObject.GetComponentInChildren<VRMPreviewUI>();
        modalUI.setMeta(meta);

        // Permission of file open
        modalUI.setLoadable(true);

        // define callback on load completed
        modalUI.m_ok.onClick.AddListener(ModelLoad);
    }


    /// <summary>Initialize model's gameobject (i.e. attaching assets, etc)</summary>
    private void ModelLoad() {
        m_context.LoadAsync(_ =>
            {
                m_context.ShowMeshes();
                GameObject root = m_context.Root;
                loadedVRM = root;

                SetupVRM();
                SetupCamera();
            }, Debug.LogError);

    }


    /// <summary>Setup loaded VRM, i.e. attaching assets</summary>
    void SetupVRM() {
                loadedVRM.AddComponent<Cjbc.FaceDataServer.Unity.ApplyFaceDataToVRM>();
                loadedVRM.SetActive(true);
                Debug.Log("Get Aminmator component");
                loadedVRM.GetComponent<Animator>().runtimeAnimatorController = VRMdefaltAnimationController;
                Debug.Log("controller set");
    }




    /// <summary>Move camera to aim VRM's head line</summary>
    void SetupCamera() {
        Transform Head = loadedVRM.transform
                                  .Find("Root")
                                  .Find("J_Bip_C_Hips")
                                  .Find("J_Bip_C_Spine")
                                  .Find("J_Bip_C_Chest")
                                  .Find("J_Bip_C_UpperChest")
                                  .Find("J_Bip_C_Neck")
                                  .Find("J_Bip_C_Head");
        Vector3 cpos = new Vector3(CurrentCamera.transform.position.x
                                  , Head.position.y
                                  , CurrentCamera.transform.position.z);
        CurrentCamera.transform.position = cpos;
    }

    /// <summary>Unload and destroy currently loaded VRM</summary>
    void UnloadCurrentVRM() {
        Destroy(loadedVRM);
        loadedVRM = null;
    }

}
