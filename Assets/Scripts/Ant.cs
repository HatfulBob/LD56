
using System;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [Header("Personal Properties")]
    public int xPosition,yPosition;
    public Ant.Direction directionFacing;
    public Sprite sprite;
    public Animator animator;
    public bool isMoving = false;

    [Header("Animations")]
    public Animation northMoving;
    public Animation southMoving;
    public Animation leftMoving;
    public Animation rightMoving;

    public enum Direction { NORTH,SOUTH,LEFT,RIGHT}
    public enum Job { DEFAULT,PUSHER,FIGHTER}

    // Start is called before the first frame update
    void Start()
    {
        xPosition = (int)transform.position.x;
        yPosition = (int)transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        DrawSprite();
    }

    private void DrawSprite()
    {
        //set the sprite/animation to the correct one based on if moving
        switch (directionFacing)
        {

        }
    }
}
