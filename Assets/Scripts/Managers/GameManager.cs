using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public ScoreManager scoreManager;    // 점수 관리자
    public bool isGaming;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] TextMeshProUGUI gameOverMessage;
    [SerializeField] GameTimer gameTimer;

    // 싱글톤
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this); // CardMemory 게임 시작시 새로 scoreManager/ui/timer 를 참조해야 하기 때문에, DontDestroyOnRoad 하지 않음
        }
        else Destroy(gameObject);
    }

    public void GameStart()
    {
        gameTimer.StartTimer();
        isGaming = true;
    }

    public void GamePause()
    {
        isGaming = !isGaming;
        if(!isGaming) DOTween.PauseAll();
        if(isGaming) DOTween.PlayAll();
    }

    public void GameEnd(bool isVictory)
    {
        isGaming = false;
        scoreManager.setGameoverScore();

        if (isVictory)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.VICTORY);
            gameOverMessage.text = "Victory!";
            gameTimer.StopTimer();
        }
        else
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.TIMEOUT);
            gameOverMessage.text = "Time Over!";
        }
        gameOverUI.SetActive(true);
    }

    public void PlusScore()
    {
        scoreManager.scoreAdd();
    }

    public void MinusScore()
    {
        scoreManager.scoreMinus();
    }

}
