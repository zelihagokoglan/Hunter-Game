// kameraya bakar player ve enemy için slider canvas larına ekledim

using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cam;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void LateUpdate() 
    {
        if (cam == null) cam = _camera;
        transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
    }
}