using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isSlowed = false;
    public bool isDefend = false;

    public float moveSpeed = 1.0f;
    public float lerpLength = 2.0f;
    public float maxAttackLength = 3.0f;

    public int health = 30;
    public GameObject attackPrefab;
    public GameObject slowAttackPrefab;
    public GameObject nukeAttackPrefab;
    public GameObject enemy;

    Coroutine slowCoroutine = null;

    public void BeAttacked(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Debug.Log($"{gameObject.name} 이 사망하였습니다!");
            Destroy(gameObject, 3f);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = enemy.transform.position;
        target.y = transform.position.y;

        transform.LookAt(target);
    }

    // 모든 행동을 1초 내로 해결할 것

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }

    public IEnumerator Attack()
    {
        Vector3 attackPosition = enemy.transform.position;
        yield return new WaitForSeconds(1);

        if (IsCouldReach() && (isSlowed == false))
        {
            Instantiate(attackPrefab, attackPosition, Quaternion.identity);
        }
    }

    public IEnumerator MoveBack()
    {
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            if (isSlowed == false)
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    public IEnumerator MoveForward()
    {
        for (float time = 0; time < 1; time += Time.deltaTime)
        {
            if (isSlowed == false)
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    public IEnumerator Leap()
    {
        yield return new WaitForSeconds(1);
        Vector3 right = Vector3.Cross(Vector3.up, transform.forward).normalized;
        Vector3 direction = (Random.Range(0.0f, 1.0f) > 0.5f) ? right : -right;

        transform.position += direction * lerpLength;
    }

    public IEnumerator Slow()
    {
        Vector3 attackPosition = enemy.transform.position;
        yield return new WaitForSeconds(1);

        if (IsCouldReach())
        {
            Instantiate(slowAttackPrefab, attackPosition, Quaternion.identity);
        }
    }
    
    public IEnumerator Nuke()
    {
        Vector3 attackPosition = enemy.transform.position;
        yield return new WaitForSeconds(1);

        if (IsCouldReach())
        {
            Instantiate(nukeAttackPrefab, attackPosition, Quaternion.identity);
        }
    }

    public IEnumerator Defend()
    {
        isDefend = true;
        yield return new WaitForSeconds(1);
        isDefend = false;
    }

    public void SlowEffect()
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }

        IEnumerator m_coroutine()
        {
            isSlowed = true;
            yield return new WaitForSeconds(1);
            isSlowed = false;
        }

        slowCoroutine = StartCoroutine(m_coroutine());
    }

    private bool IsCouldReach() => 
        (transform.position - enemy.transform.position).sqrMagnitude
        < maxAttackLength * maxAttackLength;
}
