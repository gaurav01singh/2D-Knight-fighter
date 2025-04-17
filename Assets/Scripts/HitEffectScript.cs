using UnityEngine;

public class HitEffectScript : MonoBehaviour
{
    private float hitTime = 0f;

    public float showTime = 0.1f;

    private void Update()
    {
        hitTime += Time.deltaTime;
        if (showTime<hitTime)
        {
            hitTime = 0;
            this.gameObject.SetActive(false);
        }
    }
}
