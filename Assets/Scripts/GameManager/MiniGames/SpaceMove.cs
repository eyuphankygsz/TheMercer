using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMove : MiniGames
{
    [SerializeField] private RectTransform _rocketHolder;
    private Animator _rocketAnimator;
    private bool _done;

    [SerializeField] private Vector2 _limitX, _limitY;
    [SerializeField] private UIShake uiShake;

    private float _radioMinDistance;
    [SerializeField] private RectTransform _radioObj; 
    
    private float _planetMinDistance;
    [SerializeField] private RectTransform _planetObj;

    private int _collectedCount = 0;

    [SerializeField] private AudioClip _boostSound;
    private bool _boostPlaying;


    [SerializeField] private AudioClip _radioCollect, _planetLand;

    public override void HideMiniGame()
    {
        throw new System.NotImplementedException();
    }

    public override void Lost()
    {
        throw new System.NotImplementedException();
    }

    public override void MiniUpdate()
    {
        RotateRocket();
        uiShake.Shake();

        CheckObjects();


    }
    private void CheckObjects()
    {
        if (!_done && Vector2.Distance(_rocketHolder.anchoredPosition, _radioObj.anchoredPosition) < _radioMinDistance)
        {
            _collectedCount++;
            AudioManager.Instance.PlayAudio(_radioCollect);
            NextRadio();
        }
        else if(_done && Vector2.Distance(_rocketHolder.anchoredPosition, _planetObj.anchoredPosition) < _planetMinDistance)
        {
            AudioManager.Instance.PlayAudio(_planetLand);
            Won();
        }
    }
    private void NextRadio()
    {
        if (_collectedCount == 5)
        {
            _radioObj.gameObject.SetActive(false);
            _planetObj.gameObject.SetActive(true);
            _done = true;
            return;
        }

        float x = Random.Range(_limitX.x, _limitX.y);
        float y = Random.Range(_limitY.x, _limitY.y);

        _radioObj.anchoredPosition = new Vector2(x, y);
    }
    private void RotateRocket()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(vertical == 0)
            vertical = 0.5f;
        else if (vertical < 0)
            vertical = -0.2f;

        _rocketAnimator.SetBool("Fire", vertical > 0);
        if(vertical > 0 && !_boostPlaying)
        {
            _boostPlaying = true;
            AudioManager.Instance.PlayRepeatAudio(_boostSound);
        }
        else if(vertical <= 0)
        {
            _boostPlaying = false;
            AudioManager.Instance.StopAudio(_boostSound);
        }

        float newRotationZ = _rocketHolder.localEulerAngles.z + (180 * Time.deltaTime * -horizontal);

        Vector2 movement = _rocketHolder.up * vertical * 500 * Time.deltaTime;
        _rocketHolder.anchoredPosition += movement;

        Vector2 limitedPos = new Vector2(
            Mathf.Clamp(_rocketHolder.anchoredPosition.x, _limitX.x, _limitX.y),
            Mathf.Clamp(_rocketHolder.anchoredPosition.y, _limitY.x, _limitY.y)
        );
        _rocketHolder.anchoredPosition = limitedPos;


        // Apply the new rotation
        _rocketHolder.localEulerAngles = new Vector3(
            _rocketHolder.localEulerAngles.x,
            _rocketHolder.localEulerAngles.y,
            newRotationZ
        );
    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _radioMinDistance = _radioObj.rect.width / 2;
        _planetMinDistance = _planetObj.rect.height / 2;
        _rocketAnimator = _rocketHolder.GetComponent<Animator>();
    }

    public override void Won()
    {
        AudioManager.Instance.StopAudio(_boostSound);
        SceneManager.Instance.GetScene(true);
        gameObject.SetActive(false);
    }

}
