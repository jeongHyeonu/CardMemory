using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public ScoreManager scoreManager;    // ���� ������
    public bool isGaming;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] TextMeshProUGUI gameOverMessage;
    [SerializeField] GameTimer gameTimer;

    // �̱���
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this); // CardMemory ���� ���۽� ���� scoreManager/ui/timer �� �����ؾ� �ϱ� ������, DontDestroyOnRoad ���� ����
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
