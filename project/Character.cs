using Godot;
public class Character : KinematicBody2D
{
    public AnimationPlayer animation;
    public Sprite sprite;
    public Label label;
    public virtual int Speed { get { return 100; } }
	public Vector2 velocity = new Vector2();
	public Vector2 lastVelocity = new Vector2();
    public int money;
    public bool isAlive { get; set; }
    public float attackDuration = 0f;
    public override void _Ready()
	{
        isAlive = true;
		sprite = GetNode<Sprite>(@"Sprite");
		label = GetNode<Label>(@"Label");
		animation = GetNode<AnimationPlayer>(@"AnimationPlayer");
        var rand = new RandomNumberGenerator();
        rand.Randomize();
        money = (int)(rand.Randf() * 100f);
        label.Text = money.ToString() + "g";
	}
    public  void SetAnimationState(int index)
	{
		switch(index)
		{
			case 0 : animation.Play("IdleLeft", 0); break;
			case 1 : animation.Play("MoveLeft", 0); sprite.FlipH = false; break;
			case 2 : animation.Play("MoveLeft", 0); sprite.FlipH = true; break;
			case 3 : animation.Play("MoveUp", 0); sprite.FlipH = false; break;
			case 4 : animation.Play("MoveDown", 0); sprite.FlipH = false; break;
			case 5 : animation.Play("IdleUp", 0); break;
			case 6 : animation.Play("IdleDown", 0); break;
			case 7 : animation.Play("AttackLeft", 0); sprite.FlipH = false; break;
			case 8 : animation.Play("AttackLeft", 0); sprite.FlipH = true; break;
			case 9 : animation.Play("AttackUp", 0); sprite.FlipH = false; break;
			case 10 : animation.Play("AttackDown", 0); sprite.FlipH = false; break;
			case 11 : animation.Play("DeadLeft", 0); sprite.FlipH = false; break;
			case 12 : animation.Play("DeadLeft", 0); sprite.FlipH = true; break;
			case 13 : animation.Play("DeadUp", 0); sprite.FlipH = false; break;
			case 14 : animation.Play("DeadDown", 0); sprite.FlipH = false; break;
		}
	}
    public void OnDead(Node2D from)
    {
        isAlive = false;
        SetAnimationState(GetLookIndex(from) + 10);
        attackDuration = .5f;

        var cam = GetNode<CameraController>("/root/World/Camera2D");
        cam.Shake();

    }
    public virtual void GetInput()
	{

	}
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(attackDuration > 0f)
            attackDuration -= delta;
        
    }
    
	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		GetInput();
        velocity = MoveAndSlide(velocity);
	}
    public int GetLookIndex(Node2D node)
    {
        float xDiff = this.Position.x - node.Position.x;
        float yDiff = this.Position.y - node.Position.y;
        if(Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            return xDiff > 0 ? 2 : 1;
        }
        else
        {
            return yDiff > 0 ? 3 : 4;
        }
    }
}