using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class MonsterController : BaseController {
    private Coroutine aiPatrol;
    private Vector3Int destPos;

    public override CreatureState State {
        get => _state;
        protected set {
            if (_state == value) return;

            _state = value;
            UpdateAnimation();
            StopAiCoroutines();
        }
    }

    protected override void UpdateOnIdle() {
        base.UpdateOnIdle();

        if (aiPatrol == null) {
            aiPatrol = StartCoroutine(nameof(AiPatrol));
        }
    }

    protected override void MoveToNextPos() {
        // TODO: destPos까지 가는 길찾기 알고리즘으로 대체할 것

        var destDir = destPos - CellPos;

        if (destDir.x > 0) CurrentDir = MoveDir.Right;
        else if (destDir.x < 0) CurrentDir = MoveDir.Left;
        else if (destDir.y > 0) CurrentDir = MoveDir.Up;
        else if (destDir.y < 0) CurrentDir = MoveDir.Down;
        else CurrentDir = MoveDir.None;

        if (CurrentDir == MoveDir.None) {
            State = CreatureState.Idle;
            return;
        }

        var deltaPos = GetDeltaPos(CurrentDir);
        var nextPos = CellPos + deltaPos;
        
        if (!Director.Map.CanGo(nextPos) || !ReferenceEquals(Director.Object.Find(nextPos), null)) {
            State = CreatureState.Idle;
            return;
        }

        CellPos = nextPos;
    }

    IEnumerator AiPatrol() {
        int waitSeconds = Random.Range(1, 4);
        yield return new WaitForSeconds(waitSeconds);

        for (int i = 0; i < 10; i++) {
            int xRange = Random.Range(-3, 4);
            int yRange = Random.Range(-3, 4);
            var randPos = CellPos + new Vector3Int(xRange, yRange, 0);

            if (Director.Map.CanGo(randPos) && Director.Object.Find(randPos) == null) {
                destPos = randPos;
                State = CreatureState.Moving;
                yield break;
            }
        }

        State = CreatureState.Idle;
    }

    void StopAiCoroutines() {
        if (aiPatrol != null) {
            StopCoroutine(aiPatrol);
            aiPatrol = null;
        }
    }
}
