using Unity.VisualScripting;
using UnityEngine;

//EAction.cs파일의 내용
//public enum EAction
//{
//    wait,
//    attack,
//    moveFront,
//    moveBack,
//    leap, // 도약
//    slow,
//    nuke,
//    defend,
//    MAX
//}

public class AiCharacter : MonoBehaviour
{
    public EAction prevMyAction = EAction.wait;
    public EAction prevEnemyAction = EAction.wait;
    public EAction tempMyAction = EAction.wait;

    public Character Enemy;
    public AiCharacter EnemyAi;

    double gamma = 0.9;
    double theta = 0.01;

    double HIT_REWARD = 1.0;
    double CRITICAL_HIT_REWARD = 2.0;
    double HURT_PENALTY = -0.8;
    double CRITICAL_HURT_PENALTY = -1.6;

    // 내 행동 / 에너미 행동 / 거리
    double[,,] value = new double[(int)EAction.MAX, (int)EAction.MAX, 2];
    EAction[,,] policyActions = new EAction[(int)EAction.MAX, (int)EAction.MAX, 2];

    Character myCharacter;

    public void NextStepStart()
    {
        // tempMyAction에 현재 행동 액션 집어넣음
    }

    public void FinishStepNext()
    {
        prevMyAction = tempMyAction;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCharacter = GetComponent<Character>();
        BakePolicy();
    }

    // Update is called once per frame
    void Update()
    {
        // 1초에 한번씩 정책 실행
    }



    void BakePolicy()
    {
        // 정책 무작위화
        for (int indexI = 0; indexI < (int)EAction.MAX; ++indexI)
        {
            for (int indexJ = 0; indexJ < (int)EAction.MAX; ++indexJ)
            {
                for (int indexDistance = 0; indexDistance < 2; ++indexDistance)
                {
                    policyActions[indexI, indexJ, indexDistance]
                        = (EAction)Random.Range(0, (int)EAction.MAX);
                }
            }
        }

        // 정책 평가
        bool policyStable = false;
        while (policyStable == false)
        {
            while (true)
            {
                double delta = 0.0;
                for (int indexMyAction = 0; indexMyAction < (int)EAction.MAX; ++indexMyAction)
                {
                    for (int indexEnemyAction = 0; indexEnemyAction < (int)EAction.MAX; ++indexEnemyAction)
                    {
                        for (int distance = 0; distance < 2; distance++)
                        {
                            double reward = 0.0f;

                            // 자신의 입장을 바꿔서 상대방의 행동도 예측함.

                            int nextEnemyAction =
                                (int)policyActions[indexEnemyAction, indexMyAction, distance];
                            int nextPlayerAction =
                                (int)policyActions[indexMyAction, indexEnemyAction, distance];
                            int nextDistance = distance;

                            bool isEnemyHit =
                                distance == 0 &&
                                nextEnemyAction != (int)EAction.defend &&
                                nextEnemyAction != (int)EAction.leap &&
                                nextEnemyAction != (int)EAction.moveBack;
                            bool isPlayerHit =
                                distance == 0 &&
                                nextPlayerAction != (int)EAction.defend &&
                                nextPlayerAction != (int)EAction.leap &&
                                nextPlayerAction != (int)EAction.moveBack;

                            // 유효타를 먹인 경우
                            if (nextPlayerAction == (int)EAction.attack)
                            {
                                if (isEnemyHit)
                                {
                                    reward += HIT_REWARD;
                                }
                            }
                            if (nextPlayerAction == (int)EAction.nuke)
                            {
                                if (isEnemyHit)
                                {
                                    reward += CRITICAL_HIT_REWARD;
                                }
                            }
                            if (nextEnemyAction == (int)EAction.attack)
                            {
                                if (isPlayerHit)
                                {
                                    reward += HURT_PENALTY;
                                }
                            }
                            if (nextEnemyAction == (int)EAction.nuke)
                            {
                                if (isPlayerHit)
                                {
                                    reward += CRITICAL_HURT_PENALTY;
                                }
                            }

                            double prevValue = value[indexMyAction, indexEnemyAction, distance];
                            value[indexMyAction, indexEnemyAction, distance]
                                += reward + gamma * 
                                value[nextPlayerAction, nextEnemyAction, nextDistance];
                            double different = 
                                System.Math.Abs(prevValue - value[indexMyAction, indexEnemyAction, distance]);
                            if (different > delta) delta = different;
                        }
                    }
                }
            }

        }

        // 정책 개선
    }
}
