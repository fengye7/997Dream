using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRunningshoes : MonoBehaviour
{
    private Vector2 initialPosition;
    private Vector2 targetPosition;

    void Start()
    {
        GenerateRandomTargetPosition();
    }

    public void SetinitialPosition(Vector2 position)
    {
        initialPosition = position;
    }
    void Update()
    {
        CheckIfReachedTarget();
    }
    private void GenerateRandomTargetPosition()
    {
        targetPosition = new Vector2(Random.Range(-10f, 10f), 0f);
    }


    private void CheckIfReachedTarget()
    {
        if (Vector2.Distance(transform.position, targetPosition) < 1.0f)
        {
            ReturnToInitialPosition();
        }
    }

    private void ReturnToInitialPosition()
    {
        if (Vector2.Distance(transform.position, initialPosition) < 1.0f)
        {
            //success
        }
    }
}
