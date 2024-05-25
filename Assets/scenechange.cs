using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenechange : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Chapter1");
    }
}
