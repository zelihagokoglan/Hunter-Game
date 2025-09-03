using System;

public static class StaticEvents
{
    public static event Action EnemyDied;       // Her düşman öldüğünde
    public static event Action AllEnemiesDead;  // Son düşman öldüğünde (bir kez)

    public static void RaiseEnemyDied()      => EnemyDied?.Invoke();
    public static void RaiseAllEnemiesDead() => AllEnemiesDead?.Invoke();
}