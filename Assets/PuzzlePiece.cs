using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private Transform parentObj;
    private Vector2 offSet;
    public GameInfomation gameInfo;
    private bool isSnaped;
    private bool isMove;
    private Vector2 targetPos;
    private SpriteRenderer spriteRender;
    [Header("Spawm Position")]
    public Transform topLeftLimit;
    public Transform bottomRightLimit;

    private void Awake()
    {
        if(transform.parent != null)
        {
            parentObj = transform.parent;
        }
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(parentObj != null)
        {
            transform.parent = null;
        }
        isSnaped = false;
        isMove = false;
        SceneController.Instance.AddPuzzlePieceForList(this);
    }

    private void Update()
    {
        if (isMove && targetPos != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, gameInfo.moveSpeed);
            if (transform.position.x == targetPos.x && transform.position.y == targetPos.y) isMove = false;
        }
    }

    private void OnMouseDown()
    {
        if (!isSnaped)
        {
            offSet = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (isSnaped == false && spriteRender != null)
        {
            MovePuzzlePieceUp();
        }

    }

    private void OnMouseDrag()
    {
        if(isSnaped == false)
        {
            Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 newPos = new Vector2(offSet.x + tempPos.x, offSet.y + tempPos.y);
            if(newPos.x <=topLeftLimit.transform.position.x || newPos.x >= bottomRightLimit.transform.position.x ||
                newPos.y >= topLeftLimit.transform.position.y || newPos.y <= bottomRightLimit.transform.position.y)
            {
                return;
            }
            transform.position = newPos;
        }
        
    }

    private void OnMouseUp()
    {
        if (SceneController.Instance.isPlaying == false) return;
        if (isSnaped == false && Vector3.Distance(transform.position, parentObj.position) <= gameInfo.snapDistance)
        {
            SnapToParent();
        }
    }

    private void MovePuzzlePieceUp()
    {
        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, gameInfo.detectiveRange);
        if (others.Length <= 0) return;
        foreach(Collider2D other in others)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PuzzlePiece"))
            {
                SpriteRenderer tempSprite = other.GetComponent<SpriteRenderer>();
                if (tempSprite == null) continue;
                tempSprite.sortingOrder = 0;
            }
            
        }
        spriteRender.sortingOrder = 1;
    }

    private void SnapToParent()
    {
        transform.position = parentObj.position;
        transform.parent = parentObj;
        spriteRender.sortingOrder = -1;
        isSnaped = true;
        GetComponent<BoxCollider2D>().enabled = false;
        SceneController.Instance.AddPuzzlePieceUnit(1);
    }

    public void MoveToSpawn()
    {
        targetPos = new Vector2(Random.Range(topLeftLimit.position.x, bottomRightLimit.position.x),
            Random.Range(bottomRightLimit.position.y, topLeftLimit.position.y));
        isMove = true;
    }
}
