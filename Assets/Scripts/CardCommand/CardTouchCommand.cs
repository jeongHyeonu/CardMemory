using UnityEngine;

// CardTouchCommand.cs : ���� ����� ����� ĸ��ȭ �ϱ� ���� ��ũ��Ʈ - ȣ����/������ ������ ����

// -- ȣ���� (Invoker) : CardManager.cs
// -- ������ (Receiver) : CardTouch.cs

// ī�带 2�� �����ؼ� Ŭ�� ��, ���� ����Ǵ� ����� CardManager Ŭ�������� ����

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
