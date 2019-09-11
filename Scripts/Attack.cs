using Godot;
using System;

/*
The attack class is used to subtract health off a target based upon attacker's strength and target's defense
 */

public class Attack
{
    public void AttackTarget(Node2D _attacker, Node2D _target)
    {
        if (_attacker is Player)
        {

        }
        else if (_attacker is Enemy)
        {

        }
    }

    private void DealDamageToPlayer(Enemy _attacker, Player _target)
    {

    }

    private void DealDamageToEnemy(Player _attacker, Enemy _target)
    {

    }
}