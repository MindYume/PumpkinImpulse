using Godot;

public class Bullet : RigidBody2D
{
	private GeneralSingleton _generalSingleton;
	private Vector2 _previousPosition;
	private float _power = 5;
	private Line2D _trail;
	private Sprite _circleSprite;
	private Sprite _circleSprite2;
	private CollisionShape2D _collisionShape2D;
	private string _target = "enemy";

	public override void _Ready()
	{
		_generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
		_previousPosition = Position;
		_trail = GetNode<Line2D>("Line2D");
		_circleSprite = GetNode<Sprite>("Circle");
		_circleSprite2 = GetNode<Sprite>("Circle2");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _PhysicsProcess(float delta)
	{
		Rotation = 0;
		if (_previousPosition != Vector2.Zero)
		{
			_trail.SetPointPosition(1, (_previousPosition - Position) * 2 * Scale);
		}
		_previousPosition = Position;

		SetPower(_power - delta * 0.2f);
	}

	public void SetPower(float power)
	{
		if (power <= 0 || (_target == "player" && power <= 0.3f))
		{
			QueueFree();
		}
		else
		{
			_power = power;
			_trail.Scale = Vector2.One * power * 5;
			_circleSprite.Scale = Vector2.One * power * 0.25f;
			_circleSprite2.Scale = Vector2.One * power * 0.125f;
			_collisionShape2D.Scale = Vector2.One * power * 5;
			Mass = power;
		}
	}

	public void SetColor(Color bulletColor1, Color bulletColor2, Color trailColor)
	{
		_circleSprite.Modulate = bulletColor1;
		_circleSprite2.Modulate = bulletColor2;
		_trail.DefaultColor = trailColor;
	}

	public void SetTarget(string target)
	{
		_target = target;
		if(target == "enemy")
		{
			SetCollisionMaskBit(2, true);
			SetCollisionMaskBit(1, false);
		}
		else if (target == "player")
		{
			SetCollisionMaskBit(2, false);
			SetCollisionMaskBit(1, true);
		}
	}
	
	public void _on_Bullet_body_entered(Node body)
	{
		if (body is IEnemy)
		{
			((IEnemy)body).TakeDamage(_power * 100);
			_generalSingleton.PlaySound("hit", (-40 + _power * 40), 2);
			HitEffect.Create(
				(Node2D)GetParent().GetParent(),
				(GlobalPosition + ((Node2D)body).GlobalPosition)/2,
				Vector2.One * _power * 2,
				_trail.DefaultColor
			);
			QueueFree();
		}
		else if (body is Player)
		{
			((Player)body).TakeDamage();
			_generalSingleton.PlaySound("hit", (-40 + _power * 40), 2);
			HitEffect.Create(
				(Node2D)GetParent().GetParent(),
				(GlobalPosition + ((Node2D)body).GlobalPosition)/2,
				Vector2.One * _power * 2,
				_trail.DefaultColor
			);
			QueueFree();
		}
	}
}
