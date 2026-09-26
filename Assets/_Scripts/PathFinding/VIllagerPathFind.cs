using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class VillagerPathFind : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public VillagerAI villager;

    private List<Vector2Int> currentPath = new List<Vector2Int>();
    public bool isMoving = false;

    public Vector2Int currentGridPosition; 

    private Coroutine moveRoutine;

    void Start()
    {
        moveSpeed = villager.villagerSpeed * villager.villagerHealth.functionSpeed;

        currentGridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        
        transform.position = new Vector3(currentGridPosition.x, currentGridPosition.y, 0f);
    }

    public void OrderMoveTo(Vector2Int targetCoordinates)
    {
        if(GridManager.instance.walkableGrid.Length <= 0) 
        {
            Debug.Log("No roads yet");
            return;
        }

        if (GridManager.instance == null || !GridManager.instance.isReady)
        {
            villager.FailedPathfinding();
            return;
        }

        GridManager.instance.RequestPath(currentGridPosition, targetCoordinates, OnPathCalculated);
    }

    private void OnPathCalculated(List<Vector2Int> newPath)
    {
        if (newPath != null && newPath.Count > 0)
        {
            currentPath = newPath;

            if (!isMoving)
            {
                moveRoutine = StartCoroutine(FollowPathRoutine());
            }
        }
        else
        {
            villager.FailedPathfinding();
        }
    }

    private IEnumerator FollowPathRoutine()
    {
        isMoving = true;
        moveSpeed = villager.villagerSpeed * villager.villagerHealth.functionSpeed;

        int currentPathIndex = 0;
        if (currentPath.Count > 1 && currentPath[0] == currentGridPosition)
        {
            currentPathIndex = 1;
        }

        while (currentPathIndex < currentPath.Count)
        {
            Vector2Int nextTile = currentPath[currentPathIndex];
            Vector3 targetWorldPos = new Vector3(nextTile.x, nextTile.y, 0f);
            
            while (transform.position != targetWorldPos)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    targetWorldPos, 
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }
            
            transform.position = targetWorldPos;
            currentGridPosition = nextTile;
            currentPathIndex++;
        }

        currentPath.Clear();
        isMoving = false;
        villager.FinishedPathfinding();
    }

    public void CancelMovement()
    {
        currentPath.Clear();

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        isMoving = false;

        if (villager.cacheTarget == null)
        {
            TownManager.instance.ReleaseWanderSlot();
        }
    }
}