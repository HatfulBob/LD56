
using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Ant : MonoBehaviour
{
    [Header("Personal Properties")]
    public int xPosition, yPosition;
    public Ant.Direction directionFacing;
    public bool isMoving = false;
    public float waitPerMovement = 0.2f;//TODO: PUT IN GENERAL GAME CONTROLLER?
    public float waitPerMovementCurrent = 0;
    public float speed = 0.33f;


    [Header("Animations")]
    public AnimationClip northMoving;
    //public AnimationClip southMoving;
    //public AnimationClip leftMoving;
    public AnimationClip rightMoving;
    public AnimationClip rightIdle, northIdle;



    Tilemap tilemap;
    SpriteRenderer sprite;
    //Animation anim;
    Animator animator;
    Vector3Int destination;

    public enum Direction { NORTH, SOUTH, LEFT, RIGHT }
    public enum Job { DEFAULT, PUSHER, FIGHTER }

    // Start is called before the first frame update
    void Start()
    {
        directionFacing = Direction.NORTH;//TODO: This should be set based on the spawner I came out from
        //anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        tilemap = FindObjectOfType<Tilemap>();
        xPosition = (int)transform.position.x;
        yPosition = (int)transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        xPosition = (int)(transform.position.x);
        yPosition = (int)(transform.position.y);
        //temporary adjustments
        if (directionFacing == Direction.SOUTH)
            yPosition++;
        if (directionFacing == Direction.RIGHT)
            xPosition--;

        if (waitPerMovementCurrent < waitPerMovement)
        {
            waitPerMovementCurrent += Time.deltaTime;
        }
        else
        {
            //make a move
            if (isMoving)
            {
                if (xPosition == destination.x && yPosition == destination.y)
                {
                    isMoving = false;
                    //destination = null;
                }
                else
                {
                    MoveCloser();
                }
                waitPerMovementCurrent = 0;
            }

            //check if I need to change direction
            //TODOOOOOOOOOO

            //can I move?
            Vector3Int directionIWishToGo = new Vector3Int((int)transform.position.x, (int)transform.position.y);
            switch (directionFacing)
            {
                case Direction.LEFT:
                    directionIWishToGo = new Vector3Int(xPosition - 1, yPosition); break;
                case Direction.RIGHT:
                    directionIWishToGo = new Vector3Int(xPosition + 1, yPosition); break;
                case Direction.SOUTH:
                    directionIWishToGo = new Vector3Int(xPosition, yPosition - 1); break;
                case Direction.NORTH:
                    directionIWishToGo = new Vector3Int(xPosition, yPosition + 1); break;
            }

            var m = tilemap.GetTile(directionIWishToGo);

            //if (m.name.Contains("GroundRule"))
            if (!m.name.ToLower().Contains("grass"))
            {
                destination = directionIWishToGo;
                isMoving = true;
            }
            else
            {
                //its a wall, cant move
                isMoving = false;
            }

            waitPerMovementCurrent = 0;
        }
    }

    private void MoveCloser()
    {
        switch (directionFacing)
        {
            case Direction.LEFT:
                transform.position = new Vector3(transform.position.x - speed, (int)transform.position.y); break;
            case Direction.RIGHT:
                transform.position = new Vector3(transform.position.x + speed, (int)transform.position.y); break;
            case Direction.SOUTH:
                transform.position = new Vector3((int)transform.position.x, transform.position.y - speed); break;
            case Direction.NORTH:
                transform.position = new Vector3((int)transform.position.x, transform.position.y + speed); break;
        }
    }

    public void OnAnimationEnd()
    {
        DrawSprite();
    }
    private void DrawSprite()
    {
        animator.SetBool("isMoving", isMoving);
        switch (directionFacing)
        {
            case Direction.LEFT:
                animator.SetBool("isFacingAway", true); break;
            case Direction.RIGHT:
                animator.SetBool("isFacingAway", false); break;
            case Direction.SOUTH:
                animator.SetBool("isFacingAway", false); break;
            case Direction.NORTH:
                animator.SetBool("isFacingAway", true); break;
        }
    }

    /*
    private void DrawSprite()
    {
        //set the sprite/animation to the correct one based on if moving
        switch (directionFacing)
        {
            case Direction.LEFT:
                sprite.flipX = true;

                if (isMoving)
                {
                    if ((!anim.isPlaying || anim.clip.name.Contains("Idle"))&&anim.clip.name!= "rightMovement")
                    {
                        anim.clip = rightMoving;
                        anim.Play("rightMovement");
                    }
                }
                else if (!anim.isPlaying)
                {
                    anim.Play("rightIdle");
                }
                break;
            case Direction.SOUTH:
                sprite.flipX = false;
                if (isMoving)
                {
                    if ((!anim.isPlaying || anim.clip.name.Contains("Idle"))&& anim.clip.name != "rightMovement")
                    {
                        anim.clip = rightMoving;
                        anim.Play("rightMovement");
                    }
                }
                else if (!anim.isPlaying)
                {
                    anim.Play("rightIdle");
                }
                break;
            case Direction.RIGHT:
                sprite.flipX = false;
                if (isMoving)
                {
                    if ((!anim.isPlaying || anim.clip.name.Contains("Idle")) && anim.clip.name != "upMovement")
                    {
                        anim.Play("upMovement");
                    }
                }
                else if (!anim.isPlaying)
                {
                    anim.Play("upIdle");
                }
                break;
            case Direction.NORTH:
                sprite.flipX = true;
                if (isMoving)
                {
                    if ((!anim.isPlaying || anim.clip.name.Contains("Idle")) && anim.clip.name != "upMovement")
                    {
                        anim.Play("upMovement");
                    }
                }
                else if (!anim.isPlaying)
                {
                    anim.Play("upIdle");
                }
                break;
        }

    }*/
}
