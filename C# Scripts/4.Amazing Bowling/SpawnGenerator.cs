using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour
{
    // 박스(프롭)들을 랜덤하게 생성시키는 컴포넌트

    private BoxCollider area; //범위를 지정해주기 위함 (콜라이더의 충돌 기능을 쓰려는 것은 아님)
    public GameObject[] propPrefabs; //생성할 프롭들을 넣을 공간
    public int count = 100;

    // 프롭의 HP가 0이 되면, 파괴(Destroy)한 후 매번 생성하지 않고, 비활성화한 후 다음 게임에서 다시 재배치할 것이다.
    // 따라서 프롭들을 리스트에 넣어두고 비활성화/활성화를 조절한다.
    private List<GameObject> props = new List<GameObject>();

    void Start()
    {
        area = GetComponent<BoxCollider>();

        for (int i = 0; i< count; i++) //count의 개수만큼 반복한다.
        {
            //생성용 함수 실행.
            Spawn();
        }

        area.enabled = false; //박스 생성 후에는 불필요한 콜라이더를 끈다. (.enable: 오브젝트에 적용된 컴포넌트(스크립트 포함)의 활성화/비활성화 여부를 바꾼다)
        //어차피 파괴/재생성하지 않을 것이니 최초 생성 후에는 필요 없으며, 남아 있으면 불필요한 충돌을 일으킬 수 있다.
    }

    private void Spawn()
    {
        int selection = Random.Range(0, propPrefabs.Length); // 프리팹 중 어느 것을 생성할 지 랜덤으로 고른다. 정수(f가 안 붙은 수)로 입력 시 정수만 반환된다.
        GameObject selectedPrefab = propPrefabs[selection]; //선택한 유형의 프리팹을 가져온다.
        Vector3 spawnPos = GetRandomPosition(); //위치 생성 함수로 랜덤 위치를 하나 생성한다.

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity); //해당 위치에 선택한 유형의 프리팹 생성. Quaternion.identity는 기본 회전값(0,0,0)
        props.Add(instance); //생성된 프롭을 리스트에 추가.
    }

    private Vector3 GetRandomPosition() //랜덤한 위치값을 반환하는 함수
    {
        Vector3 basePosition = transform.position; //현재 이 오브젝트의 위치(중심부 위치, 즉 (0,0,0)이다.)
        Vector3 size = area.size; //콜라이더의 크기

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f); //박스가 생성될 위치의 x값은, 오브젝트 중심 ± 콜라이더 크기의 절반. 즉 콜라이더 범위와 같게 한다.
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ); //생성될 위치를 반환
        return spawnPos;
    }
    

    public void Reset() //시작 시 프롭들을 재배치하는 함수. 이후 게임 매니저가 게임 시작 시마다 실행시킬 것이다. 다른 스크립트에서 쓸 것이니 public으로 선언.
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].transform.position = GetRandomPosition(); //위치 지정
            props[i].SetActive(true); //파괴(비활성화)된 프롭 모두 활성화
        }
    }
}
