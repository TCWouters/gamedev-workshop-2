using UnityEngine;

public class IdleState : IEnemyState
{
	private float orbitAngle;

	public void Enter(EnemyMovement enemy)
	{
		orbitAngle = Random.Range(0f, Mathf.PI * 2f);
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

		orbitAngle += enemy.IdleCircleSpeed * deltaTime;
		Vector3 orbitOffset = new Vector3(
			Mathf.Cos(orbitAngle) * enemy.IdleCircleRadius,
			0f,
			Mathf.Sin(orbitAngle) * enemy.IdleCircleRadius);
		Vector3 tangent = new Vector3(
			-Mathf.Sin(orbitAngle),
			0f,
			Mathf.Cos(orbitAngle));

		Vector3 idlePoint = enemy.GetIdleAreaCenter() + orbitOffset;
		idlePoint = enemy.ClampToIdleArea(idlePoint);
		enemy.MoveTowards(idlePoint, deltaTime);
		enemy.RotateTowards(tangent, deltaTime);
	}

	public void Exit(EnemyMovement enemy)
	{
	}
}
