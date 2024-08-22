using UnityEngine;

public class CameraFollowingThePlayer : MonoBehaviour
{
    [SerializeField] private float _verticalAddition;
    private float _cameraFollowSpeed = 3f;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (_player.Health > 0)
        {
            transform.position = Vector3.Lerp(gameObject.transform.position,
                new Vector3(_player.transform.position.x, _player.transform.position.y + _verticalAddition, -10), _cameraFollowSpeed * Time.deltaTime);
        }
    }
}
