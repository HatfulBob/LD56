using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    [SerializeField] 
    private Tilemap tilemap;
    [SerializeField]
    private Tile leftArrow;
    [SerializeField]
    private Tile rightArrow;
    [SerializeField]
    private Tile downArrow;
    [SerializeField]
    private Tile upArrow;
    [SerializeField] 
    private RuleTile groundTile;
    [SerializeField] 
    private Color hoverColour;
    [SerializeField] 
    private Color regularColour;

    private List<Vector3Int> highlightedTiles;
    // Start is called before the first frame update

    private ArrowDirection currentTile = ArrowDirection.Left;
    void Start()
    {
        highlightedTiles = new List<Vector3Int>();
    }

    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (hit.collider != null)
        {
            var hitPosition = hit.point;
            var tilePosition  = tilemap.WorldToCell(hitPosition);
            var tileInfo = tilemap.GetTile(tilePosition);

            if (tileInfo.name.ToLower() != "grass")
            {
                tilemap.SetColor(tilePosition, hoverColour);
                highlightedTiles.Add(tilePosition);
            }

            var filteredList = new List<Vector3Int>();
            foreach (var tile in highlightedTiles)
            {
                if (tile != tilePosition)
                {
                    tilemap.SetColor(tile, regularColour);
                }
                else
                {
                    filteredList.Add(tile);
                }
            }

            highlightedTiles = filteredList;
        }
        else
        {
            foreach (var tile in highlightedTiles)
            {
                tilemap.SetColor(tile, regularColour);
            }

            highlightedTiles = new List<Vector3Int>();
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentTile = (ArrowDirection) (int)currentTile + 1;
            if (currentTile > ArrowDirection.Up)
            {
                currentTile = ArrowDirection.Left;
            }
            
            Debug.Log("Tile is now: " + currentTile);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            if (hit.collider != null)
            {
                var hitPosition = hit.point;
                var tilePosition  = tilemap.WorldToCell(hitPosition);
                var tileInfo = tilemap.GetTile(tilePosition);

                if (tileInfo.name != "GroundRule")
                {
                    return;
                }
                
                if (currentTile == ArrowDirection.Left)
                {
                    tilemap.SetTile(tilePosition , leftArrow);
                    Debug.Log("Left Arrow");
                }
                else if (currentTile == ArrowDirection.Right)
                {
                    tilemap.SetTile(tilePosition , rightArrow);
                    Debug.Log("Right Arrow");
                }
                else if (currentTile == ArrowDirection.Up)
                {
                    tilemap.SetTile(tilePosition , upArrow);
                    Debug.Log("Up Arrow");
                }
                else if (currentTile == ArrowDirection.Down)
                {
                    tilemap.SetTile(tilePosition , downArrow);
                    Debug.Log("Down Arrow");
                }
            }
            else
            {
                Debug.Log("No Hit");
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (hit.collider != null)
            {   
                var hitPosition = hit.point;
                var tilePosition  = tilemap.WorldToCell(hitPosition);
                var tileInfo = tilemap.GetTile(tilePosition);
                if (tileInfo.name.ToLower() == "grass")
                {
                    return;
                }
                tilemap.SetTile(tilePosition , groundTile);
            }
        }
    }
}

public enum ArrowDirection
{
    Left,
    Right,
    Down,
    Up
}
