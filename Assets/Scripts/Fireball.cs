using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _explosionSound;
    [SerializeField] private AudioClip _castSound;
    [SerializeField] private float _fireballSpeed = 2.5f;
    private string[] _explosionAnimations = { "Explode", "Explode_2" };
    private Player _player => FindObjectOfType<Player>();
    private bool _isAbleToMove = true;

    private void Start()
    {
        _audioSource.PlayOneShot(_castSound);
    }

    private void Update()
    {
        if (_player == null) Destroy(gameObject);

        if (_isAbleToMove && _player != null)
        {
            Vector2 newPosition = Vector2.MoveTowards(gameObject.transform.position, _player.transform.position, _fireballSpeed * Time.fixedDeltaTime);
            _rigidbody2D.MovePosition(newPosition);
        }
    }
    private IEnumerator TimeBeforeDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "Wall" || collision.collider.tag == "Obstacle")
        {
            _isAbleToMove = false;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            _animator.SetTrigger(_explosionAnimations[Random.Range(0, 2)]);
            _audioSource.PlayOneShot(_explosionSound[Random.Range(0, 4)]);
            StartCoroutine(TimeBeforeDestroy(0.35f));
            if (collision.collider.tag == "Player") FindObjectOfType<Player>().PlayerTakeDamage(15);
        }
    }
}
