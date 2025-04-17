using UnityEngine;

public class PetrollingScript : MonoBehaviour
{
    [Header("Patrolling Points")]
    [SerializeField] private Transform LeftPoint;
    [SerializeField] private Transform RightPoint;

    [Header("Enemies")]
    [SerializeField] private Rigidbody2D enemy1;
    [SerializeField] private Rigidbody2D enemy2;

    [Header("Speed")]
    [SerializeField] private float speed = 2f;

    private int direction1 = 1;
    private int direction2 = 1;

    private void FixedUpdate()
    {
        MoveEnemy(enemy1, ref direction1);
        MoveEnemy(enemy2, ref direction2);
    }

    private void MoveEnemy(Rigidbody2D enemy, ref int direction)
    {
        enemy.linearVelocity = new Vector2(direction * speed, enemy.linearVelocity.y);

        if (enemy.position.x >= RightPoint.position.x)
        {
            direction = -1; // Move left
            FlipEnemy(enemy, -1);
        }
        else if (enemy.position.x <= LeftPoint.position.x)
        {
            direction = 1; // Move right
            FlipEnemy(enemy, 1);
        }
    }

    private void FlipEnemy(Rigidbody2D enemy, int newDirection)
    {
        Vector3 scale = enemy.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * newDirection;
        enemy.transform.localScale = scale;
    }
}
