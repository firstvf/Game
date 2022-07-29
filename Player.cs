using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayers;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip[] _attackSound;
    [SerializeField] private AudioClip _attackMissSound;
    private PlayerUI _playerUI;
    private Vector2 _playerMovementVector2;
    private float _multiplyMoveSpeed = 5f;
    private float _attackRange = 0.95f;
    private int _damage = 20;
    private bool _isBossScene => SceneManager.GetSceneByBuildIndex(5).isLoaded;
    private bool _isAttackCooldown = true;

    [HideInInspector] public bool IsPlayerDead => Health <= 0;
    [HideInInspector] public int Health = 100;
    [HideInInspector] public int Money=0;

    private void Start()
    {
        _playerUI = FindObjectOfType<PlayerUI>();
    }

    private void Update()
    {
        PlayerAttackLogic();
    }

    private void FixedUpdate()
    {
        PlayerPhysicalMovementLogic();
    }

    public void AddMoneyFromLoot(int money)
    {
        Money += money;
        _playerUI.SetMoneyUI(Money);
    }

    public void ChangeDamage(int damage)
    {
        _damage = damage;
    }

    public void GiveMoneyToTheMerchant(int money)
    {
        Money -= money;
        _playerUI.SetMoneyUI(Money);
    }

    public void AddHealth()
    {
        Health = 100;
        _playerUI.MoveSliderIfPlayerGotDamage(Health);
    }
    public void LoseMoney()
    {
        Money = 0;
        _playerUI.SetMoneyUI(Money);
    }

    private IEnumerator CooldownBetweenAttacks(float time)
    {
        yield return new WaitForSeconds(time);
        _isAttackCooldown = true;
    }

    public void PlayerTakeDamage(int damage)
    {
        Health -= damage;
        _playerUI.MoveSliderIfPlayerGotDamage(Health);
        if (!IsPlayerDead)
        {
            _audioSource.PlayOneShot(_hitSound, 0.45f);
            if (!_audioSource.isPlaying)
                _playerAnimator.Play("Hit");
        }
        else PlayerDeath();
    }

    private IEnumerator TimeBeforeDestroyPlayerObject(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void PlayerDeath()
    {
        Money = 0;
        _audioSource.PlayOneShot(_deathSound, 0.4f);
        _playerUI.MoveSliderIfPlayerGotDamage(Health);
        _playerAnimator.Play("Die");
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine(TimeBeforeDestroyPlayerObject(1.5f));
    }

    private void PlayerAttackLogic()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);
        if (Input.GetKey(KeyCode.Space) && _isAttackCooldown == true && !IsPlayerDead)
        {
            foreach (var enemy in hitEnemies)
            {
                _audioSource.PlayOneShot(_attackSound[Random.Range(0, 3)], 0.7f);
                if (!_isBossScene)
                    enemy.GetComponent<Enemy>().GotHitFromPlayer(_damage);
                else
                    enemy.GetComponent<Boss>().GotHitFromPlayer(_damage);
            }
            _audioSource.PlayOneShot(_attackMissSound);
            _playerAnimator.Play("Attack");
            _isAttackCooldown = false;
            StartCoroutine(CooldownBetweenAttacks(0.75f));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    private void PlayerPhysicalMovementLogic()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        _playerMovementVector2 = new Vector2(horizontalMovement, verticalMovement).normalized;

        if (horizontalMovement != 0 || verticalMovement != 0)
            _playerAnimator.SetBool("IsRun", true);
        else _playerAnimator.SetBool("IsRun", false);

        if (horizontalMovement < 0 && !IsPlayerDead)
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontalMovement > 0 && !IsPlayerDead)
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        if (!IsPlayerDead)
            _rigidbody2D.MovePosition(_rigidbody2D.position + _playerMovementVector2 * _multiplyMoveSpeed * Time.fixedDeltaTime);
    }
}
