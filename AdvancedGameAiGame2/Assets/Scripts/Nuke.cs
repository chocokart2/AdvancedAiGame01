using UnityEngine;

public class Nuke : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character == null) return;
        character.BeAttacked(20);
    }
}
