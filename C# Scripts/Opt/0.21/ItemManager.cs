using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject player;
    List<GameObject> laserLaunchers;
    int itemCode;

    void Start()
    {
        itemCode = PlayerPrefs.GetInt("SelectedItem");
        if(itemCode == 0)
        {
            itemCode = 101;
        }
        Debug.Log("Item Code: " + itemCode);
    }

    
    void Update()
    {
        laserLaunchers = GetComponent<LaserManager>().laserLaunchers;
    }

    public void OnClickItemUI()
    {
        StageManager.instance.state = StageManager.State.ItemUsing;
        ItemActive(itemCode);
    }

    void ItemActive(int itemCode)
    {
        switch (itemCode)
        {
            case 101:
                Debug.Log("아이템 발동");
                StageManager.instance.state = StageManager.State.Idle;
                break;
        }
    }
}
