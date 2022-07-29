using UnityEngine;

public class Loot : MonoBehaviour
{
    private int _money=5;

    private void Awake()
    {
        _money = Random.Range(5, 10);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().AddMoneyFromLoot(_money);
            Destroy(gameObject);
        }
    }
}
