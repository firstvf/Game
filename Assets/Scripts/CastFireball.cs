using UnityEngine;

public class CastFireball : StateMachineBehaviour
{


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Boss>().InvokeFireballWhileCast();
        animator.GetComponent<Collider2D>().isTrigger = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Boss>().DisableInvoke();
        animator.GetComponent<Collider2D>().isTrigger = false;
    }
}
