using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Tile currentTile;
    private Stage stage;
    private Animator animator;
    private bool isMoving;
    private Tile pendingTarget;
    private void Start()
    {
        var mapObject = GameObject.FindWithTag("Map");
        stage = mapObject.GetComponent<Stage>();

        currentTile = stage.Map.startTile;
        transform.position = stage.GetTilePos(currentTile.id);
        stage.OnTileVisited(currentTile.id);

        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    private void Update()
    {
        // if (isMoving || currentTile == null)
        // {
        //     return;
        // }

        // int direction = -1;
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     direction = (int)Sides.Top;
        // }
        // else if (Input.GetKeyDown(KeyCode.A))
        // {
        //     direction = (int)Sides.Left;
        // }
        // else if (Input.GetKeyDown(KeyCode.S))
        // {
        //     direction = (int)Sides.Bottom;
        // }
        // else if (Input.GetKeyDown(KeyCode.D))
        // {
        //     direction = (int)Sides.Right;
        // }
        if (Input.GetMouseButtonDown(0))
        {
            int targetId = stage.ScreenPosTileId(Input.mousePosition);
            Tile target = stage.Map.tiles[targetId];
            if (!target.CanMove)
            {
                return;
            }
            pendingTarget = target;
            if (!isMoving)
            {
                StartCoroutine(CoMovePlayer());
            }
        }

        // if (direction < 0)
        // {
        //     return;
        // }
        // var next = currentTile.adjacents[direction];
        // if (next == null || !next.CanMove)
        // {
        //     return;
        // }
        // StartCoroutine(CoMoveOneStep(next));
    }
    private IEnumerator CoMovePlayer()
    {
        isMoving = true;
        animator.speed = 1f;

        List<Tile> path = null;
        Tile pathTarget = null;
        int pathIndex = 0;

        while (pendingTarget != currentTile)
        {
            if (pathTarget != pendingTarget)
            {
                path = stage.Map.PathFinding(currentTile, pendingTarget);
                pathTarget = pendingTarget;
                pathIndex = 1; // 0이면 currentTile임
            }

            if (path == null || pathIndex >= path.Count)
            {
                break;
            }

            yield return CoMoveOneStep(path[pathIndex++]);
        }

        animator.speed = 0f;
        isMoving = false;
    }

    private IEnumerator CoMoveOneStep(Tile next)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = stage.GetTilePos(next.id);
        float moveDuration = Vector3.Distance(startPos, targetPos) / moveSpeed;
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        currentTile = next;

        stage.OnTileVisited(currentTile.id);
    }
}
