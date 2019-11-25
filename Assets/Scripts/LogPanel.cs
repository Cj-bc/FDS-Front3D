using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    private FaceData faceData;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        faceData = GameObject.FindObjectOfType<FaceData>() as FaceData;
        text = GetComponent<Text>();
        text.text = "X: 1.0, Y: 0.0, Z: 0.0";
    }

    // Update is called once per frame
    void Update()
    {
        Text text = GetComponent<Text>();
        text.text = faceData.currentFaceData.ToString();
    }
}
