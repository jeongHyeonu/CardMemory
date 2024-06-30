using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 타이틀 (메인) 화면에 있는 카드 strategy

public class CardTitle : MonoBehaviour, CardStrategy
{
    public Card targetCard; // 해당 strategy를 실행할 Card 클래스
    float flip_duration = .5f;
    bool isFlipped; // 이미 카드가 뒤집힌 경우
    float expandValue = 1.5f;

    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject gameInfo;
    [SerializeField] Button infoClose;

    private void Start()
    {
        this.targetCard.SetCardStrategy(this); // 카드 전략 설정

        bestScore.text = "Best score : "+PlayerPrefs.GetInt("BestScore", 0); // 최대점수

        this.transform.DOScale(expandValue, .5f).From(0).SetDelay(1f) // 시작화면 카드 등장
            .OnComplete(() =>
            {
                startBtn.SetActive(true);
                startBtn.transform.DOScale(1f, 0.5f).From(0); // 게임 시작 버튼 등장
            }); 
    }
    public void CardBehave()
    {
        if (isFlipped) return;
        Flip();
        gameInfo.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.FLAP_3); // 사운드
    }

    public void CardInit(Card card)
    {
        targetCard = card;
    }

    public void Flip()
    {
        if (isFlipped)
        {
            targetCard.transform.DOLocalRotate(Vector3.zero, flip_duration).SetEase(Ease.InOutQuad).From(new Vector3(0, 180, 0));
            infoClose.interactable = false;
            DOVirtual.DelayedCall(flip_duration, () =>
            {
                gameInfo.SetActive(false);
                isFlipped = false;
                infoClose.interactable = true;
            });
        }
        else {
            targetCard.transform.DOLocalRotate(new Vector3(0, 180, 0), flip_duration).SetEase(Ease.InOutQuad).From(Vector3.zero);
            isFlipped = true;
        }
    }
}
