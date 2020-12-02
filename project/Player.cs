using Godot;
using System;

public class Player : Character
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		base._Ready();
		this.money = 30;
		label.Text = this.money.ToString() + "g";
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		if(attackDuration <= 0f && !isAlive)
		{
			this.money /= 2;
			label.Text = this.money.ToString() + "g";
			this.isAlive = true;
			var rnd = new RandomNumberGenerator();
			rnd.Randomize();

			this.Position = new Vector2(rnd.Randf() * 2000f - 1000f, rnd.Randf() * 2000f - 1000f);
		}
	}
	
	public override void GetInput()
	{
		if(attackDuration > 0) return;

		lastVelocity = velocity;
		velocity = Vector2.Zero;

		if(Input.IsActionPressed("ui_right"))
		{
			velocity.x = 1;	
			SetAnimationState(1);
		}
		else if(Input.IsActionPressed("ui_left"))
		{
			velocity.x = -1;	
			SetAnimationState(2);
		}
		else if(Input.IsActionPressed("ui_up"))
		{
			velocity.y = -1;	
			SetAnimationState(3);
		}
		else if(Input.IsActionPressed("ui_down"))
		{
			velocity.y = 1;	
			SetAnimationState(4);
		}
		else if(velocity == Vector2.Zero)
		{
			if(lastVelocity.x != 0)
				SetAnimationState(0);
			if(lastVelocity.y < 0)
				SetAnimationState(5);
			if(lastVelocity.y >= 0)
				SetAnimationState(6);
		}

		
		

		velocity = velocity.Normalized() * Speed;
	}
	public override void _PhysicsProcess(float delta)
	{
		if(attackDuration > 0) return;

		base._PhysicsProcess(delta);
		var collisionInfo = MoveAndCollide(Vector2.Zero);
		if(collisionInfo != null)
		{
			var collisionTarget = (Node)collisionInfo.Collider;
			var enemyName = collisionTarget.Name;
			var enemy = GetNode<Enemy>("/root/World/" + enemyName);

			if(enemy.isAlive)
			{
				if(this.money > enemy.money)
				{
					this.SetAnimationState(GetLookIndex(enemy) + 6);
					money += enemy.money;
					label.Text = money.ToString() + "g";
					enemy.OnDead(this);
					attackDuration = 1f;
				}
				else if(this.money < enemy.money)
				{
					enemy.SetAnimationState(GetLookIndex(this) + 6);
					enemy.money += this.money;
					enemy.label.Text = enemy.money.ToString() + "g";
					this.OnDead(enemy);
					enemy.attackDuration = 1f;

					
				}
			}


		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
