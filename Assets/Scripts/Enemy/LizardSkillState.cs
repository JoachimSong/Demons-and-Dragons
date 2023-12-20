using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("fire", true);
        currentEnemy.anim.SetTrigger("skill");
        currentEnemy.lostTargetTimeCounter = currentEnemy.lostTargetTime;

    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTargetTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        Debug.Log("exit skill");
        currentEnemy.anim.SetBool("fire", false);

    }
}
