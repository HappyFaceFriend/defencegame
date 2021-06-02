using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class AnimUtils
{
    public static bool IsDone(Animator animator, string name)
    {
        var info = animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(name) &&  info.normalizedTime >= 1;
    }
}
