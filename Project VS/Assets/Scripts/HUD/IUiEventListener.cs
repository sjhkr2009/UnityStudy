using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUiEventListener<T> {
    void InvokeEvent(T param);
}
