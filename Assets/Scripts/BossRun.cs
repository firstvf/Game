using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    [SerializeField] private float _maxDistance = 2.5f;
    private Vector2 _startPosition = new Vector2(1, 18.5f);
    private Transform _bossTransform;
    private Transform _playerTransform;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private Player _player => FindObjectOfType<Player>();
    private bool _isChasingPlayer => !FindObjectOfType<Boss>().GetComponent<Animator>().GetBool("IsMageCast") && _player != null;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player ==null) animator.SetTrigger("SatanWin");
        _bossTransform = animator.GetComponent<Transform>();
        _audioSource = animator.GetComponent<AudioSource>();
        _playerTransform = _player.transform;
        _rigidbody2D = animator.GetComponent<Rigidbody2D>();
        _audioSource.Play();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((Vector2)animator.transform.position == _startPosition && !_isChasingPlayer)
        {
            animator.SetBool("IsSatanIdle", true);
            animator.SetBool("IsMageCast", true);
        }
        if (_isChasingPlayer)
        {
            Vector2 target = new Vector2(_playerTransform.position.x, _playerTransform.position.y+0.1f);
            Vector2 newPosition = Vector2.MoveTowards(_rigidbody2D.position, target, _maxDistance * Time.fixedDeltaTime);
            _rigidbody2D.MovePosition(newPosition);
            if (target.x > _rigidbody2D.position.x) _bossTransform.localScale = new Vector3(-1, 1, 1);
            else if (target.x < _rigidbody2D.position.x) _bossTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (!_isChasingPlayer)
        {
            Vector2 newPosition = Vector2.MoveTowards(_rigidbody2D.position, _startPosition, _maxDistance * 2 * Time.fixedDeltaTime);
            _rigidbody2D.MovePosition(newPosition);
            if (_startPosition.x > _rigidbody2D.position.x) _bossTransform.localScale = new Vector3(-1, 1, 1);
            else if (_startPosition.x < _rigidbody2D.position.x) _bossTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _audioSource.Stop();
    }
}
