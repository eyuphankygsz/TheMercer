using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MiniGames
{
    public string sceneName;
    public override void HideMiniGame()
    {
    }

    public override void Lost()
    {
    }

    public override void MiniUpdate()
    {
    }

    public override void ShowMiniGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public override void Won()
    {
        
    }
}
