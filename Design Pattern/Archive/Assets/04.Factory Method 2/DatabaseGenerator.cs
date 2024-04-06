using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseGenerator : DatabaseFactory
{
    public DBType dbType = DBType.MySQL; //타입만 바꾸면 해당 데이터베이스에 맞게 접근 방식이 바뀐다.

    public override Database GenerateDatabase()
    {
        //어떤 데이터베이스를 사용할지는 사정에 따라 바뀔 수 있으므로, 구축한 정보를 지우지 않고 재사용에 대비한다.

        switch (dbType)
        {
            case DBType.MySQL:
                Debug.Log("MySQL 사용");
                return new MySQL();
            case DBType.Oracle:
                Debug.Log("Oracle 사용");
                return new Oracle();
            case DBType.Informix:
                Debug.Log("Informix 사용");
                return new Informix();
            default:
                return null;
        }
    }
}
