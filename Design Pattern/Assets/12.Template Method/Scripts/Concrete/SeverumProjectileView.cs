using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TemplateMethod {
    public class SeverumProjectileView : BaseProjectileView {
        public SeverumProjectileView() : base("Severum") { }
        
        protected override Sequence MoveToTarget(Transform projectile, Transform target) {
            var ret = DOTween.Sequence().SetId(projectile);
            ret.Append(projectile.DOMove(target.position, 0.08f).SetEase(Ease.InQuad));

            return ret;
        }
    }
}
