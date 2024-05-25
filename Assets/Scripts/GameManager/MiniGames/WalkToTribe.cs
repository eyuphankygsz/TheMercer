using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalkToTribe : MiniGames
{
    [SerializeField] private RectTransform _astro;
    [SerializeField] private GameObject _mouse;

    [SerializeField] private int _maxMoveClick;
    private int _moveClicks;
    [SerializeField] private int _lastPos;

    private bool _moveAstro;
    private float _nextXPos;
    private bool _returnStep;
    private float _direction;

    [SerializeField] private GameObject _opposite;


    [SerializeField] Sprite[] _astroSprites, _alienSprites;
    private int _spritesLine;

    [SerializeField] private AudioClip _noClip;

    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
    }

    public override void Lost()
    {

    }

    public override void MiniUpdate()
    {

        if (_moveAstro)
            _mouse.SetActive(true);

        if (Input.GetMouseButtonDown(0) && !_moveAstro)
            Next();

        MoveAstro();

    }
    private void Next()
    {
        NextSprites();
        _mouse.SetActive(false);
        if (_moveClicks < _maxMoveClick)
        {
            _moveAstro = true;
            _astro.GetComponent<Animator>().SetTrigger("Jump");
            _nextXPos = _astro.anchoredPosition.x + 200;
            _direction = 1;
            _moveClicks++;
        }
        else
        {
            if (!_returnStep)
            {
                _opposite.SetActive(false);
                _moveAstro = true;
                _astro.GetComponent<Animator>().SetTrigger("Jump");
                _nextXPos = _astro.anchoredPosition.x - 300;
                Vector2 pos = new Vector2(_nextXPos, _astro.anchoredPosition.y);
                _direction = -1;
                _returnStep = true;
                _moveClicks++;


            }
            else Won();
        }
        if(_moveClicks == _maxMoveClick)
        {
            _opposite.SetActive(true);
            AudioManager.Instance.PlayAudio(_noClip);
        }

    }
    private void NextSprites()
    {
        if (_spritesLine == _astroSprites.Length) return;

        _astro.GetComponent<Image>().sprite = _astroSprites[_spritesLine];
        _opposite.transform.parent.GetComponent<Image>().sprite = _alienSprites[_spritesLine++];
    }
    private void MoveAstro()
    {
        if (_moveAstro)
        {
            _astro.anchoredPosition += new Vector2(200, 0) * Time.deltaTime * 2 * _direction;
            if ((_direction >= 0 && _astro.anchoredPosition.x >= _nextXPos) || (_direction < 0 && _astro.anchoredPosition.x <= _nextXPos))
                _moveAstro = false;
        }

    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _mouse.SetActive(true);

    }

    public override void Won()
    {
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }

}
