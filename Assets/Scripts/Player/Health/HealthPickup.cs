using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int AddHealth;
    private float Timer = 5f;

    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Health.Instance.Hearts < Health.Instance.NumberOfHearts || 
            collision.CompareTag("Player") && Health.Instance.HP < Health.Instance.MaxHP)
        {
            Health.Instance.HP += AddHealth;
            Movement.Instance.HealthSystem.Play();
            SoundManager.Play("PowerH");
            Destroy(gameObject);
        }
    }
}
