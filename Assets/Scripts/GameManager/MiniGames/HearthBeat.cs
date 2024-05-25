using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HearthBeat : MiniGames
{
    [SerializeField] private Image _bar;
    [SerializeField] private RectTransform _midPoint, _stopPoint;
    [SerializeField] private Image _hearth;
    [SerializeField] private string[] _texts;
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private Image _bg;
    [SerializeField] private Sprite[] _bgSprites;
    [SerializeField] private AudioClip _beatSfx, _wrongSfx, _hitSfx;
    [SerializeField] private Image _white;
    
    private int _bgLine;
    private Vector2 _limits;
    private float _midLimit;
    private bool _goRight = true;
    private float _pressCount, _lerpAmount;
    private bool _done;
    private bool _keyPressed;

    private int _textLine;


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
        AudioManager.Instance.PlayRepeatAudio(_beatSfx);
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


        Color c = _white.color;
        c.a -= Time.deltaTime;
        _white.color = c;

    }

    private void CheckMidPoint()
    {
        float distance = Vector2.Distance(_stopPoint.anchoredPosition, _midPoint.anchoredPosition);
        if (distance <= _midLimit)
        {
            _pressCount += 1;
            _lerpAmount += 1f / _texts.Length;
            _tmp.text = _texts[_textLine++];
            if (_pressCount >= _texts.Length)
            {
                Won();
                _done = true;
            }
            _hearth.color = Color.Lerp(Color.black, Color.white, _lerpAmount);
            _hearth.GetComponent<Animator>().SetTrigger("BeatFast");

            _bg.sprite = _bgSprites[_bgLine++];
            AudioManager.Instance.PlayAudio(_hitSfx);

            Color c = _white.color;
            c.a = 0.85f;
            _white.color = c;
        }
        else
        {
            _keyPressed = false;
            AudioManager.Instance.PlayAudio(_wrongSfx);
        }
    }

    private void CheckKeyPressed()
    {
        float distance = Vector2.Distance(_stopPoint.anchoredPosition, _midPoint.anchoredPosition);
        if (distance > _midLimit)
            _keyPressed = false;

    }
    public override void Won()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.StopAudio(_beatSfx);
        SceneManager.Instance.GetScene(true);
    }

}
