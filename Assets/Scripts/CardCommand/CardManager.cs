using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ī�带 �����ϴ� Ŭ����, ī�� ����/���� ����, ��� ����(���� ó�� ��..)�� �� Ŭ������ ���
public class CardManager : MonoBehaviour 
{
    [SerializeField] public CardInvoker cardInvoker = new CardInvoker();
    [SerializeField] GameObject cardPrefab; // ī�� ������
    [SerializeField] List<CardData> cardData;
    [SerializeField] TextMeshProUGUI stageNumberText; // �������� ��ȣ

    List<Card> createdCards = new List<Card>();
    CardTouch selectedCard;

    public bool isWrong;
    float cooltime = 1f;

    public int stageNumber = 1;
    int maxStageNumber = 3;

    public int leftCardCount = 0; // ���� ī���
    int[,] stageRowCol = new int[3, 2] { {2,2},{2,4},{3,6} }; // �������� ����*����
    List<List<int>> selectedCardData = new List<List<int>>();

    // UX inner class
    Card_UX ux_apply = new Card_UX();

    private void Start()
    {
        // ī�带 ������ �̾Ƶд�
        for(int i = 0; i < maxStageNumber; i++)
        {
            List<int> selected = RandomSelectCard(stageRowCol[i,0] * stageRowCol[i,1]);
            selectedCardData.Add(selected);
        }

        // ��� �Ŀ� ī�� ���� �� ���� ����
        DOVirtual.DelayedCall(cooltime, ()=> {
            SpawnCards();
            GameManager.Instance.GameStart();
        });
    }

    public void SpawnCards() // �������� ��ȣ�� ���� ī�� ����
    {
        int _row = stageRowCol[stageNumber - 1, 0];
        int _col = stageRowCol[stageNumber - 1, 1];
        GenerateCards(_row, _col);
    }

    private List<int> RandomSelectCard(int _cnt)
    {
        List<int> selectedCardData = new List<int>(); // ���õ� ī�� ������ �ε��� �迭 (return ��)
        bool[] isSelected = new bool[cardData.Count]; // �ߺ� ���ſ� bool �迭 ����


        int cardCounts = _cnt / 2; // �������� �� ī��� ��, ���ݸ� �̴´�
        while (cardCounts != 0) // ī��� �� ���� �� ������ ����
        {
            int randomIdx = Random.Range(0, cardData.Count);
            if (isSelected[randomIdx]) continue; // �̹� �� ī����̸� �ٽ� �̱�

            isSelected[randomIdx] = true;
            selectedCardData.Add(randomIdx); // ī����̹Ƿ� List�� 2�� �ִ´�
            selectedCardData.Add(randomIdx); 
            cardCounts--;
        }

        // ���� ī��� �����ϰ� ����
        for(int i = 0; i < selectedCardData.Count; i++) {
            int rand1 = Random.Range(0, _cnt);
            int rand2 = Random.Range(0, _cnt);
            int temp = selectedCardData[rand1];
            selectedCardData[rand1] = selectedCardData[rand2];
            selectedCardData[rand2] = temp; 
        }

        // ���� ī�� ����Ʈ ����
        return selectedCardData;
    }

    private void GenerateCards(int row, int col)
    {
        // �ٸ����� Grid Layout Group ������Ʈ���� �� �ֹǷ�, ī��� ������ ���ָ� �˾Ƽ� x,y �ڵ� ���ĵ�
        // ���� �����Ǵ� ī�� ��ġ�� �ƹ� ���̳� �־ OK
        Quaternion rot = this.transform.rotation;
        Vector3 pos = this.transform.position;

        // Column ����ŭ �� ����
        this.GetComponent<GridLayoutGroup>().constraintCount = col; 

        // row * col ��ŭ ī�� ����
        for(int i = 0; i < row * col; i++)
        {
            Card c = Instantiate(cardPrefab, pos, rot, this.transform).GetComponent<Card>();
            ux_apply.SpawnUX(c); // ���� UX ����

            // �ش� ���������� �ش��ϴ� ī�� ������ ����
            c.cardData = cardData[selectedCardData[stageNumber-1][i]]; 

            // ī�忡 Ŭ�� �� ������ Strategy ����
            CardTouch ct = new CardTouch();
            ct.cm = this;
            c.SetCardStrategy(ct);
            createdCards.Add(c); // ������ Card �� list�� �߰�
        }

        leftCardCount = createdCards.Count;
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.FLAP_2); // ����
    }

    // 1) ī�� Ŭ����, �ش� Command �� CardInvoker ���� ť(historyQueue)�� Ǫ�õ�
    // 2) ���� �� ���� ó��, ������ ī����� ���� ��ġ�ϴ� ���, ���� ����
    public void RunCardCommand(CardTouch _card)
    {
        // ���� ���� ������¸� ���� X
        if (!GameManager.Instance.isGaming) return;

        // ī�� Ŭ�� ��, �ش� Command ����
        ICommand command = new CardTouchCommand(_card);
        cardInvoker.ExecuteCommand(command);

        // UX, ī�� ���� �� ������ ���� �� ��¦ ������ �̵�
        ux_apply.SelectUX(_card);

        // ���� or ���� ó��
        if (ReferenceEquals(selectedCard, null))
        {
            selectedCard = _card; // ī�� ���� ���� ���� ����, �� ī�� ����
        }
        else
        {
            int firstCardID = selectedCard.targetCard.cardData.CardID;
            int secondCardID = _card.targetCard.cardData.CardID;

            if (firstCardID == secondCardID) CorrectMatch(_card, selectedCard); // ī�� ID ��ġ�� ��� (=ī��� ��ġ)
            else NonMatch(_card, selectedCard); // ī�� ID ����ġ ��� (=ī��� ����ġ)

            selectedCard = null; // ������ Ŭ���� ī�� �ޱ� ���� null �� �ʱ�ȭ
        }
    }

    private void NextStage()
    {
        // �������� ��ȣ ����
        stageNumber++;
        stageNumberText.text = "Stage " + stageNumber;

        // ��Ÿ�� ��, ������ ī�带 ��� ���� �� ��� ��ٷȴٰ� ī�� �����
        DOVirtual.DelayedCall(cooltime, () =>
        {
            for (int i = 0; i < createdCards.Count; i++)
            {
                ux_apply.FadeAndDeleteUX(createdCards[i]);
            }
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.WHEEP); // ȿ����
            createdCards.Clear();
        }).OnComplete(() => DOVirtual.DelayedCall(cooltime, SpawnCards) );
    }

    public void AllClear()
    {
        GameManager.Instance.GameEnd(true);
    }

    private void CorrectMatch(CardTouch c1, CardTouch c2)
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.MATCH); // ����
        GameManager.Instance.PlusScore(); //����

        DOVirtual.DelayedCall(cooltime, () => AfterCorrectMatch(c1, c2));
        leftCardCount -= 2;
        if (leftCardCount == 0) // ī�带 �� ���� ���� ī�尡 ���� ���
        {
            if (stageNumber == maxStageNumber) AllClear(); // �������� �� Ŭ���� �� ���
            else NextStage(); // ���� �������� ���Ҵٸ� ���� ����������
        }
    }

    private void NonMatch(CardTouch c1, CardTouch c2)
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.NON_MATCH); // ����
        GameManager.Instance.MinusScore(); // ����

        isWrong = true;
        DOVirtual.DelayedCall(cooltime, () => AfterNonMatch(c1, c2));
    }


    private void AfterCorrectMatch(CardTouch c1, CardTouch c2)
    {
        ux_apply.SetOriginUX(c1, c2);
    }

    private void AfterNonMatch(CardTouch c1, CardTouch c2)
    {
        // ī�� ����ġ ��, ī�� ũ�� ������ �ǵ�����, �г�Ƽ ����(isWrong)�� �ٲ㼭 ī�带 �ٽ� Ŭ�� �����ϰ� ����
        isWrong = false;
        c1.Flip();
        c2.Flip();
        ux_apply.SetOriginUX(c1, c2);
    }


    // ī�� UX ó�� ���� Ŭ����
    class Card_UX
    {
        float uxTime = 0.5f;
        float originScale = 1f;
        float expandScale = 1.1f;
        float clickMove = -.01f;

        public void SelectUX(CardTouch c) // �� ī�� UX
        {
            c.targetCard.transform.DOScale(expandScale, uxTime);
            c.targetCard.transform.DOLocalMoveZ(clickMove, uxTime).SetRelative(true);
        }

        public void SetOriginUX(CardTouch c1, CardTouch c2) // ���� ���·� ���ư��� UX
        {
            c1.targetCard.transform.DOScale(originScale, uxTime);
            c1.targetCard.transform.DOLocalMoveZ(-clickMove, uxTime).SetRelative(true);
            c2.targetCard.transform.DOScale(originScale, uxTime);
            c2.targetCard.transform.DOLocalMoveZ(-clickMove, uxTime).SetRelative(true);
        }

        public void SpawnUX(Card c)
        {
            c.transform.DOScale(originScale, uxTime).From(0);
        }

        public void FadeAndDeleteUX(Card c)
        {
            c.cardRender.materials[0].DOFade(0, uxTime);
            c.cardRender.materials[1].DOFade(0, uxTime).OnComplete(() => { Destroy(c.gameObject); });
        }
    }
}

