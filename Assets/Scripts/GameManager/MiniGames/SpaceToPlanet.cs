using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SpaceToPlanet : MiniGames
{
    [SerializeField] private RectTransform _middlePoint; // Reference to the middle point RectTransform
    [SerializeField] private float _middleLimit;
    [SerializeField] private float _moveSpeed; // Speed of movement
    [SerializeField] private RectTransform _controlPoint;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Image _fillImage;
    private float _fillAmount = 0;

    private float _randomTimer;
    private Vector2 _oppositeMovement;




    [SerializeField] private UIShake _uiShake;
    private bool _done;
    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
    }

    public override void Lost()
    {
        _fillAmount = 0;
        _fillImage.fillAmount = 0;

    }

    public override void MiniUpdate()
    {
        MoveControlPoint();

        if(!_done) 
            _uiShake.Shake();
    }
    private void MoveControlPoint()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector2 movement = new Vector3(horizontalInput, verticalInput).normalized;

        ChangeDirection();

        Vector3 newPosition = _controlPoint.anchoredPosition + ((movement * _moveSpeed * Time.deltaTime) + (_oppositeMovement * Time.deltaTime * _moveSpeed / 3));
        newPosition = ClampControl(newPosition);
        _controlPoint.anchoredPosition = newPosition;

        // Check if the control point reaches the middle point
        float distance = Vector2.Distance(newPosition, _middlePoint.anchoredPosition);
        if (distance < _middleLimit) // Adjust the threshold as needed
        {
            _fillAmount += 0.3f * Time.deltaTime;
            if (_fillAmount > 1)
            {
                _fillAmount = 1;
                _done = true;
                _uiShake.StopShake();
                Won();
            }
            _fillImage.color = Color.Lerp(Color.red, Color.green, _fillAmount);
            _fillImage.fillAmount = _fillAmount;
        }
    }
    private Vector3 ClampControl(Vector3 pos)
    {
        float halfWidth = _content.rect.width / 2;
        float halfHeight = _content.rect.height / 2;
        pos.x = Mathf.Clamp(pos.x, -halfWidth, halfWidth);
        pos.y = Mathf.Clamp(pos.y, -halfHeight, halfHeight);
        return pos;
    }
    private void ChangeDirection()
    {
        if (_randomTimer > 0)
        {
            _randomTimer -= Time.deltaTime;
            return;
        }
        _randomTimer = Random.Range(0.6f, 1.2f);

        float horizontal = Random.Range(0, 2)  == 0 ? Random.Range(0.6f,1f) : Random.Range(-1f,-0.6f);
        float vertical = Random.Range(0, 2) == 0 ? Random.Range(0.6f, 1f) : Random.Range(-1f, -0.6f);

        Vector2 movement = new Vector3(horizontal, vertical).normalized;
        _oppositeMovement = movement;
    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _middleLimit = _middlePoint.rect.width / 2;
    }

    public override void Won()
    {
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }
}
