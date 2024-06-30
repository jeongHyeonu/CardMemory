using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 데이터 정보 저장용 ScriptableObject

// Q.   왜 사용했나? 
// A.   prefab의 클래스 변수로 저장해서 사용하는 방식은 효율적이지 않음. ScriptableObject를 이용해 저장하면 메모리 사본을 만들지 않고 원본을 참조해 메모리 사용량 감소 효과

[CreateAssetMenu(fileName = "Card Data", menuName ="Scriptable Obejct/Card Data", order = int.MaxValue)]
public class CardData : ScriptableObject
{
    [SerializeField]
    private string cardName;
    public string CardName { get { return cardName; } }

    [SerializeField]
    private int cardID;
    public int CardID { get { return cardID;} }

    [SerializeField]
    private Texture2D cardImage;
    public Texture2D CardImage { get { return cardImage; } }
}
