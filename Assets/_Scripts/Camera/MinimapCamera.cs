using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [Header("Follow functionality")]
    [SerializeField] Transform _target;
    [SerializeField] float _height = 50f;

    [Header("Zoom functionality")]
    [SerializeField][Range(0, 1000)] float _minSize = 100f;
    [SerializeField][Range(0, 1000)] float _maxSize = 1000f;
    [SerializeField][Range(0.1f, 1.0f)] float _zoomRate = 0.5f;
    [SerializeField] KeyCode _zoomInKey;
    [SerializeField] KeyCode _zoomOutKey;
    private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {
        // Camera follow player
        transform.position = _target.position + Vector3.up * _height;

        // Camera zoom controls
        if (Input.GetKey(_zoomInKey))
        {
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - _zoomRate, _minSize, _maxSize);
        }
        if (Input.GetKey(_zoomOutKey))
        {
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + _zoomRate, _minSize, _maxSize);
        }
        
    }
}
