using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBgController : MonoBehaviour
{
    [SerializeField] Image bgImage;

    [SerializeField] Sprite bgDay;
    [SerializeField] Sprite bgEvening;
    [SerializeField] Sprite bgNight;

    /// <summary>
    /// 배경이 낮에서 저녁으로 넘어가는 시간 (n시 0분)
    /// </summary>
    const int DayToEven = 17;
    /// <summary>
    /// 배경이 저녁에서 밤으로 넘어가는 시간 (n시 0분)
    /// </summary>
    const int EvenToNight = 20;
    /// <summary>
    /// 배경이 밤에서 새벽으로 넘어가는 시간 (n시 0분)
    /// </summary>
    const int NightToEven = 5;
    /// <summary>
    /// 배경이 새벽에서 낮으로 넘어가는 시간 (n시 0분)
    /// </summary>
    const int EvenToDay = 8;


    void Start()
    {
        if (bgImage == null)
            bgImage = GameObject.Find("bgImage").GetComponent<Image>();

        if (bgDay == null)
            bgDay = Resources.Load<Sprite>("Sprites/title/start_bg01_day");

        if (bgEvening == null)
            bgEvening = Resources.Load<Sprite>("Sprites/title/start_bg02_evening");

        if(bgNight == null)
            bgNight = Resources.Load<Sprite>("Sprites/title/start_bg03_night");

        SetBgImage();
        StartCoroutine(nameof(BackGroundCheck));
    }

	private void OnDestroy()
	{
        StopAllCoroutines();
	}

	void SetBgImage()
	{
        int nowHour = System.DateTime.Now.Hour;

        if (nowHour < NightToEven || nowHour >= EvenToNight)
            bgImage.sprite = bgNight;
        else if ((nowHour < EvenToNight && nowHour >= DayToEven) || (nowHour < EvenToDay && nowHour >= NightToEven))
            bgImage.sprite = bgEvening;
        else if (nowHour >= EvenToDay && nowHour < DayToEven)
            bgImage.sprite = bgDay;
        else
            Debug.Log("시간 설정 오류입니다. 이 메시지를 발견한다면 개발자에게 얘기해 주세요.");

    }

    IEnumerator BackGroundCheck()
	{
        while (bgImage != null)
		{
            yield return new WaitForSeconds(30f);
            SetBgImage();
        }
	}

}
