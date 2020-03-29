using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text BestRound;

    private void Awake()
    {
        int bestround = PlayerPrefs.GetInt("BestRound");

        BestRound.text = bestround.ToString();
    }

    public void OnLoadInGame()
    {
        SceneManager.LoadScene(1);
    }
}
