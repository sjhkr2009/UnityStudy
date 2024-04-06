using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager2 : MonoBehaviour
{
    //팩토리 메서드 패턴 2

    //데이터베이스 접속에 관한 기술을 모두 파악하기는 어렵다.
    //데이터베이스를 잘 아는 사람이 각 접근방식을 만들어놓으면, 다른 사람들은 그걸 가져와서 사용한다.

    //인스펙터에서 데이터베이스 타입이 무엇인지만 정해주면, 접근방식을 몰라도 여기서 해당 데이터를 통한 작업이 가능하다.
    
    DatabaseFactory factory;
    Database db;

    private void Start()
    {
        factory = GetComponent<DatabaseFactory>();

        //어떤 데이터베이스가 사용 중인지 모르니, 팩토리에서 데이터베이스를 만들라고 지시하면 팩토리가 알맞은 접근방식대로 접근하여 만들어준다.
        db = factory.GenerateDatabase();

        //데이터베이스에 접속한다.
        db.ConnectDatabase();

        //접속한 데이터베이스를 이용해 업무처리를 한다. 표준 SQL 사용 시 업무 방식은 같을 것이다.
        db.InsertData();
        db.SelectData();
    }
}
