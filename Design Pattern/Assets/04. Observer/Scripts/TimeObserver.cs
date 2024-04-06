using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observer {
    public abstract class TimeObserver : MonoBehaviour, IObserver<float> {
        public abstract void OnUpdate(float deltaTime);

        protected virtual void OnEnable() {
            TimerSubject.Instance.AddObserver(this);
        }

        protected virtual void OnDisable() {
            TimerSubject.Instance.RemoveObserver(this);
        }
    }
}