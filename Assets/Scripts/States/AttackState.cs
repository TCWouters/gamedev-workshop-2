public class AttackState : IEnemyState
{
	public void Enter(EnemyMovement enemy)
	{
	}

	public void Tick(EnemyMovement enemy, float deltaTime)
	{
		if (!enemy.HasValidPlayerTarget())
		{
			enemy.ChangeState(enemy.GetIdleState());
			return;
		}

		if (enemy.DistanceToPlayer() > enemy.PlayerDetectionRange * 1.3f)
		{
			enemy.ChangeState(enemy.GetIdleState());
			return;
		}

		enemy.MoveTowards(enemy.PlayerTarget.position, deltaTime);
		enemy.TryAttackPlayer();
	}

	public void Exit(EnemyMovement enemy)
	{
	}
}
