using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LookAtObject should be child of VRM model
public class LookAtObject : MonoBehaviour
{
    private FaceData faceData;
    private Transform vrm;

    void Start()
    {
        // TODO: Change parent to VRM here in the future
        faceData = GameObject.FindObjectOfType<FaceData>() as FaceData;
        vrm = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(vrm.position, Vector3.right,faceData.currentFaceData.x);
        transform.RotateAround(vrm.position, Vector3.up,faceData.currentFaceData.y);
        transform.RotateAround(vrm.position, Vector3.forward,faceData.currentFaceData.z);
    }
}
