using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Talking1 : MiniGames
{
    [SerializeField] private string[] _dialogues;
    [SerializeField] private int[] _line;

    [SerializeField] private GameObject[] _dialogueObj;
    [SerializeField] private TextMeshProUGUI[] _dialogueText;
    [SerializeField] private RectTransform _pressButton;
    [SerializeField] private Vector2 _maxPos, _minPos;
    [SerializeField] private float _minButtonSize;
    private Vector2 _buttonSize = new Vector2(140, 140);




    [SerializeField] Sprite[] _astroSprites, _alienSprites, _womanSprites;
    [SerializeField] Image _astro, _aliens, _woman;

    private int _pressCount;
    private bool _done;
    public override void HideMiniGame()
    {

    }

    public override void Lost()
    {

    }

    public override void MiniUpdate()
    {
        if (_done)
            return;

        _pressButton.sizeDelta -= new Vector2(100, 100) * Time.deltaTime;

        if (_pressButton.sizeDelta.x < _minButtonSize)
            NextButton();
    }

    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
    }

    public override void Won()
    {
        gameObject.SetActive(false);
        SceneManager.Instance.GetScene(true);
    }
    public void ButtonPressed()
    {
        if (_pressCount >= _dialogues.Length)
        {
            _done = true;
            gameObject.SetActive(true);
            _pressButton.gameObject.SetActive(false);
            Won();
        }
        else
        {
            NextDialogue();
            NextButton();
            _pressCount++;
        }
    }
    private void NextButton()
    {
        _pressButton.anchoredPosition = new Vector2(Random.Range(_minPos.x, _maxPos.x), Random.Range(_minPos.y, _maxPos.y));
        _pressButton.sizeDelta = _buttonSize;
    }
    private void NextDialogue()
    {
        CheckSpecial();
        int line = _line[_pressCount];

        _astro.sprite = _astroSprites[_pressCount];
        _aliens.sprite = _alienSprites[_pressCount];
        _woman.sprite = _womanSprites[_pressCount];

        for (int i = 0; i < _dialogueObj.Length; i++)
            _dialogueObj[i].SetActive(false);

        _dialogueObj[line].SetActive(true);
        _dialogueText[line].text = _dialogues[_pressCount];
    }

    void CheckSpecial()
    {
        if(_pressCount == 4)
        {
            _woman.GetComponent<Animator>().SetTrigger("WomanIn");
        }
    }
}
