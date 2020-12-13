using Godot;
using System;

public class Enemy : Character
{
	public bool attackState;
	public float changeDirectionTimer; // .5 - 3s
	public float stopDuration; // -5 ~ 2s
	public int direction;
	public int enemyIndex;
	public int[] speeds = new int[] {
		30,
		30,
		30,
		20,
		40,
		20,
		40,
		40,
		30,
	};
	public override int Speed { get { return speeds[enemyIndex]; } }
	public override void _Ready()
	{
		base._Ready();
		var rnd = new RandomNumberGenerator();
		rnd.Randomize();
		this.Position = new Vector2(rnd.Randf() * 2000f - 1000f, rnd.Randf() * 2000f - 1000f);
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		if(attackDuration <= 0f && !isAlive)
		{
			this.money *= 2;
			this.isAlive = true;
			var rnd = new RandomNumberGenerator();
			rnd.Randomize();

			this.Position = new Vector2(rnd.Randf() * 2000f - 1000f, rnd.Randf() * 2000f - 1000f);
		}
		if(attackDuration > 0) return;

		changeDirectionTimer -= delta;
		
		if(changeDirectionTimer <= 0f)
		{
			var rnd = new RandomNumberGenerator();
			rnd.Seed = (ulong)System.DateTime.Now.Ticks;
			changeDirectionTimer = rnd.Randf() * 2.5f + 0.5f;

			stopDuration = rnd.Randf() *7f - 3f;
			direction = (int)(rnd.Randf() * 100000f) % 4 +1;
		}
	}
	public override void GetInput()
	{
		if(attackDuration > 0) return;

		lastVelocity = velocity;
		velocity = Vector2.Zero;

		if(changeDirectionTimer >= stopDuration)
		{
			switch(direction)
			{
				case 1 :
					velocity.x = 1;	
					SetAnimationState(1);
					break;
				case 2 :
					velocity.x = -1;	
					SetAnimationState(2);
					break;
				case 3 :
					velocity.y = -1;	
					SetAnimationState(3);
					break;
				case 4 :
					velocity.y = 1;	
					SetAnimationState(4);
					break;
			}
		}

		if(velocity == Vector2.Zero)
		{
			if(lastVelocity.x != 0)
				SetAnimationState(0);
			if(lastVelocity.y < 0)
				SetAnimationState(5);
			if(lastVelocity.y > 0)
				SetAnimationState(6);
		}

		velocity = velocity.Normalized() * Speed;
	}
	
}
