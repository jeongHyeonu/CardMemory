using UnityEngine;

// CardTouchCommand.cs : 실제 실행될 기능을 캡슐화 하기 위한 스크립트 - 호출자/수신자 의존성 제거

// -- 호출자 (Invoker) : CardManager.cs
// -- 수신자 (Receiver) : CardTouch.cs

// 카드를 2개 연속해서 클릭 시, 실제 실행되는 기능은 CardManager 클래스에서 구현

public class CardTouchCommand : ICommand
{
    private CardTouch card;

    public CardTouchCommand(CardTouch _card)
    {
        this.card = _card;
    }

    public void Execute()
    {
        card.CardBehave();
    }

    public CardTouch GetCard()
    {
        return card;
    }

    //public void Undo()
}
