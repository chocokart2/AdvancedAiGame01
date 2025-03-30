using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject agentA;
    [SerializeField] GameObject agentB;
    Character agentACharacter;
    Character agentBCharacter;
    AiCharacter agentAAiCharacter;
    AiCharacter agentBAiCharacter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentACharacter = agentA.GetComponent<Character>();
        agentBCharacter = agentA.GetComponent<Character>();
        agentAAiCharacter = agentB.GetComponent<AiCharacter>();
        agentBAiCharacter = agentB.GetComponent<AiCharacter>();

        agentAAiCharacter.Enemy = agentBCharacter;
        agentAAiCharacter.EnemyAi = agentBAiCharacter;
        agentBAiCharacter.Enemy = agentACharacter;
        agentBAiCharacter.EnemyAi = agentAAiCharacter;

        agentACharacter.enemy = agentB;
        agentBCharacter.enemy = agentA;
    }

    // Update is called once per frame
    void Update()
    {
        // 1초에 한번씩 정책 실행

    }
}
