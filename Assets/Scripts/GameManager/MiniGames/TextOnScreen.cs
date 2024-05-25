using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOnScreen : MiniGames
{
    [SerializeField] private string[] _texts;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _pressButton;

    [SerializeField] private AudioClip _beatSfx;

    private float _timer = 0.8f;

    private int _pressCount;
    private bool _done, _pressed;

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
        if (_done)
            return;

        if (Input.GetMouseButtonDown(0) && !_pressed)
        {
            _pressed = true;
            _pressButton.SetActive(false);
            _timer = 0.8f;
            ShowText();
            _pressCount++;
            if(_pressCount == _texts.Length)
            {
                _done = true;
                Won();
            }
            AudioManager.Instance.PlayAudio(_beatSfx);
        }
        if(_pressed)
        {
            _timer -= Time.deltaTime;
            if(_timer < 0)
            {
                _pressed = false;
                _pressButton.SetActive(true);
            }
        }
    }

    private void ShowText()
    {
        _text.text = _texts[_pressCount];
    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _pressButton.SetActive(true);
    }

    public override void Won()
    {
        HideMiniGame();
    }
}
