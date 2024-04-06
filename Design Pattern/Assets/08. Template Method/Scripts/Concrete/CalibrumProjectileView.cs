using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TemplateMethod {
    public class CalibrumProjectileView : BaseProjectileView {
        public CalibrumProjectileView() : base("Calibrum") { }
        
        protected override Sequence MoveToTarget(Transform projectile, Transform target) {
            return DOTween.Sequence()
                .Append(projectile.DOMove(target.position, 0.25f).SetEase(Ease.Linear));
        }
    }
}
