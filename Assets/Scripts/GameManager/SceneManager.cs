using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private TheScene[] _scenes;
    [SerializeField] private Image _background;
    private TheScene _selectedScene;
    private float _timer;
    private int _index = 0;

    [SerializeField] private GameObject _enterImage;
    [SerializeField] private AudioClip _startBgMusic;



    [SerializeField] private Image _blackness;
    private bool _runBlack;
    private bool _blackOut;
    private bool _started;

    private void Awake()
    {
        Instance = this;

        _selectedScene = _scenes[_index];
        GetScene(false);

    }
    private void Start()
    {
        AudioManager.Instance.ChangeBackground(_startBgMusic);
    }
    private void Update()
    {
        NextSceneTimer();
        ClickControl();
        BlackEntry();
        if (_selectedScene.MiniGame != null && _started)
            _selectedScene.MiniGame.MiniUpdate();
    }

    private void NextSceneTimer()
    {
        if (!_selectedScene.TimerOn)
            return;

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }
        GetScene(true);


    }

    private void ClickControl()
    {
        if (_selectedScene.AllowClick)
        {
            if (_enterImage)
                _enterImage.SetActive(true);

            if (Input.GetMouseButtonDown(0))
                GetScene(true);
        }
    }

    public void GetScene(bool next)
    {
        _started = false;
        if (next)
            _index++;

        _selectedScene = _scenes[_index];

        if (_selectedScene.BlackEntry)
        {
            _blackness.gameObject.SetActive(true);
            _runBlack = true;
        }
        else
            SetScene();
    }
    private void SetScene()
    {
        _timer = _selectedScene.TimerOn ? _selectedScene.Time : 1;
        _background.sprite = _selectedScene.Sprite;
        _enterImage.SetActive(false);

        for (int i = 0; i < _selectedScene.Events.Length; i++)
            _selectedScene.Events[i].Invoke();

        if (_selectedScene.MiniGame != null)
        {
            _selectedScene.MiniGame.ShowMiniGame();
            _started = true;
        }
    }


    private void BlackEntry()
    {
        if (!_runBlack)
            return;

        Color color = _blackness.color;

        if (!_blackOut)
        {
            color.a += Time.deltaTime;
            if(color.a >= 1)
            {
                color.a = 1;
                _blackOut = true;
                SetScene();
            }
        }
        else
        {
            color.a -= Time.deltaTime;
            if(color.a <= 0)
            {
                color.a = 0;
                _blackOut = false;
                _runBlack = false;
                _blackness.gameObject.SetActive(false);
            }
        }

        _blackness.color = color;


    }
}
[Serializable]
public struct TheScene
{
    public string SceneName;
    public Sprite Sprite;
    public MiniGames MiniGame;
    public bool TimerOn;
    public float Time;
    public bool AllowClick;
    public bool BlackEntry;
    public UnityEvent[] Events;
}
