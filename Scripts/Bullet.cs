/*
MIT License

Copyright (c) 2023 Viktor Grachev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Godot;

public class Bullet : RigidBody2D
{
	private float _power = 5;
	private Line2D _trail;
	private Sprite _circleSprite;
	private Sprite _circleSprite2;
	private CollisionShape2D _collisionShape2D;
	private string _target = "enemy";

	public override void _Ready()
	{
		_trail = GetNode<Line2D>("Line2D");
		_circleSprite = GetNode<Sprite>("Circle");
		_circleSprite2 = GetNode<Sprite>("Circle2");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _PhysicsProcess(float delta)
	{
		Rotation = 0;
		_trail.SetPointPosition(1, -LinearVelocity / 30 * Scale);

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
			SoundPlayer.PlaySound("hit", (-40 + _power * 40), 2);
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
			SoundPlayer.PlaySound("hit", (-40 + _power * 40), 2);
			HitEffect.Create(
				(Node2D)GetParent().GetParent(),
				(GlobalPosition + ((Node2D)body).GlobalPosition)/2,
				Vector2.One * _power * 2,
				_trail.DefaultColor
			);
			QueueFree();
		}
		else if (body is StaticBody2D)
		{
			SoundPlayer.PlaySound("hit", (-50 + _power * 30), 1/_power);
		}
	}
}
