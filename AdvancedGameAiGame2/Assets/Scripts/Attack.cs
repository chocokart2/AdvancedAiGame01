using UnityEngine;

public class Attack : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character == null) return;
        character.BeAttacked(10);
    }
}
