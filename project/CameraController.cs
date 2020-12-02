using Godot;
using System;

public class CameraController : Camera2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private Node2D player;
	int count = 0;
	Vector2 shakeVector;
	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/World/Player");
	}
	public override void _Process(float delta)
	{
		if(count > 0)
		{
			count --;
			if(count < 10)
			{
				var rnd = new RandomNumberGenerator();
				rnd.Randomize();
				var x = rnd.Randf();
				rnd.Randomize();
				var y = rnd.Randf();
				shakeVector = new Vector2(x * 10f - 5f, y * 10f - 5f);
			}
			else
			{
				shakeVector = Vector2.Zero;
			}
			if(count == 1)
			{
				var playerCompnent = GetNode<Player>("/root/World/Player");
				if(playerCompnent.money <= 1)
				{
					var gameOverPanel = GetNode<Panel>("/root/World/CanvasLayer/Panel");
					gameOverPanel.RectScale = Vector2.One;

				}
			}
		}
		else
		{
			shakeVector = Vector2.Zero;
		}
		this.Position = new Vector2(Mathf.Lerp(this.Position.x, player.Position.x, .1f), Mathf.Lerp(this.Position.y, player.Position.y, .1f)) + shakeVector;
	}
	public void Shake()
	{
		count = 30;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
