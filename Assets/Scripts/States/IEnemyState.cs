public interface IEnemyState
{
    void Enter(EnemyMovement enemy);
    void Tick(EnemyMovement enemy, float deltaTime);
    void Exit(EnemyMovement enemy);
}
