using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _fireball;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerLayers;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private GameObject _winText;
    private Player _player;
    private bool _isDie => _health <= 0;
    private int _health = 2000;

   

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _healthSlider.maxValue = _health;
        TransformationLogic();
    }

    private void Update()
    {
        if (_player == null && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Satan_Win"))
            _animator.SetTrigger("SatanWin");

        if (_isDie && !_animator.GetCurrentAnimatorStateInfo(0).IsName("SatanLose"))
        {
            _winText.SetActive(true);
            _animator.SetTrigger("SatanLose");
            StartCoroutine(TimeBeforeDestroy(3));
        }
    }

    private IEnumerator TimeBeforeDestroy(float time)
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void TransformationLogic()
    {
        _animator.SetBool("IsMageCast", true);
        StartCoroutine(CastUntilMetamorphosisIntoSatan(5));
        StartCoroutine(CooldownBeforeMetamorphosisIntoMage(25));
    }

    public void InvokeFireballWhileCast()
    {
        InvokeRepeating("CreateFireball", 1, 1);
    }

    public void DisableInvoke()
    {
        CancelInvoke();
    }

    private void CreateFireball()
    {
        Instantiate(_fireball, _spawnPoint.transform.position, Quaternion.identity);
    }

    private IEnumerator ReloadTransformationCycle(float time)
    {
        yield return new WaitForSeconds(time);
        TransformationLogic();
    }

    private IEnumerator CastUntilMetamorphosisIntoSatan(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetBool("IsMageCast", false);
        CancelInvoke();
    }

    private IEnumerator CooldownBeforeMetamorphosisIntoMage(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.SetBool("IsMageCast", true);
        StartCoroutine(ReloadTransformationCycle(20));
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    public void GotHitFromPlayer(int damage)
    {
            _health -= damage;
            _healthSlider.value = _health;
            if (!_audioSource.isPlaying && !_isDie)
                _audioSource.PlayOneShot(_hitSound);        
    }

    private IEnumerator TimeBeforeAttack(float time)
    {
        yield return new WaitForSeconds(time);

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<Player>().PlayerTakeDamage(16);
        }
    }

    public void AttackLogic()
    {
        StartCoroutine(TimeBeforeAttack(0.45f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            _animator.SetTrigger("SatanAttack");

        if (collision.collider.tag == "Obstacle")
        {
            _animator.SetTrigger("SatanJump");
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            _animator.SetTrigger("SatanAttack");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            _animator.SetTrigger("SatanJump");
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
