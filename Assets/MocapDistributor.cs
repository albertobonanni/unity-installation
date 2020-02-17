using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.components;

public class MocapDistributor : MonoBehaviour
{
    public MocapPlayer leftPlayer;
    public MocapPlayer rightPlayer;

    bool isLeft;

    public void SetPlayerAnimation(AnimationClip animClip)
    {
        animClip.wrapMode = WrapMode.Loop;
        if(isLeft)
        {
            leftPlayer.PlayAnimationClip(animClip);
        }
        else
        {
            rightPlayer.PlayAnimationClip(animClip);
        }
        isLeft = !isLeft;
    }
   
}
