using Godot;
using System;

/*
The attack class is used to subtract health off a target based upon attacker's strength and target's defense.
Seems fairly redundant since it needs to cast the player and enemy from Node2D. But works.
 */

public class Attack
{
    public void AttackTarget(Node2D _attacker, Node2D _target, Console _console, Random _random)
    {
        if (_attacker is Player)
        {
            Player player = _attacker as Player;
            Enemy enemy = _target as Enemy;
            DealDamage(player, enemy, _console, _random);
            enemy.CheckIfAlive();
        }
        else if (_attacker is Enemy)
        {
            Player player = _target as Player;
            Enemy enemy = _attacker as Enemy;
            DealDamage(enemy, player, _console, _random);
            // TODO add a method in player script that will check if player is alive
        }
    }

    private void DealDamage(Enemy _attacker, Player _target, Console _console, Random _random)
    {
        int damage = DamageFormula(_attacker.Stats.Strength, _target.Stats.Strength, _random);
        _target.Stats.CurrentHealth -= damage;
        _console.PrintMessageToConsole(_attacker.Name + " attacked " + _target.Name + " with a damage value of " + damage + "!");
        _target.Stats.CallForUpdateOfPlayerCurrentHP();  // Updates Players Health in GUI
    }

    private void DealDamage(Player _attacker, Enemy _target, Console _console, Random _random)
    {
        int damage = DamageFormula(_attacker.Stats.Strength, _target.Stats.Strength, _random);
        _target.Stats.CurrentHealth -= damage;
        _console.PrintMessageToConsole(_attacker.Name + " attacked " + _target.Name + " with a damage value of " + damage + "!");
    }
    
    private int DamageFormula(int _strength, int _defense, Random _random)
    {
        int damageRandomizer = _random.Next(0, (int)(_strength * .25) + 1);
        return (_strength * 2) / _defense + damageRandomizer;
    }
}