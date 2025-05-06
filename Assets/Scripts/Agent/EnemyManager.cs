using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int enemies;

    public void OnDeath()
    {
        enemies--;
    }
}
