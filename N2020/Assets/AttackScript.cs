﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{

    public EnemyController enemyCont;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public  void AttackTrigger()
    {
        enemyCont.Attack();
    }
}
