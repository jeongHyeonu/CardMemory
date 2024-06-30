using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardTouch : CardStrategy
{
    public Card targetCard; // 해당 strategy를 실행할 Card 클래스
    public CardManager cm;

    float flip_duration = .5f;
    bool isFlipped; // 이미 카드가 뒤집힌 경우

    public int stageNumber; 

    public void CardBehave()
    {
        if (isFlipped) return; // 이미 눌린 경우 실행 X
        if (cm.isWrong) return; // 패널티 변수, 틀린 경우 터치 방지
        Flip(); // 카드 회전

        // 커맨드 등록
        cm.RunCardCommand(this); // 오답/정답 체크하는 기능은 CardManager에서 진행

        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.FLAP_1); // 사운드
    }

    public void CardInit(Card card)
    {
        targetCard = card;
        stageNumber = cm.stageNumber;
    }

    public void Flip()
    {
        // 카드 회전 UX
        if (isFlipped) targetCard.transform.DOLocalRotate(Vector3.zero, flip_duration).SetEase(Ease.InOutQuad).From(new Vector3(0, 180, 0));
        else targetCard.transform.DOLocalRotate(new Vector3(0, 180, 0), flip_duration).SetEase(Ease.InOutQuad).From(Vector3.zero);
        this.isFlipped = !this.isFlipped;
    }
}
