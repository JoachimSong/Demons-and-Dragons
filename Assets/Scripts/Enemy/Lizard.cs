using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new LizardPatrolState();
        //skillState = new LizardSkillState();
    }
}
