using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShake : MonoBehaviour
{
    [SerializeField] private Vector2 _startPoint;
    [SerializeField] private float _maxDistance, _shakeSpeed;
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _shakeTimer;
    private void Awake()
    {
        _startPoint = _content.anchoredPosition;
    }
    public void Shake()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            return;
        }
        _shakeTimer = _shakeSpeed;

        Vector2 offset = new Vector2(Random.Range(-_maxDistance, _maxDistance), Random.Range(-_maxDistance, _maxDistance));
        _content.anchoredPosition = _startPoint + offset;
    }
    public void StopShake()
    {
        _content.anchoredPosition = _startPoint;
    }
}
