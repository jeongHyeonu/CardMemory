using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void ScoreFunc();
    public ScoreFunc scoreAdd, scoreMinus, setGameoverScore;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameoverScore;

    public int successScore = 10;
    public int failureScore = 1;

    public int score = 0;

    private void Start()
    {
        GameManager.Instance.scoreManager = this;
        scoreAdd = ScoreAdd; 
        scoreMinus = ScoreMinus;
        setGameoverScore = SetGameoverScore;
    }

    public void ScoreAdd()
    {
        score += successScore;
        scoreText.text = "Score : " + score.ToString();
        scoreText.DOColor(Color.white,.5f).From(Color.green); // ux
    }

    public void ScoreMinus()
    {
        if (score < failureScore) return; // 현재 점수가 감소시킬 점수보다 작다면 실행 X
        score -= failureScore;
        scoreText.text = "Score : " + score.ToString();
        scoreText.DOColor(Color.white, .5f).From(Color.red); // ux
    }

    public void SetGameoverScore()
    {
        gameoverScore.DOText("Your score : " + score.ToString(), 1f).From(""); // ux

        int highScore = PlayerPrefs.GetInt("BestScore", 0);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("BestScore", highScore);
            PlayerPrefs.Save(); // 데이터 저장
        }
    }


}
