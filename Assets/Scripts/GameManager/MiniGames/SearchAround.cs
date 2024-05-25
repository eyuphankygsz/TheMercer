using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchAround : MiniGames
{
    private float _fillAmount;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _keyImage;


    private bool _done = false;

    private float _filled = 0;
    private float _threshold = 0.2f;
    [SerializeField] private Image _fillDoneImage;


    [SerializeField] private RectTransform _background;
    private Vector2 _backStartPos = Vector2.zero;

    [SerializeField] private Image _photoParent;
    [SerializeField] private Image _photo;
    [SerializeField] private TextMeshProUGUI _photoText;
    [SerializeField] private Sprite[] _photos;
    [SerializeField] private string[] _title;


    [SerializeField] private AudioClip _photoSFX;
    private int _photoIndex = 0;

    [SerializeField] private AudioClip[] _step;
    private int _stepIndex = 0;
    private float _stepTimer;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Sprite[] _backgroundImages;
    private float _backgroundTimer = 0.5f;
    private int _backgroundLine = 0;

    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
        _background.sizeDelta = _backStartPos;
        _keyImage.gameObject.SetActive(false);
    }

    public override void Lost()
    {

    }

    public override void MiniUpdate()
    {

        ChangeBackground();

        if (_done)
            return;
        CheckDone();

        _fillAmount -= Time.deltaTime * 0.4f;
        if (Input.GetKeyDown(KeyCode.E))
            _fillAmount += 0.1f;

        _fillAmount = Mathf.Clamp(_fillAmount, 0, 1);

        _fillImage.fillAmount = _fillAmount;
        if (_fillAmount > 0.9f)
        {
            _background.sizeDelta += new Vector2(192, 108) * Time.deltaTime;
            _filled += Time.deltaTime * 0.6f;
            _stepTimer -= Time.deltaTime;
            if(_stepTimer <= 0)
            {
                _stepTimer = 0.6f;
                AudioManager.Instance.PlayAudio(_step[_stepIndex]);
                _stepIndex = (_stepIndex + 1) % _step.Length;
            }
        }

        float doneFillAmount = Mathf.InverseLerp(0, 5, _filled);
        if (doneFillAmount >= _threshold)
        {
            ShowPhoto();
            _threshold = doneFillAmount + 0.2f;
        }

        _fillDoneImage.fillAmount = doneFillAmount;
    }
    private void CheckDone()
    {
        if (_filled > 5)
        {
            _keyImage.gameObject.SetActive(false);
            _done = true;
            Won();
        }

    }
    public override void ShowMiniGame()
    {
        if (_backStartPos == Vector2.zero)
            _backStartPos = _background.sizeDelta;

        gameObject.SetActive(true);
        _keyImage.gameObject.SetActive(true);
        _done = false;
        _filled = 0;
        _fillAmount = 0;
        _photoIndex = 0;
    }
    public void ChangeBackground()
    {
        _backgroundTimer -= Time.deltaTime;
        if (_backgroundTimer <= 0)
        {
            _backgroundTimer = 0.5f;
            _backgroundImage.sprite = _backgroundImages[_backgroundLine];
            _backgroundLine = (_backgroundLine + 1) % _backgroundImages.Length;
        }
    }
    private void ShowPhoto()
    {
        AudioManager.Instance.PlayAudio(_photoSFX);
        _photoText.text = _title[_photoIndex];
        _photo.sprite = _photos[_photoIndex++];
        _photoParent.gameObject.SetActive(true);
        _photoParent.GetComponent<Animator>().SetTrigger("Show");
    }
    public override void Won()
    {
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }

}
