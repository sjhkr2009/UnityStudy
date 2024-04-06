using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TemplateMethod {
    public class CrescendumProjectileView : BaseProjectileView {
        public CrescendumProjectileView() : base("Crescendum") { }
        
        protected override Sequence MoveToTarget(Transform projectile, Transform target) {
            projectile.localScale = Vector3.one * 0.3f;
            var ret = DOTween.Sequence();
            ret.Append(projectile.DOMove(target.position, 0.15f).SetEase(Ease.Linear));

            return ret;
        }
        
        protected override void OnComplete(Transform origin, Transform target) {
            base.OnComplete(origin, target);
            var obj = InstantiateObject(Projectile, target);
            obj.localScale = Vector3.one * 0.3f;
            MoveToTarget(obj, origin)
                .Append(obj.DOScale(0.1f, 0.15f))
                .OnComplete(() => Object.Destroy(obj.gameObject));
        }
    }
}
