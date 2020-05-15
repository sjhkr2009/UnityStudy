using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Database
{
    public string name;
    public double rows;

    //데이터베이스마다 접근 방식이 다르므로, 해당 데이터베이스에서 접근 방식을 구체적으로 구현해 주어야 한다.
    public abstract void ConnectDatabase();

    //데이터의 삽입과 선택은 방식이 같으므로, 공통으로 사용하면 된다.
    public void InsertData()
    {
        Debug.Log($"{name} 에 데이터를 추가했습니다.");
    }

    public void SelectData()
    {
        Debug.Log($"{name} 에서 데이터를 {rows}개 조회했습니다.");
    }
}

public class MySQL : Database
{
    public MySQL()
    {
        name = nameof(MySQL);
        rows = 20;
    }

    public override void ConnectDatabase()
    {
        Debug.Log($"{name} 에 접속했습니다.");
    }
}

public class Oracle : Database
{
    public Oracle()
    {
        name = nameof(Oracle);
        rows = 10;
    }

    public override void ConnectDatabase()
    {
        Debug.Log($"{name} 에 접속했습니다.");
    }
}

public class Informix : Database
{
    public Informix()
    {
        name = nameof(Informix);
        rows = 40;
    }

    public override void ConnectDatabase()
    {
        Debug.Log($"{name} 에 접속했습니다.");
    }
}