using UnityEngine;
using UnityEngine.UI;

public class UnHoldHands : MiniGames
{
    private float _amount;

    private float _fillAmount;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _keyImage;


    private bool _done = false, _stopWhite = false;

    [SerializeField] private RectTransform _astro, _woman;

    [SerializeField] private AudioClip _unholdSfx;



    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
        SceneManager.Instance.GetScene(true);
    }

    public override void Lost()
    {
        throw new System.NotImplementedException();
    }

    public override void MiniUpdate()
    {
        if (_done)
            return;

        CheckDone();

        _fillAmount -= Time.deltaTime * 0.5f;
        if (Input.GetKeyDown(KeyCode.E))
            _fillAmount += 0.1f;

        _fillAmount = Mathf.Clamp(_fillAmount, 0, 1);

        _fillImage.fillAmount = _fillAmount;
        if (_fillAmount > 0.9f)
        {
            _astro.anchoredPosition += Vector2.right * Time.deltaTime * -100;
            _woman.anchoredPosition += Vector2.right * Time.deltaTime * 100;
        }
        else
        {
            _astro.anchoredPosition += Vector2.right * Time.deltaTime * 250;
            _woman.anchoredPosition += Vector2.right * Time.deltaTime * -250;
        }
        Vector2 limitAstro = new Vector2(Mathf.Clamp(_astro.anchoredPosition.x, -1250, -700),_astro.anchoredPosition.y);
        _astro.anchoredPosition = limitAstro;
        Vector2 limitWoman = new Vector2(Mathf.Clamp(_woman.anchoredPosition.x, 620, 1250), _woman.anchoredPosition.y);
        _woman.anchoredPosition = limitWoman;
    }
    private void CheckDone()
    {
        if (Vector2.Distance(_astro.anchoredPosition, _woman.anchoredPosition) > 2400)
        {
            Won();
            _keyImage.gameObject.SetActive(false);
            _done = true;
            _fillImage.gameObject.SetActive(false);
            _keyImage.gameObject.SetActive(false);
        }

    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
    }

    public override void Won()
    {
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }
}
