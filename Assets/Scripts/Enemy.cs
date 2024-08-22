using System.Collections;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _gameObjectLoot;
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _playerLayers;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip[] _deathSound;
    private bool _isEnemyAbleToMove => !_aiPath.isStopped;
    private bool _isDead => _maxHealth <= 0;
    private bool _isEnemyAbleToAttack = true;
    private float _attackRange = 1.13f;
    private int _maxHealth = 100;
    private int _damage = 5;

    private void Start()
    {
        gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().gameObject.transform;
    }

    private void Update()
    {
        PermissionToMove();
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    private IEnumerator CooldownBeforeAttack(float time)
    {
        yield return new WaitForSeconds(time);
        if (!_isDead)
        {
            _animator.Play("Attack");
             _aiPath.isStopped = false;
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayers);
            foreach (var player in hitPlayer)
            {
                player.GetComponent<Player>().PlayerTakeDamage(_damage);
            }
                _audioSource.PlayOneShot(_attackSound);
            _isEnemyAbleToAttack = true;
        }
    }

    private void AttackLogic()
    {
        if (_isEnemyAbleToAttack && !_isDead)
        {
            _aiPath.isStopped = true;
            _isEnemyAbleToAttack = false;
            StartCoroutine(CooldownBeforeAttack(0.5f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !_isDead)
        {
            AttackLogic();
        }
    }

    private void PermissionToMove()
    {
        if (_isEnemyAbleToMove&&!_isDead)
        {
            EnemyMovementLogic();
            _aiPath.isStopped = false;
        }
        else if (!_isEnemyAbleToMove&&_isDead)
        {
            _aiPath.isStopped = true;
            _animator.SetBool("IsWalk", false);
        }
    }

    private void EnemyMovementLogic()
    {
        if (!_isDead&&_isEnemyAbleToMove)
        {
            if (_aiPath.desiredVelocity.x <= 0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (_aiPath.desiredVelocity.x >= 0.01f)
                transform.localScale = new Vector3(1, 1, 1);

            if (transform.position.x != 0 * Time.deltaTime || transform.position.y != 0 * Time.deltaTime)
                _animator.SetBool("IsWalk", true);
            else _animator.SetBool("IsWalk", false);
        }
    }

    public void GotHitFromPlayer(int damage)
    {
        _maxHealth -= damage;
        if (!_isDead)
        {
            _animator.Play("Hit");
            AttackLogic();
        }
        else  EnemyDead();
    }

    private void EnemyDead()
    {
        if (_isDead)
        {
            _audioSource.PlayOneShot(_deathSound[Random.Range(0,2)]);
            Destroy(_aiPath);
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            _animator.SetTrigger("Die");
            Destroy(gameObject, 1.2f);
            Instantiate(_gameObjectLoot, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        }
    }
}
