using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 카드를 관리하는 클래스, 카드 생성/제거 관리, 기능 구현(로직 처리 등..)도 이 클래스가 담당
public class CardManager : MonoBehaviour 
{
    [SerializeField] public CardInvoker cardInvoker = new CardInvoker();
    [SerializeField] GameObject cardPrefab; // 카드 프리팹
    [SerializeField] List<CardData> cardData;
    [SerializeField] TextMeshProUGUI stageNumberText; // 스테이지 번호

    List<Card> createdCards = new List<Card>();
    CardTouch selectedCard;

    public bool isWrong;
    float cooltime = 1f;

    public int stageNumber = 1;
    int maxStageNumber = 3;

    public int leftCardCount = 0; // 남은 카드수
    int[,] stageRowCol = new int[3, 2] { {2,2},{2,4},{3,6} }; // 스테이지 가로*세로
    List<List<int>> selectedCardData = new List<List<int>>();

    // UX inner class
    Card_UX ux_apply = new Card_UX();

    private void Start()
    {
        // 카드를 사전에 뽑아둔다
        for(int i = 0; i < maxStageNumber; i++)
        {
            List<int> selected = RandomSelectCard(stageRowCol[i,0] * stageRowCol[i,1]);
            selectedCardData.Add(selected);
        }

        // 잠시 후에 카드 생성 및 게임 시작
        DOVirtual.DelayedCall(cooltime, ()=> {
            SpawnCards();
            GameManager.Instance.GameStart();
        });
    }

    public void SpawnCards() // 스테이지 번호에 따라 카드 생성
    {
        int _row = stageRowCol[stageNumber - 1, 0];
        int _col = stageRowCol[stageNumber - 1, 1];
        GenerateCards(_row, _col);
    }

    private List<int> RandomSelectCard(int _cnt)
    {
        List<int> selectedCardData = new List<int>(); // 선택된 카드 데이터 인덱스 배열 (return 값)
        bool[] isSelected = new bool[cardData.Count]; // 중복 제거용 bool 배열 변수


        int cardCounts = _cnt / 2; // 랜덤으로 고를 카드쌍 수, 절반만 뽑는다
        while (cardCounts != 0) // 카드쌍 다 선택 될 때까지 실행
        {
            int randomIdx = Random.Range(0, cardData.Count);
            if (isSelected[randomIdx]) continue; // 이미 고른 카드쌍이면 다시 뽑기

            isSelected[randomIdx] = true;
            selectedCardData.Add(randomIdx); // 카드쌍이므로 List에 2번 넣는다
            selectedCardData.Add(randomIdx); 
            cardCounts--;
        }

        // 뽑은 카드쌍 랜덤하게 섞기
        for(int i = 0; i < selectedCardData.Count; i++) {
            int rand1 = Random.Range(0, _cnt);
            int rand2 = Random.Range(0, _cnt);
            int temp = selectedCardData[rand1];
            selectedCardData[rand1] = selectedCardData[rand2];
            selectedCardData[rand2] = temp; 
        }

        // 뽑은 카드 리스트 리턴
        return selectedCardData;
    }

    private void GenerateCards(int row, int col)
    {
        // 줄맞춤은 Grid Layout Group 컴포넌트에서 해 주므로, 카드는 생성만 해주면 알아서 x,y 자동 정렬됨
        // 따라서 생성되는 카드 위치는 아무 값이나 넣어도 OK
        Quaternion rot = this.transform.rotation;
        Vector3 pos = this.transform.position;

        // Column 수만큼 줄 정렬
        this.GetComponent<GridLayoutGroup>().constraintCount = col; 

        // row * col 만큼 카드 생성
        for(int i = 0; i < row * col; i++)
        {
            Card c = Instantiate(cardPrefab, pos, rot, this.transform).GetComponent<Card>();
            ux_apply.SpawnUX(c); // 스폰 UX 적용

            // 해당 스테이지에 해당하는 카드 데이터 적용
            c.cardData = cardData[selectedCardData[stageNumber-1][i]]; 

            // 카드에 클릭 시 실행할 Strategy 적용
            CardTouch ct = new CardTouch();
            ct.cm = this;
            c.SetCardStrategy(ct);
            createdCards.Add(c); // 생성된 Card 는 list에 추가
        }

        leftCardCount = createdCards.Count;
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.FLAP_2); // 사운드
    }

    // 1) 카드 클릭시, 해당 Command 가 CardInvoker 내의 큐(historyQueue)에 푸시됨
    // 2) 정답 및 오답 처리, 선택한 카드쌍이 서로 일치하는 경우, 점수 증가
    public void RunCardCommand(CardTouch _card)
    {
        // 만약 게임 종료상태면 실행 X
        if (!GameManager.Instance.isGaming) return;

        // 카드 클릭 시, 해당 Command 실행
        ICommand command = new CardTouchCommand(_card);
        cardInvoker.ExecuteCommand(command);

        // UX, 카드 선택 시 스케일 증가 및 살짝 앞으로 이동
        ux_apply.SelectUX(_card);

        // 정답 or 오답 처리
        if (ReferenceEquals(selectedCard, null))
        {
            selectedCard = _card; // 카드 선택 하지 않은 경우면, 그 카드 선택
        }
        else
        {
            int firstCardID = selectedCard.targetCard.cardData.CardID;
            int secondCardID = _card.targetCard.cardData.CardID;

            if (firstCardID == secondCardID) CorrectMatch(_card, selectedCard); // 카드 ID 일치할 경우 (=카드쌍 일치)
            else NonMatch(_card, selectedCard); // 카드 ID 불일치 경우 (=카드쌍 불일치)

            selectedCard = null; // 다음에 클릭한 카드 받기 위해 null 로 초기화
        }
    }

    private void NextStage()
    {
        // 스테이지 번호 증가
        stageNumber++;
        stageNumberText.text = "Stage " + stageNumber;

        // 쿨타임 뒤, 생성된 카드를 모두 없앤 후 잠시 기다렸다가 카드 재생성
        DOVirtual.DelayedCall(cooltime, () =>
        {
            for (int i = 0; i < createdCards.Count; i++)
            {
                ux_apply.FadeAndDeleteUX(createdCards[i]);
            }
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.WHEEP); // 효과음
            createdCards.Clear();
        }).OnComplete(() => DOVirtual.DelayedCall(cooltime, SpawnCards) );
    }

    public void AllClear()
    {
        GameManager.Instance.GameEnd(true);
    }

    private void CorrectMatch(CardTouch c1, CardTouch c2)
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.MATCH); // 사운드
        GameManager.Instance.PlusScore(); //점수

        DOVirtual.DelayedCall(cooltime, () => AfterCorrectMatch(c1, c2));
        leftCardCount -= 2;
        if (leftCardCount == 0) // 카드를 다 맞춰 남은 카드가 없는 경우
        {
            if (stageNumber == maxStageNumber) AllClear(); // 스테이지 다 클리어 한 경우
            else NextStage(); // 아직 스테이지 남았다면 다음 스테이지로
        }
    }

    private void NonMatch(CardTouch c1, CardTouch c2)
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.NON_MATCH); // 사운드
        GameManager.Instance.MinusScore(); // 점수

        isWrong = true;
        DOVirtual.DelayedCall(cooltime, () => AfterNonMatch(c1, c2));
    }


    private void AfterCorrectMatch(CardTouch c1, CardTouch c2)
    {
        ux_apply.SetOriginUX(c1, c2);
    }

    private void AfterNonMatch(CardTouch c1, CardTouch c2)
    {
        // 카드 불일치 시, 카드 크기 원래로 되돌리고, 패널티 변수(isWrong)를 바꿔서 카드를 다시 클릭 가능하게 변경
        isWrong = false;
        c1.Flip();
        c2.Flip();
        ux_apply.SetOriginUX(c1, c2);
    }


    // 카드 UX 처리 내부 클래스
    class Card_UX
    {
        float uxTime = 0.5f;
        float originScale = 1f;
        float expandScale = 1.1f;
        float clickMove = -.01f;

        public void SelectUX(CardTouch c) // 고른 카드 UX
        {
            c.targetCard.transform.DOScale(expandScale, uxTime);
            c.targetCard.transform.DOLocalMoveZ(clickMove, uxTime).SetRelative(true);
        }

        public void SetOriginUX(CardTouch c1, CardTouch c2) // 원래 형태로 돌아가는 UX
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

