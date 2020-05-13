using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : MonoBehaviour
{
    // 도전과제를 달성했을 때 알려주는 스크립트.
    // 참고) 도전과제는 게임의 거의 완성단계에서 추가되며, 본 게임에 거의 영향을 미치지 않고 별도의 UI와 보상으로 성취감을 주는 용도. 

    public Text achievementText;

    public void UnlockAchievement(string title)
    {
        Debug.Log("도전과제 해제! - " + title);
        achievementText.text = "도전과제 해제: " + title;
    }
}
