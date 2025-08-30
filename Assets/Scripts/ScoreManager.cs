using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public PlayerController playerController;
    private int totalScore = 0;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        playerController.OnFlipAchieved += HandleFlipAchieved;
        playerController.OnFinishAchieved += HandleFinishAchieved;
    }

    private void HandleFinishAchieved(int finishScore)
    {
        totalScore += finishScore;
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = "Score: " + totalScore;
    }

    private void HandleFlipAchieved(int flipScore)
    {
        totalScore += flipScore;
        DisplayScore();
    }    
}
