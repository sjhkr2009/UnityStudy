using System;
using UnityEngine;

namespace Observer {
    public class TimerSubject : MonoBehaviour, ISubject<float> {
        private event Action<float> OnUpdate;

        private static TimerSubject instance;
        public static TimerSubject Instance { get { Init(); return instance; } }

        private static void Init() {
            if (instance) return;

            instance = FindObjectOfType<TimerSubject>();
            if (instance) return;

            var go = new GameObject("Timer Subject");
            instance = go.AddComponent<TimerSubject>();
        }
        
        public void AddObserver(IObserver<float> observer) {
            OnUpdate += observer.OnUpdate;
        }

        public void RemoveObserver(IObserver<float> observer) {
            OnUpdate -= observer.OnUpdate;
        }

        private void Update() {
            Notify();
        }

        public void Notify() {
            OnUpdate?.Invoke(Time.deltaTime);
        }
    }
}
