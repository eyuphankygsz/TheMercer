using UnityEngine;

public abstract class MiniGames : MonoBehaviour
{
    public abstract void ShowMiniGame();
    public abstract void HideMiniGame();
    public abstract void MiniUpdate();
    public abstract void Won();
    public abstract void Lost();
}
