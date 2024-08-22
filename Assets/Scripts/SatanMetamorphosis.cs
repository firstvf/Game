using UnityEngine;

public class SatanMetamorphosis : StateMachineBehaviour
{
    [SerializeField] private AudioClip _satanAwake;
    private AudioSource _audioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Attack")) animator.GetComponent<Boss>().AttackLogic();
        else
        {
            _audioSource = animator.GetComponent<AudioSource>();
            _audioSource.PlayOneShot(_satanAwake);
        }
    }
}
