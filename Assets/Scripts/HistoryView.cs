using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HistoryView : MonoBehaviour
{
    [SerializeField] CardManager cardManager; // ī�� �Ŵ������� ��� �ҷ�����
    [SerializeField] GameObject historyContent; // ������
    [SerializeField] GameObject historyContainer; // ������ ���� object
    int attemptCount = 0;

    int successScore;
    int failureScore;
    int totalScore;

    public void ViewHistory()
    {
        Queue<ICommand> histories = cardManager.cardInvoker.historyQueue;

        // ���ھ�Ŵ������� ���� �ҷ�����
        successScore = GameManager.Instance.scoreManager.successScore;
        failureScore = GameManager.Instance.scoreManager.failureScore;

        // ��� Ž�� �� ����
        while (histories.Count >= 2) // ī�� 2���� dequeue ���� �ؾ� �ϹǷ�, 2�̻��� ��쿡�� ����
        {
            CardTouchCommand cmd1 = (CardTouchCommand)histories.Dequeue();
            CardTouchCommand cmd2 = (CardTouchCommand)histories.Dequeue();

            CreateContent(cmd1.GetCard(), cmd2.GetCard());
        }

        // view Ȱ��ȭ
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    // history contents ����
    private void CreateContent(CardTouch c1, CardTouch c2)
    {
        HistoryContent content = Instantiate(historyContent,historyContainer.transform).GetComponent<HistoryContent>();

        attemptCount++;
        Texture2D tex1 = c1.targetCard.cardData.CardImage;
        Texture2D tex2 = c2.targetCard.cardData.CardImage;
        content.card1.sprite = Sprite.Create(tex1, new Rect(0, 0, tex1.width, tex1.height), new Vector2(0.5f, 0.5f));
        content.card2.sprite = Sprite.Create(tex2, new Rect(0, 0, tex2.width, tex2.height), new Vector2(0.5f, 0.5f));
        content.stage.text = "Stage : "+c1.stageNumber;
        content.attemptCount.text = "Attempt : " + attemptCount;
        if(c1.targetCard.cardData.CardID == c2.targetCard.cardData.CardID)
        {
            totalScore += successScore;
            content.bar.color = new Color(0, .5f, 0, .3f);
            content.results.text = "Success";
            content.addScore.text = "+ "+successScore;
            content.totalScore.text = "total : " + totalScore;
        }
        else
        {
            if(totalScore>0) totalScore -= failureScore;
            content.bar.color = new Color(.4f, 0, 0, .3f);
            content.results.text = "Failure";
            content.addScore.text = "- "+(Mathf.Abs(failureScore)).ToString();
            content.totalScore.text = "total : " + totalScore;
        }

    }
}
