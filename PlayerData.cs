using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    private PlayerUI _playerUI;
    private Player _player;
    private static int _health=100;
    private static int _money=0;
    private static int _damage=20;
    private static float _light;

    private void Start()
    {
        _playerUI = FindObjectOfType<PlayerUI>();
        _player = FindObjectOfType<Player>();
            GetDataToNewScene();
    }

    public static void RestartGame()
    {
        _health = 100;
        _money = 0;
    }

    public void GetDataToNewScene()
    {
        _playerUI.SetMoneyUI(_money);
        _playerUI.MoveSliderIfPlayerGotDamage(_health);
        _player.Health = _health;
        _player.Money = _money;
        if (SceneManager.GetSceneByBuildIndex(3).isLoaded)
            GameObject.Find("Light_Potion_Effect").GetComponent<Light2D>().intensity = _light;
        if (SceneManager.GetSceneByBuildIndex(5).isLoaded)
            _player.ChangeDamage(_damage);
    }

    public void SaveDataRestartLevel()
    {
        _health = 100;
    }

    public void GetDamage(int damage)
    {
        _damage = damage;
    }

    public void SaveHealthAndMoney()
    {
        _health = _player.Health;
        _money = _player.Money;
        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
            _light = GameObject.Find("Light_Potion_Effect").GetComponent<Light2D>().intensity;
        if (SceneManager.GetSceneByBuildIndex(4).isLoaded)
            _player.ChangeDamage(_damage);
    }
}
