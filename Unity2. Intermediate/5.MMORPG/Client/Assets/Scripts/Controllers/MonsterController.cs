using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class MonsterController : BaseController {
    private Coroutine aiPatrol;
    private Coroutine aiSearch;
    WaitForSeconds searchInterval = new WaitForSeconds(1);

    private Vector3Int destPos;
    private BaseController target;
    private float searchRange = 5f;

    public override float Speed { get; protected set; } = 2f;

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
        
        if (aiSearch == null) {
            aiSearch = StartCoroutine(nameof(AiSearch));
        }
    }

    protected override void MoveToNextPos() {
        if (target) destPos = target.CellPos;

        var path = Director.Map.FindPath(CellPos, destPos, true);
        // 길을 못 찾았거나, 대상이 너무 멀어지면 중단
        if (path.Count <= 1 || path.Count > 10) {
            target = null;
            State = CreatureState.Idle;
            return;
        }

        var destDir = path[1] - CellPos;

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

    IEnumerator AiSearch() {
        while (true) {
            yield return searchInterval;

            if (target) continue;

            target = Director.Object.FindIf<BaseController>(controller => {
                var distance = (controller.CellPos - CellPos).magnitude;
                return distance < searchRange;
            });
        }
    }

    void StopAiCoroutines() {
        if (aiPatrol != null) {
            StopCoroutine(aiPatrol);
            aiPatrol = null;
        }
        
        if (aiSearch != null) {
            StopCoroutine(aiSearch);
            aiSearch = null;
        }
    }
}
