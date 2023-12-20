using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
}
