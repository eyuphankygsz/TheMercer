using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEyes : MiniGames
{
    [SerializeField] private RectTransform _upper;
    [SerializeField] private Vector2 _upperMin;
    [SerializeField] private RectTransform _lower;
    [SerializeField] private Vector2 _lowerMin;
    [SerializeField] private int _openLimit;
    private int _openCount;

    bool _rightKey = false, _done = false, _autoClose = false;

    [SerializeField] private GameObject _pressKey;

    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
    }

    public override void Lost()
    {
        throw new System.NotImplementedException();
    }

    public override void MiniUpdate()
    {
        if (_done)
            return;

        if (!_autoClose)
        {
            _pressKey.SetActive(true);
            if (!_rightKey && Input.GetKeyDown(KeyCode.A))
            {
                _rightKey = true;
                OpenEye();
            }
            else if (_rightKey && Input.GetKeyDown(KeyCode.D))
            {
                _rightKey = false;
                OpenEye();
            }
            if (_upper.anchoredPosition.y > _upperMin.y - 20)
            {
                if (_openCount != _openLimit)
                {
                    _pressKey.SetActive(false);
                    _autoClose = true;
                    _openCount++;
                }
                else { Won(); }
            }

            _upper.anchoredPosition += Vector2.down * Time.deltaTime * 120;
            _lower.anchoredPosition += Vector2.up * Time.deltaTime * 120;

            Debug.Log(_upper.anchoredPosition);
        }
        else
        {
            AutoClose();
            if (_upper.anchoredPosition.y < _upperMin.x + 20)
            {
                _autoClose = false;
                _pressKey.SetActive(true);
            }
        }


        Limit();
    }

    private void OpenEye()
    {
        _upper.anchoredPosition += Vector2.up * 35;
        _lower.anchoredPosition += Vector2.down * 35;
    }
    private void AutoClose()
    {
        _upper.anchoredPosition += Vector2.down * Time.deltaTime * 1000;
        _lower.anchoredPosition += Vector2.up * Time.deltaTime * 1000;
    }
    private void Limit()
    {
        Vector2 upperLimit = new Vector2(0, Mathf.Clamp(_upper.anchoredPosition.y, _upperMin.x, _upperMin.y));
        _upper.anchoredPosition = upperLimit;

        Vector2 lowerLimit = new Vector2(0, Mathf.Clamp(_lower.anchoredPosition.y, _lowerMin.x, _lowerMin.y));
        _lower.anchoredPosition = lowerLimit;
    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _pressKey.SetActive(true);
    }

    public override void Won()
    {
        _done = true;
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }
}
