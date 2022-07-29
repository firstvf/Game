using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMechanism : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectMainCamera;
    [SerializeField] private GameObject _gameObjectDoor;
    [SerializeField] private TextMeshProUGUI _textLockedDoor;
    [SerializeField] private TextMeshProUGUI _textOpenDoor;
    private int _enemyCount;
    private PlayerData _playerData;

    private void Start()
    {
        _playerData = FindObjectOfType<PlayerData>();
    }

    public void CheckEnemyCountInRoom()
    {
        var enemy = FindObjectsOfType<Enemy>();
        if (enemy.Length == 0) _enemyCount = 0;
        else _enemyCount++;
    }

    IEnumerator HideInfoPanelTimer(float time)
    {
        yield return new WaitForSeconds(time);
        _textLockedDoor.gameObject.SetActive(false);
        _textOpenDoor.gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckEnemyCountInRoom();
        InteractWithDoorCollision(collision);
        if (collision.tag == "Player" && _enemyCount == 0)
            _textOpenDoor.gameObject.SetActive(true);
        else _textOpenDoor.gameObject.SetActive(false);

        if (collision.tag == "Player" && _enemyCount != 0)
            _textLockedDoor.gameObject.SetActive(true);
        else _textLockedDoor.gameObject.SetActive(false);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(HideInfoPanelTimer(0.1f));
    }

    private void InteractWithDoorCollision(Collider2D collisionObject)
    {
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded && _textOpenDoor.IsActive() && Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
        if (_textOpenDoor.IsActive() && SceneManager.GetSceneByBuildIndex(1).isLoaded
            && _gameObjectDoor.name != "Door_3" && collisionObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            if (gameObject.name == "Door")
                GameObject.Find("Second_Room_Mob_Spawner").GetComponent<MobSpawner>().SetQuanityOfEnemies(1);
            else
                GameObject.Find("Third_Room_Mob_Spawner").GetComponent<MobSpawner>().SetQuanityOfEnemies(2);

            _gameObjectMainCamera.transform.position = new Vector3(0, _gameObjectMainCamera.transform.position.y + 15, -10);
            collisionObject.gameObject.SetActive(false);
            collisionObject.gameObject.transform.position = new Vector3(0, _gameObjectDoor.transform.position.y + 6, 0);
            collisionObject.gameObject.SetActive(true);
        }
        if (_textOpenDoor.IsActive() && SceneManager.GetSceneByBuildIndex(1).isLoaded && gameObject.name == "Door_3" && Input.GetKey(KeyCode.E))
        {
            _playerData.SaveHealthAndMoney();
            SceneManager.LoadScene(2);
        }
        if (_textOpenDoor.IsActive() && SceneManager.GetSceneByBuildIndex(2).isLoaded && Input.GetKey(KeyCode.E))
        {
            _playerData.SaveHealthAndMoney();
            SceneManager.LoadScene(3);
        }
        if (_textOpenDoor.IsActive() && SceneManager.GetSceneByBuildIndex(3).isLoaded && Input.GetKey(KeyCode.E))
        {
            _playerData.SaveHealthAndMoney();
            SceneManager.LoadScene(4);
        }
        if (_textOpenDoor.IsActive() && SceneManager.GetSceneByBuildIndex(4).isLoaded && Input.GetKey(KeyCode.E))
        {
            _playerData.SaveHealthAndMoney();
            SceneManager.LoadScene(5);
        }
    }
}
