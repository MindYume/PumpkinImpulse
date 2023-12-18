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

public class EnergyEffect : Node2D
{
    [Export] public bool playSound;
    private Sprite[] _circles;
    private float _energyValue = 0;

    public override void _Ready()
    {
        _circles = new Sprite[3];
        _circles[0] = GetNode<Sprite>("Circle1");
        _circles[1] = GetNode<Sprite>("Circle2");
        _circles[2] = GetNode<Sprite>("Circle3");
    }

    public override void _PhysicsProcess(float delta)
    {
        for (int i = 0; i < _circles.Length; i++)
        {
            _circles[i].Modulate = new Color(1,1,1, (1 - _circles[i].Scale.x) * _energyValue);
            _circles[i].Scale -= Vector2.One * delta * 4f;

            if (_circles[i].Scale.x <= 0)
            {
                _circles[i].Scale += Vector2.One;
                _circles[i].Modulate = new Color(0,0,0,0);
                
                if (playSound)
                {
                    SoundPlayer.PlaySound("wave", (-50 + _energyValue * 60), 1);
                }
            }
        }
    }

    public float EnergyValue
    {
        set {_energyValue = value;}
    }
}
