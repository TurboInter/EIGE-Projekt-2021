using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownPlatform : MonoBehaviour
{
    [SerializeField] private float maxDelta;
    [SerializeField] private float speed;
    private Vector3 _basePos;
    private Vector3 _top;
    private Vector3 _bottom;
    private bool _movingUp = true;

    void Start()
    {
        var current = transform.position;
        _basePos = new Vector3(current.x, current.y, current.z);
        _top = _basePos + new Vector3(0, 1, 0) * (maxDelta * 1.1F);
        _bottom = _basePos + new Vector3(0, -1, 0) * (maxDelta * 1.1F);
    }

    void Update()
    {
        if ((transform.position - _basePos).magnitude >= maxDelta)
        {
            _movingUp = !_movingUp;
        }

        var target = _movingUp ? _top : _bottom;
        var lerp = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        transform.position = lerp;
    }
}