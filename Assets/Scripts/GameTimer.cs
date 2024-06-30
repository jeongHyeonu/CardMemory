using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float maxTime = 60f;
    [SerializeField] float remainTime;
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] Slider slider;

    Tween sliderTween, timeTween;

    int minute;
    int second;

    public void StartTimer()
    {
        //StartCoroutine(TimerRoutine());
        remainTime = maxTime;

        // DOTween�� ����Ͽ� �����̴��� value�� 0���� �ִϸ��̼�
        sliderTween =  slider.DOValue(0, maxTime).SetEase(Ease.Linear).From(1);

        // DOTween�� ����� Ÿ�̸� �ؽ�Ʈ 0����
        timeTween = DOTween.To(() => remainTime, x => remainTime = x, 0, maxTime).SetEase(Ease.Linear).OnUpdate(() =>
        {
            minute = (int)remainTime / 60;
            second = (int)remainTime % 60;
            timerTxt.text = minute.ToString("0") + " : " + second.ToString("00");
        }).OnComplete(() =>
        {
            timerTxt.text = "0 : 00"; // Ÿ�̸� �Ϸ� �� 0���� ǥ��
            GameManager.Instance.GameEnd(false);
        });
    }

    public void StopTimer()
    {
        sliderTween.Kill();
        timeTween.Kill();
    }
}
