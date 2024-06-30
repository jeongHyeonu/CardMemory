using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardData cardData; // 스크립터블 오브젝트, CardManager에서 랜덤으로 생성해줌
    private CardStrategy cardStrategy;
    public Renderer cardRender ; // 카드 뒷면 스프라이트 적용할 object

    // 카드의 행동(전략) 설정
    public void SetCardStrategy(CardStrategy _strategy)
    {
        this.cardStrategy = _strategy;
        cardStrategy.CardInit(this);
        SetCardImage();
    }

    // 카드의 행동(전략) 리턴
    public CardStrategy GetCardStrategy()
    {
        return cardStrategy;
    }

    // 카드 이미지 설정 - 매터리얼 동적(런타임) 변경
    private void SetCardImage()
    {
        // 기존 매터리얼
        Material[] originMat = cardRender.materials;

        Material card_back = Instantiate(originMat[0]); // 카드 뒷면
        Material card_front = Instantiate(originMat[1]); // 카드 앞면
        card_front.mainTexture = cardData.CardImage;

        // 새로 변경할 매터리얼
        Material[] newMat = { card_back, card_front };

        cardRender.materials = newMat;
    }

    // 카드 클릭 시, 실제 실행되는 명령은 설정된 CardStrategy에 따라 적용
    // 클릭 감지는 alpha=0인 이미지를 가진 Image 컴포넌트를 통해 Click 감지
    public void OnPointerClick(PointerEventData eventData)
    {
        cardStrategy.CardBehave();
    }
}
