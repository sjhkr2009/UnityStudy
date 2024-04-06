using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DBType
{
    MySQL,
    Oracle,
    Informix
}

public abstract class DatabaseFactory : MonoBehaviour
{
    public abstract Database GenerateDatabase();
}
