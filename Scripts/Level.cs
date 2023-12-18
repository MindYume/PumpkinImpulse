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
using System;

public class Level : Node2D
{
    private PackedScene _enemySimplePreload = GD.Load<PackedScene>("res://Objects/EnemySimple.tscn");
    private PackedScene _enemyOrbitPreload = GD.Load<PackedScene>("res://Objects/EnemyOrbit.tscn");
    private PackedScene _enemyGunPreload = GD.Load<PackedScene>("res://Objects/EnemyGun.tscn");
    private PackedScene _enemyWitchPreload = GD.Load<PackedScene>("res://Objects/EnemyWitch.tscn");
    private Player _player;
    private int _wave = 1;
    private float _waveScore = 1;
    private float _minWaveScore = 1;
    private bool _isBreakTime = false;
    private int _enemiesAmount = 0;
    private float _NextEnemySpawnDelay = 10;
    private float _NextEnemySpawnTime = 1;
    public int Wave
    {
        set
        {
            _wave = value;
            EmitSignal(nameof(wave_value_changed), value);
        }
        get {return _wave;}
    }

    [Signal] public delegate void wave_value_changed(int value);

    float time = 0;

    public override void _Ready()
    {
        GeneralSingleton.Instance.BulletsContainer = GetNode<Node2D>("BulletsContainer");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_player == null && GeneralSingleton.Instance.PlayerNode != null)
        {
            _player = GeneralSingleton.Instance.PlayerNode;
        }

        _NextEnemySpawnTime -= delta;

        if (_enemiesAmount == 0 && !_isBreakTime && _NextEnemySpawnTime > 1)
        {
            _NextEnemySpawnTime = 1;
        }

        if (_NextEnemySpawnTime <= 0  && _waveScore >= _minWaveScore)
        {
            _isBreakTime = false;
            while(true)
            {
                Random rand = new Random();
                int enemyNum = rand.Next(4);
                if(enemyNum == 0)
                {
                    SpawnEnemy("simple");
                    _waveScore -= _minWaveScore;
                    break;
                }
                else if(enemyNum == 1 && _waveScore >= 5)
                {
                    SpawnEnemy("witch");
                    _waveScore -= 5;
                    break;
                }
                else if(enemyNum == 2 && _waveScore >= 3)
                {
                    SpawnEnemy("orbit");
                    _waveScore -= 3;
                    break;
                }
                else if(enemyNum == 3 && _waveScore >= 4)
                {
                    SpawnEnemy("gun");
                    _waveScore -= 4;
                    break;
                }
            }
            _NextEnemySpawnTime = _NextEnemySpawnDelay;
        }

        if (_waveScore < _minWaveScore && _enemiesAmount <= 0)
        {
            if (Wave > GeneralSingleton.Instance.MaxWave)
            {
                GeneralSingleton.Instance.MaxWave = Wave;
            }
            
            Wave += 1;
            _NextEnemySpawnDelay = _NextEnemySpawnDelay * 0.9f;
            _waveScore = Wave * 1.2f;
            _isBreakTime = true;
            _NextEnemySpawnTime = 3f;

            SoundPlayer.PlaySound("wave_complete", -20, 1);
        }
    }

    private void SpawnEnemy(string enemyName)
    {
        Node2D enemy;
        switch(enemyName)
        {
            case "simple":
                enemy = _enemySimplePreload.Instance<Node2D>();
                break;
            case "witch":
                enemy = _enemyWitchPreload.Instance<Node2D>();
                break;
            case "orbit":
                enemy = _enemyOrbitPreload.Instance<Node2D>();
                break;
            case "gun":
                enemy = _enemyGunPreload.Instance<Node2D>();
                break;
            default:
                throw new Exception("Wrong enemy name");
        }

        Node2D enemies = GetNode<Node2D>("Enemies");
        enemies.AddChild(enemy);
        enemy.Connect("tree_exiting", this, nameof(EnemyDefeat));
        _enemiesAmount += 1;

        Random rand = new Random();
        for(int i = 1000; i >= 0; i--)
        {
            float x = 25 + ((float)rand.NextDouble()) * 550;
            float y = 25 + ((float)rand.NextDouble()) * 550;
            Vector2 spawnPosition = new Vector2(x,y);
            if (spawnPosition.DistanceTo(_player.Position) >= 300)
            {
                enemy.Position = spawnPosition;
                break;
            }

            if(i == 0)
            {
                // throw new Exception("The enemy could not find a position to spawn");
                GD.Print("The enemy could not find a position to spawn");
                enemy.Position = new Vector2(x, y);
            }
        }
    }

    private void EnemyDefeat()
    {
        _enemiesAmount -= 1;
    }
}
