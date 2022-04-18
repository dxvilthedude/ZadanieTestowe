using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public static int Score;
    public static int TotalScore = 0;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text totalScoreText;

    public void ShowScore()
    {
        TotalScore += Score;
        scoreText.text = Score.ToString();
        totalScoreText.text = TotalScore.ToString();
    }
    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }
}
