using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressurePlateMechanism : MonoBehaviour
{
    [SerializeField] private GameObject _openDoor;
    [SerializeField] private GameObject _closedDoor;
    [SerializeField] private List<GameObject> _collisionTargets;
    [SerializeField] private AudioClip _openDoorSound;
    [SerializeField] private AudioClip _closeDoorSound;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _spawner;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetSceneByBuildIndex(5).isLoaded)        
            StartCoroutine(TimeBeforeBossSpawn());        
        
        if (gameObject.tag == "Spawner" && collision.tag == "Player")
        {
            _spawner.SetActive(true);
        }
        if (gameObject.tag != "Spawner" && collision.tag == "Player" && !SceneManager.GetSceneByBuildIndex(5).isLoaded
            || gameObject.tag != "Spawner" && collision.tag == "Box" && !SceneManager.GetSceneByBuildIndex(5).isLoaded)
        {
            if (!_collisionTargets.Contains(collision.gameObject))
                _collisionTargets.Add(collision.gameObject);
            if (_collisionTargets.Count == 1)
            {
                _audioSource.PlayOneShot(_openDoorSound);
            }
            _openDoor.SetActive(true);
            _closedDoor.SetActive(false);
        }
        if (SceneManager.GetSceneByBuildIndex(5).isLoaded)
        {
            _openDoor.SetActive(false);
            _closedDoor.SetActive(true);
            _audioSource.PlayOneShot(_closeDoorSound);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private IEnumerator TimeBeforeBossSpawn()
    {
        yield return new WaitForSeconds(3);
        _spawner.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag != "Spawner" && collision.tag == "Player" || gameObject.tag != "Spawner" && collision.tag == "Box")
        {
            _collisionTargets.Remove(collision.gameObject);
            if (_collisionTargets.Count == 0)
            {
                _audioSource.PlayOneShot(_closeDoorSound);
                _openDoor.SetActive(false);
                _closedDoor.SetActive(true);
            }
        }
    }
}
