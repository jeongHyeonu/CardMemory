using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ÿ��Ʋ (����) ȭ�鿡 �ִ� ī�� strategy

public class CardTitle : MonoBehaviour, CardStrategy
{
    public Card targetCard; // �ش� strategy�� ������ Card Ŭ����
    float flip_duration = .5f;
    bool isFlipped; // �̹� ī�尡 ������ ���
    float expandValue = 1.5f;

    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject gameInfo;
    [SerializeField] Button infoClose;

    private void Start()
    {
        this.targetCard.SetCardStrategy(this); // ī�� ���� ����

        bestScore.text = "Best score : "+PlayerPrefs.GetInt("BestScore", 0); // �ִ�����

        this.transform.DOScale(expandValue, .5f).From(0).SetDelay(1f) // ����ȭ�� ī�� ����
            .OnComplete(() =>
            {
                startBtn.SetActive(true);
                startBtn.transform.DOScale(1f, 0.5f).From(0); // ���� ���� ��ư ����
            }); 
    }
    public void CardBehave()
    {
        if (isFlipped) return;
        Flip();
        gameInfo.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.FLAP_3); // ����
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
