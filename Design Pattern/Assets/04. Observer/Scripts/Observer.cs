using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observer {
    public interface IObserver<T> {
        void OnUpdate(T param);
    }
}
