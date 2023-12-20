using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabChaseState : BaseState
{
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTargetTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);

        if (!currentEnemy.physicsCheck.isOnGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
    }

    public override void PhysicsUpdate()
    {

    }

}
