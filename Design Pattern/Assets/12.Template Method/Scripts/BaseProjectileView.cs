using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;

namespace TemplateMethod {
    public abstract class BaseProjectileView {
        public Object Projectile { get; protected set; }
        public Object FlashObject { get; protected set; }
        public Object HitObject { get; protected set; }
        
        public BaseProjectileView(string prefabRootName) {
            Projectile = Resources.Load(Path.Combine(prefabRootName, "Projectile"));
            FlashObject = Resources.Load(Path.Combine(prefabRootName, "Flash"));
            HitObject = Resources.Load(Path.Combine(prefabRootName, "Hit"));
        }

        protected Transform InstantiateObject(Object origin, Transform parent) {
            var go = Object.Instantiate(origin, parent, true) as GameObject;
            var tr = go.transform;
            tr.localScale = Vector3.one;
            tr.localPosition = Vector3.zero;

            return tr;
        }

        public void Run(Transform origin, Transform target) {
            var obj = InstantiateObject(Projectile, origin);
            
            var seq = MoveToTarget(obj, target);
            seq.OnStart(() => OnStart(origin))
                .SetId(obj)
                .OnComplete(() => {
                    OnComplete(origin, target);
                    Object.Destroy(obj.gameObject);
                });
        }

        protected void OnStart(Transform origin) {
            var flash = InstantiateObject(FlashObject, origin);
            DOVirtual.DelayedCall(3f, () => Object.Destroy(flash.gameObject));
        } 

        protected virtual Sequence MoveToTarget(Transform projectile, Transform target) {
            return DOTween.Sequence()
                .Append(projectile.DOMove(target.position, 0.75f));
        }

        protected virtual void OnComplete(Transform origin, Transform target) {
            var hit = InstantiateObject(HitObject, target);
            hit.localScale = Vector3.one * 0.3f;
            DOVirtual.DelayedCall(3f, () => Object.Destroy(hit.gameObject));
        }
    }
}
