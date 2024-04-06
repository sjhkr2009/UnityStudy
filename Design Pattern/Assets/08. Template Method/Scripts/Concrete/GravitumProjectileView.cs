using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TemplateMethod {
    public class GravitumProjectileView : BaseProjectileView {
        public GravitumProjectileView() : base("Gravitum") { }

        protected override Sequence MoveToTarget(Transform projectile, Transform target) {
            projectile.localScale *= 0.5f;
            return base.MoveToTarget(projectile, target);
        }
    }
}
