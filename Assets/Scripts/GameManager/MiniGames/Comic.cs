using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Comic : MiniGames
{
    [SerializeField] private Image[] _images;
    [SerializeField] private TextMeshProUGUI[] _text;
    [SerializeField] private GameObject _mousePress;
    private int _imageLine;
    private float _waitAfterTimer = 1;
    private bool _canClick;
    private bool _clearImage, _cleared, _done;
    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
        SceneManager.Instance.GetScene(true);
    }

    public override void Lost()
    {

    }

    public override void MiniUpdate()
    {
        if (_canClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckFinish())
                {
                    Won();
                    _done = true;
                    return;
                }
                _mousePress.SetActive(false);
                _canClick = false;
                _clearImage = true;
            }
        }

        ClearImage();
    }
    private bool CheckFinish()
    {
        return _imageLine >= _images.Length;
    }

    private void ClearImage()
    {
        if (!_clearImage) return;
        if (!_cleared)
        {

            Color color = _images[_imageLine].color;

            color.a += Time.deltaTime * 0.4f;
            if (color.a > 1)
            {
                color.a = 1;
                _cleared = true;
            }
            _images[_imageLine].color = color;
            for (int i = 0; i < _images[_imageLine].transform.childCount; i++)
            {
                if (_images[_imageLine].transform.GetChild(i).TryGetComponent(out Image img))
                    img.color = color;
            }

            if (_text[_imageLine] != null)
            {
                Color colorText = _text[_imageLine].color;
                colorText.a += Time.deltaTime * 0.4f;
                if (colorText.a > 1)
                    colorText.a = 1;
                _text[_imageLine].color = colorText;
            }

        }
        else
        {
            _waitAfterTimer -= Time.deltaTime;
            if (_waitAfterTimer <= 0)
            {
                _waitAfterTimer = 1;
                _imageLine++;
                _cleared = false;
                _clearImage = false;
                _canClick = true;
                _mousePress.SetActive(true);
            }
        }

    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _mousePress.SetActive(true);
        _canClick = true;
    }

    public override void Won()
    {
        HideMiniGame();
    }
}
