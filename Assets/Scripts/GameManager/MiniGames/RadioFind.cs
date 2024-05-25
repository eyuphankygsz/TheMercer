using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioFind : MiniGames
{
    [SerializeField] private GameObject _pressKey;
    [SerializeField] private RectTransform _point1, _point2;
    [SerializeField] private RectTransform _pointLine1, _pointLine2;
    [SerializeField] private RectTransform _targ1, _targ2;
    [SerializeField] private Image _color1, _color2;

    private bool _found1 = false, _found2 = false;
    [SerializeField] private AudioClip _clip1, _clip2;
    private float _point1Limit, _point2Limit;

    bool _done = false;

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
        GetInput();
        LimitPos();
        CalculateLight();
        CheckPos();
    }

    private void CheckPos()
    {
        Debug.Log(Vector2.Distance(_point1.anchoredPosition, _targ1.anchoredPosition));
        Debug.Log(_targ1.rect.width / 2);
        if (Vector2.Distance(_point1.anchoredPosition, _targ1.anchoredPosition) < _targ1.rect.width / 2)
        {
            if(!_found1)
            {
                AudioManager.Instance.PlayAudio(_clip1);
                _found1 = true;
            }
            if (Vector2.Distance(_point2.anchoredPosition, _targ2.anchoredPosition) < _targ2.rect.width / 2)
            {
                if (!_found2)
                {
                    AudioManager.Instance.PlayAudio(_clip1);
                    _found2 = true;
                }
                _done = true;
                Won();
            }
            else
            {
                if (_found2)
                {
                    AudioManager.Instance.PlayAudio(_clip2);
                    _found2 = false;
                }
            }
        }
        else
        {

            if (_found1)
            {
                AudioManager.Instance.PlayAudio(_clip2);
                _found1 = false;
            }
        }
        if (Vector2.Distance(_point2.anchoredPosition, _targ2.anchoredPosition) < _targ2.rect.width / 2)
        {
            if (!_found2)
            {
                AudioManager.Instance.PlayAudio(_clip1);
                _found2 = true;
            }
        }
        else
        {
            if (_found2)
            {
                AudioManager.Instance.PlayAudio(_clip2);
                _found2 = false;
            }
        }
    }
    private void CalculateLight()
    {
        float lerpPoint1 = Mathf.InverseLerp(-_point1Limit, _point1Limit, _point1.anchoredPosition.x);
        float lerpTarg1 = Mathf.InverseLerp(-_point1Limit, _point1Limit, _targ1.anchoredPosition.x);
        float dist1 = Mathf.Abs(lerpPoint1 - lerpTarg1);
        _color1.color = Color.Lerp(Color.green, Color.red, dist1);


        float lerpPoint2 = Mathf.InverseLerp(-_point2Limit, _point2Limit, _point2.anchoredPosition.x);
        float lerpTarg2 = Mathf.InverseLerp(-_point2Limit, _point2Limit, _targ2.anchoredPosition.x);
        float dist2 = Mathf.Abs(lerpPoint2 - lerpTarg2);
        _color2.color = Color.Lerp(Color.green, Color.red, dist2);
    }
    private void LimitPos()
    {
        Vector2 p1Pos = new Vector2(Mathf.Clamp(_point1.anchoredPosition.x, -_point1Limit, _point1Limit), _point1.anchoredPosition.y);
        _point1.anchoredPosition = p1Pos;

        Vector2 p2Pos = new Vector2(Mathf.Clamp(_point2.anchoredPosition.x, -_point2Limit, _point2Limit), _point2.anchoredPosition.y);
        _point2.anchoredPosition = p2Pos;
    }
    private void GetInput()
    {
        if (Input.GetKey(KeyCode.A))
            _point1.anchoredPosition += Vector2.left * Time.deltaTime * 900;
        else if (Input.GetKey(KeyCode.D))
            _point1.anchoredPosition += Vector2.right * Time.deltaTime * 900;
        else if (Input.GetKey(KeyCode.Q))
            _point2.anchoredPosition += Vector2.left * Time.deltaTime * 200;
        else if (Input.GetKey(KeyCode.E))
            _point2.anchoredPosition += Vector2.right * Time.deltaTime * 200;
    }

    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _pressKey.SetActive(true);
        _point1Limit = _pointLine1.rect.width / 2;
        _point2Limit = _pointLine2.rect.width / 2;
    }

    public override void Won()
    {
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }
}
