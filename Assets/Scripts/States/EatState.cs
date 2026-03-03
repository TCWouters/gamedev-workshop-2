using UnityEngine;

public class EatState : IEnemyState
{
	private float eatTimer;

	public void Enter(EnemyMovement enemy)
	{
		eatTimer = 0f;
	}

	public void Tick(EnemyMovement enemy, float deltaTime)
	{
		if (!enemy.HasValidCropTarget())
		{
			enemy.ChangeState(enemy.GetIdleState());
			return;
		}

		float distanceToCrop = enemy.DistanceToCrop();
		if (distanceToCrop > enemy.AttackRange)
		{
			enemy.MoveTowards(enemy.CropTarget.position, deltaTime);
			return;
		}

		eatTimer += deltaTime;
		if (eatTimer >= enemy.EatDuration)
		{
			Object.Destroy(enemy.CropTarget.gameObject);
			enemy.SetCropTarget(null);
			enemy.ChangeState(enemy.GetIdleState());
		}
	}

	public void Exit(EnemyMovement enemy)
	{
		eatTimer = 0f;
	}
}
