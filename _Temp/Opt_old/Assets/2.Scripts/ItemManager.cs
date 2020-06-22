using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    StageManager stageManager;

    public GameObject playerObject;
    Player player;
    List<GameObject> laserLaunchers;

    public Button itemButton;
    int itemCode;

    int item101Upgrade;
    float item101Speed;
    float item101Duration;

    void Start()
    {
        itemCode = PlayerPrefs.GetInt("SelectedItem");
        if(itemCode == 0)
        {
            itemCode = 101;
        }
        Debug.Log("Item Code: " + itemCode);

        UpgradeDataLoad();

        player = playerObject.GetComponent<Player>();
        stageManager = GetComponent<StageManager>();
    }

    void UpgradeDataLoad()
    {
        item101Upgrade = PlayerPrefs.GetInt("Item101Upgrade");
    }



    void Update()
    {
        laserLaunchers = GetComponent<LaserManager>().laserLaunchers;
    }

    public void OnClickItemUI()
    {
        stageManager.state = StageManager.State.ItemUsing;
        ItemActive(itemCode);
    }

    void ItemActive(int itemCode)
    {
        switch (itemCode)
        {
            case 101:
                Debug.Log("아이템 발동");
                StartCoroutine("ItemActive101");
                itemButton.interactable = false;
                stageManager.state = StageManager.State.Idle;
                break;
        }
    }

    IEnumerator ItemActive101()
    {
        switch (item101Upgrade)
        {
            case 0:
                item101Speed = 1.1f;
                item101Duration = 8f;
                break;
            case 1:
                item101Speed = 1.25f;
                item101Duration = 15f;
                break;
            case 2:
                item101Speed = 1.5f;
                item101Duration = 30f;
                break;
            case 3:
                item101Speed = 2f;
                item101Duration = 300f;
                break;
        }

        player.speed *= item101Speed;
        yield return new WaitForSeconds(item101Duration);

        Debug.Log("아이템 101 사용 종료");
        player.speed = player.originSpeed;
    }
}
