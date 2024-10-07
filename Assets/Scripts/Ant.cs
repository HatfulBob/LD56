
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    bool canMove = true;
    LevelManager levelManager;
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
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        xPosition = (int)(transform.position.x);
        yPosition = (int)(transform.position.y);
        //temporary adjustments
        if (directionFacing == Direction.SOUTH)
            yPosition = (int)Math.Ceiling(transform.position.y);
        if (directionFacing == Direction.LEFT)
            yPosition = (int)Math.Ceiling(transform.position.y);
        if (directionFacing == Direction.RIGHT)
            xPosition = (int)Math.Floor(transform.position.x);
        if (directionFacing == Direction.NORTH)
            xPosition = (int)Math.Floor(transform.position.x);

        if (waitPerMovementCurrent < waitPerMovement)
        {
            waitPerMovementCurrent += Time.deltaTime;
        }
        else
        {
            //check if I need to change direction
            var tileUnderMe = tilemap.GetTile(new Vector3Int(xPosition, yPosition));
            Debug.Log($"I am standing on {tileUnderMe.name}");
            if (tileUnderMe.name.Contains("up"))
            {
                directionFacing = Direction.NORTH;
                canMove = true;
            }
            else if (tileUnderMe.name.Contains("right"))
            {
                directionFacing = Direction.RIGHT;
                canMove = true;

            }
            else if (tileUnderMe.name.Contains("left"))
            {
                directionFacing = Direction.LEFT;
                canMove = true;

            }
            else if (tileUnderMe.name.Contains("down"))
            {
                directionFacing = Direction.SOUTH;
                canMove = true;

            }
            if (isMoving)
            {
                //make a move
                if (!canMove)
                {

                    waitPerMovementCurrent = 0;
                    return;
                }
                if (xPosition == destination.x && yPosition == destination.y)
                {
                    isMoving = false;
                    //destination = null;
                }
                else
                {
                    MoveCloser(xPosition, yPosition);
                }
                waitPerMovementCurrent = 0;
            }


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
            if (levelManager.IsThereAnAnt(directionIWishToGo.x, directionIWishToGo.y))
            {
                destination = new Vector3Int((int)transform.position.x, (int)transform.position.y);
                isMoving = false;
            } else
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

    private void MoveCloser(int xPosition, int yPosition)
    {
        switch (directionFacing)
        {
            case Direction.LEFT:
                transform.position = new Vector3(transform.position.x - speed, yPosition); break;
            case Direction.RIGHT:
                transform.position = new Vector3(transform.position.x + speed, yPosition); break;
            case Direction.SOUTH:
                transform.position = new Vector3(xPosition, transform.position.y - speed); break;
            case Direction.NORTH:
                transform.position = new Vector3(xPosition, transform.position.y + speed); break;

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        CollisionCode(collision);
    }

    private void OnTriggerStay(Collider collision)
    {

        CollisionCode(collision);
    }

    private void CollisionCode(Collider collision)
    {
        if (collision.name.Equals("Boulder"))
        {
            canMove = false;
            Vector3Int directionAheadOfBoulder = new Vector3Int((int)transform.position.x, (int)transform.position.y);
            switch (directionFacing)
            {
                case Direction.LEFT:
                    directionAheadOfBoulder = new Vector3Int(xPosition - 2, yPosition); break;
                case Direction.RIGHT:
                    directionAheadOfBoulder = new Vector3Int(xPosition + 2, yPosition); break;
                case Direction.SOUTH:
                    directionAheadOfBoulder = new Vector3Int(xPosition, yPosition - 2); break;
                case Direction.NORTH:
                    directionAheadOfBoulder = new Vector3Int(xPosition, yPosition + 2); break;
            }
            if (collision.GetComponent<Boulder>().hasBeenPushed && collision.GetComponent<Boulder>().lastDirectionPushed != directionFacing)
                return;
            else
            {
                collision.GetComponent<Boulder>().hasBeenPushed = true;
                collision.GetComponent<Boulder>().lastDirectionPushed = directionFacing;

            }

            var m = tilemap.GetTile(directionAheadOfBoulder);

            //if (m.name.Contains("GroundRule"))
            if (!m.name.ToLower().Contains("grass"))
            {
                //push boulder
                collision.GetComponent<Collider>().transform.position = directionAheadOfBoulder;
                isMoving = true;
                canMove = true;
            }
            else
            {
                //its a wall, cant move
                isMoving = false;
            }

        }
        //else if (collision.name.Contains("Ant"))
        //{
        //    //its a wall, cant move
        //    canMove = false;
        //    isMoving = false;

        //}
        else if (collision.name.Equals("Goal"))
        {
            levelManager.savedAnts++;
            gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canMove = true;
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
