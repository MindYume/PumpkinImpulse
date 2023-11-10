using Godot;
using System;

public class EnemyWitch : RigidBody2D, IEnemy
{
	private PackedScene _firePreload = GD.Load<PackedScene>("res://Objects/Fire.tscn");
	private PackedScene _bulletPreload = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
	private GeneralSingleton _generalSingleton;
	private Player _player;
	private AnimatedSprite _animatedSprite;
	private float _health = 200;
	private float _maxWaitTime = 1;
	private float _minDistanceToPoint = 10;
	private float _maxVelocity = 200;
	private float _fireSpawnDelay = 0.2f;
	private float _bulletReloadDelay = 1.5f;
	private float _bulletReloadTime = 0;
	private Vector2 _movePoint;
	private bool _isMoving = false;
	private float _waitTime = 0;
	private float _nextFireTime = 0;
	private Random rand = new Random();
	public float Health
	{
		set => _health = value;
		get => _health;
	}

	public override void _Ready()
	{
		_generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
		_animatedSprite = GetNode<AnimatedSprite>("WitchRed");
	}

	public override void _PhysicsProcess(float delta)
	{
		Rotation = 0;
		if (_player == null && _generalSingleton.PlayerNode != null)
		{
			_player = _generalSingleton.PlayerNode;
		}
		else
		{
			if (_isMoving)
			{
				Vector2 relativePointPosition = _movePoint - Position;
				float LinearVelocityAtan2 = Mathf.Atan2(relativePointPosition.y, relativePointPosition.x);
				LinearVelocity += (new Vector2(Mathf.Cos(LinearVelocityAtan2), Mathf.Sin(LinearVelocityAtan2))) * delta * 500;
				LinearVelocity = LinearVelocity.LimitLength(_maxVelocity);

				_nextFireTime += delta;
				if (_nextFireTime >= _fireSpawnDelay && LinearVelocity.Length() >= _maxVelocity - 5)
				{
					_nextFireTime = 0;

					// Spawn fire
					Fire fire = _firePreload.Instance<Fire>();
					_generalSingleton.BulletsContainer.AddChild(fire);
					fire.GlobalPosition = Position;
				}

				if (relativePointPosition.Length() <= _minDistanceToPoint)
				{
					_isMoving = false;
				}
			}
			else
			{
				_waitTime += delta;
				if (_waitTime >= _maxWaitTime)
				{
					_isMoving = true;
					_waitTime = 0;
					_movePoint = new Vector2(50 + ((float)rand.NextDouble()) * 500, 50 + ((float)rand.NextDouble()) * 500);

				}
			}

			_bulletReloadTime += delta;
			if ((_player.Position - Position).Length() < 200 && _bulletReloadTime >= _bulletReloadDelay && LinearVelocity.x * (_player.Position.x - Position.x) >= 0)
			{
				_bulletReloadTime = 0;

				// Spawn bullet
				Bullet bullet = _bulletPreload.Instance<Bullet>();
				_generalSingleton.BulletsContainer.AddChild(bullet);
				Vector2 playerDirection = (_player.Position - Position).Normalized();
				bullet.GlobalPosition = Position;
				bullet.LinearVelocity += new Vector2(playerDirection.x, playerDirection.y) * 250;
				bullet.SetPower(0.7f);
				bullet.SetTarget("player");
				bullet.SetColor(Color.Color8(219, 23, 1), Color.Color8(251, 238, 120), Color.Color8(255, 255, 200));

				_generalSingleton.PlaySound("fire", -15 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
				_generalSingleton.PlaySound("wave_end", -5 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
			}

			if (LinearVelocity.x > 0)
			{
				_animatedSprite.FlipH = true;
				_animatedSprite.Position = new Vector2(-4, 1);
			}
			else
			{
				_animatedSprite.FlipH = false;
				_animatedSprite.Position = new Vector2(4, 1);
			}

			if (Position.x < 0 || Position.x > 600 || Position.y < 0 || Position.y > 600)
			{
				GD.Print("Out of border");
				QueueFree();
			}
		}
	}

	public void TakeDamage(float damage_value)
	{
		_health -= damage_value;
		if (_health <= 0)
		{
			HealthPoint.Spawn(GetParent(), Position, 0.5);
			QueueFree();
		}
	}
}
