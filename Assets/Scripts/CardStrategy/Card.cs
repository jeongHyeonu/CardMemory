using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public CardData cardData; // ��ũ���ͺ� ������Ʈ, CardManager���� �������� ��������
    private CardStrategy cardStrategy;
    public Renderer cardRender ; // ī�� �޸� ��������Ʈ ������ object

    // ī���� �ൿ(����) ����
    public void SetCardStrategy(CardStrategy _strategy)
    {
        this.cardStrategy = _strategy;
        cardStrategy.CardInit(this);
        SetCardImage();
    }

    // ī���� �ൿ(����) ����
    public CardStrategy GetCardStrategy()
    {
        return cardStrategy;
    }

    // ī�� �̹��� ���� - ���͸��� ����(��Ÿ��) ����
    private void SetCardImage()
    {
        // ���� ���͸���
        Material[] originMat = cardRender.materials;

        Material card_back = Instantiate(originMat[0]); // ī�� �޸�
        Material card_front = Instantiate(originMat[1]); // ī�� �ո�
        card_front.mainTexture = cardData.CardImage;

        // ���� ������ ���͸���
        Material[] newMat = { card_back, card_front };

        cardRender.materials = newMat;
    }

    // ī�� Ŭ�� ��, ���� ����Ǵ� ����� ������ CardStrategy�� ���� ����
    // Ŭ�� ������ alpha=0�� �̹����� ���� Image ������Ʈ�� ���� Click ����
    public void OnPointerClick(PointerEventData eventData)
    {
        cardStrategy.CardBehave();
    }
}
