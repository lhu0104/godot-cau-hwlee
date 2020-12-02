using Godot;
using System;

public class Game : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Node2D player;
	Node2D enemy;
	PackedScene[] ps;
	Label timerUI;
	Label goalUI;
	NinePatchRect gauge;
	Node2D person;
	float timer = 60f;
	int goal = 500;
	int[] mCount = new int[] {
		10,
		10,
		10,
		10,
		10,
		10,
		10,
		10
	};
	public override void _Ready()
	{
		ps = new PackedScene[8];
		for(int i = 1; i < 8; i ++)
		{
			ps[i - 1] = (PackedScene)ResourceLoader.Load(string.Format("res://Prefab/Enemy{0}.tscn", i.ToString()));
		}


		timerUI = GetNode<Node>("/root/World/CanvasLayer/Node2D").GetChild<Label>(4);
		goalUI = GetNode<Node>("/root/World/CanvasLayer/Node2D").GetChild<Label>(5);
		person = GetNode<Node>("/root/World/CanvasLayer/Node2D").GetChild<Sprite>(2);
		gauge = GetNode<Node>("/root/World/CanvasLayer/Node2D").GetChild<NinePatchRect>(1);
	
		StartGame();        
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		if(timer > 0f)
			timer -= delta;
		else
		{
			var playerCompnent = GetNode<Player>("/root/World/Player");
			playerCompnent.money -= goal;
			playerCompnent.label.Text = playerCompnent.money.ToString() + "g";
			if(playerCompnent.money <= 0)
			{
				var gameOverPanel = GetNode<Panel>("/root/World/CanvasLayer/Panel");
				gameOverPanel.RectScale = Vector2.One;

			}
			timer = 60f;
			goal *= 3;
		}
		timerUI.Text = string.Format("00:{0}", ((int)timer).ToString("00"));
		goalUI.Text = (int)goal + "g";
		float prop = timer / 60f;
		gauge.RectScale = new Vector2(8.4f * prop,  0.21f);
		person.Position = new Vector2(90f + (-180f * prop), -247.93f);
	}
	
	void StartGame()
	{
		player = GetNode<Node2D>("/root/World/Player");
		enemy = GetNode<Node2D>("/root/World/Enemy");
		for(int m_index = 1; m_index < 8; m_index++)
		{

			for(int i = 0 ; i < mCount[m_index - 1] ; i ++)
			{
				Node2D duplicated = (Node2D)ps[m_index - 1].Instance();
				AddChild(duplicated);
				// duplicated.GetNode<Enemy>("/root/World/" + duplicated.Name).enemyIndex = m_index;
				
				var rnd = new RandomNumberGenerator();
				rnd.Randomize();
				var x = rnd.Randf();
				rnd.Randomize();
				var y = rnd.Randf();
				(duplicated as Node2D).Position = new Vector2(x * 2000f - 1000f, y * 2000f - 1000f);
			}

		}
	}
	void EndGame()
	{

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
