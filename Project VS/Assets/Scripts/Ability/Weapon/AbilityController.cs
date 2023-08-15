using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Scanner))]
public class AbilityController : MonoBehaviour {
    public Scanner Scanner { get; private set; }
    private List<AbilityBase> _allAbilities { get; } = new List<AbilityBase>();
    private List<IWeaponAbility> _weapons { get; } = new List<IWeaponAbility>();
    private List<ISkillAbility> _skills { get; } = new List<ISkillAbility>();
    
    public IReadOnlyList<AbilityBase> AllAbilities => _allAbilities;
    public IReadOnlyList<IWeaponAbility> Weapons => _weapons;
    public IReadOnlyList<ISkillAbility> Skills => _skills;
    public Transform FireTransform => GameManager.Player.View?.SpiritTransform ?? transform;

    private void Awake() {
        Scanner = GetComponent<Scanner>();
        GameManager.Ability = this;
        GameManager.EnemyScanner = Scanner;
    }

    private void Start() {
        AddOrUpgradeItem(AbilityIndex.SkillFireball);
        AddOrUpgradeItem(AbilityIndex.SkillChargeExplosion);
    }

    public Transform CreateDummyTransform(AbilityBase ability) => CreateDummyTransform(ability?.GetType().Name);
    private Transform CreateDummyTransform(string dummyObjectName) {
        if (string.IsNullOrEmpty(dummyObjectName)) {
            Debugger.Error("[ItemController.CreateDummyTransform] Parameter is Empty!!");
        }
        
        var dummy = new GameObject(dummyObjectName).transform;
        dummy.SetParent(transform);
        dummy.ResetTransform();

        return dummy;
    }

    private void AddItem(AbilityBase ability) {
        ability.Initialize(this);
        _allAbilities.Add(ability);
        if (ability is ISkillAbility skill) _skills.Add(skill);
        if (ability is IWeaponAbility weapon) _weapons.Add(weapon);
        SendChangeItemToOther(ability);
    }
    
    public bool RemoveItem<T>() where T : AbilityBase {
        var target = AllAbilities.FirstOrDefault(i => i is T);
        if (target == null) {
            Debugger.Error($"[ItemController.RemoveItem] Cannot Find: {typeof(T).Name}");
            return false;
        }
        
        target.Abandon();
        _allAbilities.Remove(target);
        if (target is ISkillAbility skill) _skills.Remove(skill);
        if (target is IWeaponAbility weapon) _weapons.Remove(weapon);
        return true;
    }

    public T GetItem<T>() where T : AbilityBase {
        return AllAbilities.FirstOrDefault(w => w is T) as T;
    }
    
    public AbilityBase GetItem(AbilityIndex abilityIndex) {
        return AllAbilities.FirstOrDefault(w => w.Index == abilityIndex);
    }

    public bool CanAddAbility(AbilityIndex abilityIndex) {
        return !AllAbilities.Any(a => a.Index == abilityIndex && a.Level >= AbilityDataContainer.LoadData(a.Index).maxLevel);
    }

    public AbilityBase AddOrUpgradeItem(AbilityIndex abilityIndex) {
        var item = GetItem(abilityIndex);
        if (item != null) {
            UpgradeItem(item);
            return item;
        }
        
        // TODO: 모든 무기 정보를 가진 데이터를 만들어서 WeaponIndex와 구현 클래스를 연결할 것 
        item = AbilityFactory.Create(abilityIndex);
        AddItem(item);
        return item;
    }

    private void UpgradeItem(AbilityBase ability) {
        ability.Upgrade();
        SendChangeItemToOther(ability);
    }

    private void SendChangeItemToOther(AbilityBase updatedAbility) {
        AllAbilities.ForEach(w => {
            if (w != updatedAbility) w.OnChangeOtherAbility(updatedAbility);
        });
        GameBroadcaster.CallUpdateItem(updatedAbility);
    }

    public int GetLevel(AbilityIndex abilityIndex) {
        return AllAbilities.FirstOrDefault(i => i.Index == abilityIndex)?.Level ?? 0;
    }
    
    public Vector2 GetRandomPositionInCameraView() {
        var cam = GameManager.CurrentCamera;
        if (cam == null) {
            Debugger.Error("[AbilityController.GetRandomPositionInCameraView] Main camera is not assigned!!");
            return Vector3.zero;
        }

        float x = Random.Range(0f, 1f);
        float y = Random.Range(0.05f, 0.95f);

        Vector3 viewportPoint = new Vector3(x, y, cam.nearClipPlane);
        Vector3 worldPoint = cam.ViewportToWorldPoint(viewportPoint);

        return worldPoint;
    }

    public bool IsInCameraView(Vector2 position) {
        var cam = GameManager.CurrentCamera;
        if (cam == null) {
            Debugger.Error("[AbilityController.GetRandomPositionInCameraView] Main camera is not assigned!!");
            return false;
        }
        
        Vector3 viewportPoint = new Vector3(0f, 0f);
        Vector3 worldMinPoint = cam.ViewportToWorldPoint(viewportPoint);

        viewportPoint = new Vector3(1f, 1f);
        Vector3 worldMaxPoint = cam.ViewportToWorldPoint(viewportPoint);

        if (position.x < worldMinPoint.x || position.y < worldMinPoint.y) return false;
        if (position.x > worldMaxPoint.x || position.y > worldMaxPoint.y) return false;

        return true;
    }

    private void Update() {
        if (GameManager.IsPause) return;
        
        var deltaTime = Time.deltaTime;
        Weapons.ForEach(w => w.OnEveryFrame(deltaTime));
    }

    public void Dispose() {
        _allAbilities.ForEach(a => a.Abandon());
        
        _allAbilities.Clear();
        _weapons.Clear();
        _skills.Clear();
    }
}
