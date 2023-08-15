public interface IGameListener {
    void OnGameStart();
    void OnDeadEnemy(EnemyStatus deadEnemy);
    void OnHitPlayer();
    void OnHitEnemy(DamageData data, EnemyStatus hitEnemy);
    void OnDeadPlayer();
    void OnLevelUp();
    void OnPauseGame();
    void OnResumeGame();
    void OnGainDropItem(DropItemIndex dropItemIndex);
    void OnSelectItem();
    void OnSkill1();
    void OnSkill2();
    void OnUpdateItem(AbilityBase updatedAbility);
    void OnEverySecond();
    void OnGameEnd(GameResult gameResult);
    void OnContinueGame();
}
