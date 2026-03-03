using UnityEngine;

public class IdleState : IEnemyState
{
	public void Enter(EnemyMovement enemy)
	{
	}

	public void Tick(EnemyMovement enemy, float deltaTime)
	{
		if (enemy.HasValidPlayerTarget() && enemy.DistanceToPlayer() <= enemy.PlayerDetectionRange)
		{
			enemy.ChangeState(enemy.GetAttackState());
			return;
		}

		if (enemy.FindNearestCropTarget())
		{
			enemy.ChangeState(enemy.GetEatState());
			return;
		}

		Vector3 idlePoint = enemy.SpawnPoint;
		enemy.MoveTowards(idlePoint, deltaTime);
	}

	public void Exit(EnemyMovement enemy)
	{
	}
}
