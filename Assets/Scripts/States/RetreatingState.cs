using UnityEngine;

public class RetreatingState : IEnemyState
{
	private float retreatTimer;
	private Vector3 retreatDestination;

	public void Enter(EnemyMovement enemy)
	{
		retreatTimer = 0f;

		Vector3 awayDirection = Vector3.back;
		if (enemy.HasValidPlayerTarget())
		{
			awayDirection = (enemy.transform.position - enemy.PlayerTarget.position).normalized;
			if (awayDirection.sqrMagnitude < 0.001f)
			{
				awayDirection = Random.insideUnitSphere;
				awayDirection.y = 0f;
				awayDirection.Normalize();
			}
		}

		retreatDestination = enemy.transform.position + awayDirection * enemy.RetreatDistance;
	}

	public void Tick(EnemyMovement enemy, float deltaTime)
	{
		retreatTimer += deltaTime;
		enemy.MoveTowards(retreatDestination, deltaTime);

		if (retreatTimer >= enemy.RetreatDuration)
		{
			enemy.ChangeState(enemy.GetIdleState());
		}
	}

	public void Exit(EnemyMovement enemy)
	{
		retreatTimer = 0f;
	}
}
