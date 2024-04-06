using System.Collections;
using System.Collections.Generic;
using Strategy;
using UnityEngine;

namespace TemplateMethod {
    public class Player08 : Player06 {
        [SerializeField] private Transform target;
        
        protected override void Update() {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(1)) {
                WeaponController.DoAttack(transform, target);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                WeaponController.DoSkillQ(transform, Vector3.zero);
            }
            if (Input.GetKeyDown(KeyCode.W)) {
                WeaponController.Phase();
            }
        }
    }
}

