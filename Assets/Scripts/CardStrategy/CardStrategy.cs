// 카드의 행동, 카드의 strategy를 설정하는 interface

public interface CardStrategy
{
    void CardBehave();
    void CardInit(Card card);
}
