using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectEnemy;
    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private int _enemyQuantity;

    private void Start()
    {
        SpawnMobsInRoom();
    }

    public void SetQuanityOfEnemies(int quantity)
    {
        _enemyQuantity = quantity;
        SpawnMobsInRoom();
    }

    private void SpawnMobsInRoom()
    {
        int count = 0;
        for (int i = 0; i < _enemyQuantity; i++)
            foreach (var point in _spawnPoints)
            {
                count++;
                if (count > _enemyQuantity) return;
                Instantiate(_gameObjectEnemy, point.transform.position, Quaternion.identity);
            }

    }
}
