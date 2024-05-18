using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class HearthBeat : MiniGames
{
    [SerializeField] private Image _bar;
    [SerializeField] private RectTransform _midPoint, _stopPoint;
    [SerializeField] private Image _hearth;
    private Vector2 _limits;
    private float _midLimit;
    private bool _goRight = true;
    private float _pressCount;
    private bool _done;
    private bool _keyPressed;

    //presscouny += 0.15f


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
        if (_done) return;
        MoveBar();
        CheckInput();
    }
    private void MoveBar()
    {
        _stopPoint.anchoredPosition += new Vector2(_goRight ? 500 : -500, 0) * Time.deltaTime;
        if (_goRight && _stopPoint.anchoredPosition.x >= _limits.y)
            _goRight = false;
        else if (!_goRight && _stopPoint.anchoredPosition.x <= _limits.x)
            _goRight = true;
    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _pressCount = 0;
        _goRight = true;
        _stopPoint.anchoredPosition = new Vector2(_limits.x, _stopPoint.anchoredPosition.y);

        _midLimit = _midPoint.rect.width / 2;
        _limits = new Vector2(-(_bar.rectTransform.rect.width / 2), _bar.rectTransform.rect.width / 2);
    }

    private void CheckInput()
    {
        CheckKeyPressed();
        if (_keyPressed) return;

        if (Input.GetKeyDown(KeyCode.E))
            _keyPressed = true;

        if (_keyPressed)
            CheckMidPoint();

    }

    private void CheckMidPoint()
    {
        float distance = Vector2.Distance(_stopPoint.anchoredPosition, _midPoint.anchoredPosition);
        if (distance <= _midLimit)
        {
            _pressCount += 0.15f;
            if (_pressCount > 1)
            {
                _pressCount = 1;
                Won();
                _done = true;
            }
            _hearth.color = Color.Lerp(Color.black, Color.white, _pressCount);
            _hearth.GetComponent<Animator>().SetTrigger("BeatFast");
        }
        else
            _keyPressed = false;
    }

    private void CheckKeyPressed()
    {
        float distance = Vector2.Distance(_stopPoint.anchoredPosition, _midPoint.anchoredPosition);
        if (distance > _midLimit)
            _keyPressed = false;

    }
    public override void Won()
    {

    }

}
