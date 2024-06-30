using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardTouch : CardStrategy
{
    public Card targetCard; // �ش� strategy�� ������ Card Ŭ����
    public CardManager cm;

    float flip_duration = .5f;
    bool isFlipped; // �̹� ī�尡 ������ ���

    public int stageNumber; 

    public void CardBehave()
    {
        if (isFlipped) return; // �̹� ���� ��� ���� X
        if (cm.isWrong) return; // �г�Ƽ ����, Ʋ�� ��� ��ġ ����
        Flip(); // ī�� ȸ��

        // Ŀ�ǵ� ���
        cm.RunCardCommand(this); // ����/���� üũ�ϴ� ����� CardManager���� ����

        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.FLAP_1); // ����
    }

    public void CardInit(Card card)
    {
        targetCard = card;
        stageNumber = cm.stageNumber;
    }

    public void Flip()
    {
        // ī�� ȸ�� UX
        if (isFlipped) targetCard.transform.DOLocalRotate(Vector3.zero, flip_duration).SetEase(Ease.InOutQuad).From(new Vector3(0, 180, 0));
        else targetCard.transform.DOLocalRotate(new Vector3(0, 180, 0), flip_duration).SetEase(Ease.InOutQuad).From(Vector3.zero);
        this.isFlipped = !this.isFlipped;
    }
}
