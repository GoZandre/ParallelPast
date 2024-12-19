using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraRatioLock : MonoBehaviour
{
    private Camera _camera;
    public float targetAspectRatio = 16f / 9f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        if (!_camera)
        {
            Debug.LogError("Camera component not found on this GameObject.");
            return;
        }

        // Calculate the desired height based on the target aspect ratio
        float targetWidth = _camera.orthographicSize * 2 * targetAspectRatio;
        _camera.orthographicSize = targetWidth / (2 * _camera.aspect);
    }
}
