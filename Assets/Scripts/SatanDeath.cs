using UnityEngine;

public class SatanDeath : StateMachineBehaviour
{
    [SerializeField] private AudioClip _satanLaugh;
    private AudioSource _audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _audioSource = animator.GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_satanLaugh);
    }
}
