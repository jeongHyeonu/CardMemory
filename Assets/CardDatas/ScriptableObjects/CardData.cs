using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�� ������ ���� ����� ScriptableObject

// Q.   �� ����߳�? 
// A.   prefab�� Ŭ���� ������ �����ؼ� ����ϴ� ����� ȿ�������� ����. ScriptableObject�� �̿��� �����ϸ� �޸� �纻�� ������ �ʰ� ������ ������ �޸� ��뷮 ���� ȿ��

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
