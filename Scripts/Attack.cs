using Godot;
using System;

/*
The attack class is used to subtract health off a target based upon attacker's strength and target's defense.
Seems fairly redundant since it needs to cast the player and enemy from Node2D. But works.
 */

public class Attack
{
    public void AttackTarget(Node2D _attacker, Node2D _target, Console _console)
    {
        if (_attacker is Player)
        {
            Player player = _attacker as Player;
            Enemy enemy = _target as Enemy;
            DealDamage(player, enemy, _console);
            enemy.CheckIfAlive();
        }
        else if (_attacker is Enemy)
        {
            Player player = _target as Player;
            Enemy enemy = _attacker as Enemy;
            DealDamage(enemy, player, _console);
            // TODO add a method in player script that will check if player is alive
        }
    }

    private void DealDamage(Enemy _attacker, Player _target, Console _console)
    {
        int damage = DamageFormula(_attacker.Stats.Strength, _target.Stats.Strength);
        _target.Stats.CurrentHealth -= DamageFormula(_attacker.Stats.Strength, _target.Stats.Strength);
        _console.PrintMessageToConsole(_attacker.Name + " attacked " + _target.Name + " with a damage value of " + damage + "!");
    }

    private void DealDamage(Player _attacker, Enemy _target, Console _console)
    {
        int damage = DamageFormula(_attacker.Stats.Strength, _target.Stats.Strength);
        _target.Stats.CurrentHealth -= DamageFormula(_attacker.Stats.Strength, _target.Stats.Strength);
    _console.PrintMessageToConsole(_attacker.Name + " attacked " + _target.Name + " with a damage value of " + damage + "!");
    }
    
    private int DamageFormula(int _strength, int _defense)
    {
        return (_strength * 2) / _defense;
    }
}