using Godot;
using System;

public class UI : Node2D
{
	private Node2D player;
	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/World/Player");
	}
	public override void _Process(float delta)
	{
		this.Position = player.Position;
	}
}
