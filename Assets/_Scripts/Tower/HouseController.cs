using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : BaseTowerController
{
    public virtual IEnumerator Shoot(Transform target)
    {
        yield return null;
    }
}
