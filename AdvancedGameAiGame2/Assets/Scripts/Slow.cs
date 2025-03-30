using UnityEngine;

public class Slow : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character == null) return;
        character.SlowEffect();
    }
}
