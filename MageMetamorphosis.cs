using UnityEngine;

public class MageMetamorphosis : StateMachineBehaviour
{
    [SerializeField] private AudioClip _mageAwake;
    private AudioSource _audioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsSatanIdle", false);
        _audioSource = animator.GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_mageAwake);
    }
}
