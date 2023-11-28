using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Tile blueSquare;
    public Tile greenSquare;
    public Tile pinkSquare;
    public Tile bulletTile;
    public Tile gearTile;
    public Tile chipTile;
    public Tile beamTile;
    public Tile enemyTile;
    public List<Gun> guns = new();
    public List<Vector3Int> bullets;
    public List<Vector3Int> enemies;
    public Vector3Int playerPosition;
    public Vector3Int[] possiblePositions;
    public Tilemap playerTilemap;
    public Tilemap playerMovementTilemap;
    public Tilemap gunsTilemap;
    public Tilemap bulletsTilemap;
    public Tilemap resourcesTilemap;
    public Tilemap enemiesTilemap;

    private void Start()
    {
        enemies = new List<Vector3Int> { new(15, 2, 0), new(17, 0, 0), new(17, -1, 0) };
        possiblePositions = new[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Paint();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var clickedCell = playerMovementTilemap.WorldToCell(mp);
            if (possiblePositions.Contains(clickedCell))
            {
                ChangePosition(clickedCell);
                GameMove();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var clickedCell = playerMovementTilemap.WorldToCell(mp);
            if (possiblePositions.Contains(clickedCell))
            {
                if (clickedCell.x >= 0)
                {
                    SetGun(new Gun(clickedCell));
                }
                else
                {
                    if (resourcesTilemap.HasTile(clickedCell))
                    {
                        var type = resourcesTilemap.GetTile(clickedCell);
                        if (type == gearTile)
                        {
                            Bag.GetResources(1, 0, 0);
                        }
                        else if (type == beamTile)
                        {
                            Bag.GetResources(0, 1, 0);
                        }
                        else
                        {
                            Bag.GetResources(0, 0, 1);
                        }

                        resourcesTilemap.SetTile(clickedCell, null);
                    }
                }

                GameMove();
            }
        }
    }

    public void Paint()
    {
        playerTilemap.SetTile(playerPosition, blueSquare);
        foreach (var pos in possiblePositions)
        {
            playerMovementTilemap.SetTile(pos, greenSquare);
        }
    }

    public void Clear()
    {
        playerTilemap.SetTile(playerPosition, null);
        foreach (var pos in possiblePositions)
        {
            playerMovementTilemap.SetTile(pos, null);
        }
    }

    public void ChangePosition(Vector3Int pos)
    {
        Clear();
        playerPosition = pos;
        possiblePositions = new[]
            { pos + Vector3Int.up, pos + Vector3Int.down, pos + Vector3Int.left, pos + Vector3Int.right };
        Paint();
    }

    public void SetGun(Gun gun)
    {
        if (Bag.GearsCount == 0)
        {
            return;
        }

        Bag.SpendResources(1, 0, 0);
        gunsTilemap.SetTile(gun.Position, pinkSquare);
        guns.Add(gun);
    }

    public void GameMove()
    {
        foreach (var gun in guns)
        {
            var gunAction = gun.MakeMove();
            if (gunAction == 1)
            {
                var bulletPos = gun.Position;
                bulletsTilemap.SetTile(bulletPos, bulletTile);
                bullets.Add(bulletPos);
            }
        }

        var newBullets = new List<Vector3Int>();
        for (var i = 0; i < bullets.Count; i++)
        {
            bulletsTilemap.SetTile(bullets[i], null);
            bullets[i] += Vector3Int.right;
            bulletsTilemap.SetTile(bullets[i], bulletTile);
            var newEnemies = new List<Vector3Int>();
            var bulletIsCollided = false;
            for (var j = 0; j < enemies.Count; j++)
            {
                if (enemies[j] == bullets[i])
                {
                    enemiesTilemap.SetTile(enemies[j], null);
                    bulletsTilemap.SetTile(bullets[i], null);
                    bulletIsCollided = true;
                }
                else
                {
                    newEnemies.Add(enemies[j]);
                }
            }

            if (!bulletIsCollided)
            {
                newBullets.Add(bullets[i]);
            }

            enemies = newEnemies;
        }

        bullets = newBullets;


        var newEnemies2 = new List<Vector3Int>();
        for (var i = 0; i < enemies.Count; i++)
        {
            enemiesTilemap.SetTile(enemies[i], null);
            enemies[i] += Vector3Int.left;
            enemiesTilemap.SetTile(enemies[i], enemyTile);
            var newBullets2 = new List<Vector3Int>();
            var enemyIsCollided = false;
            for (var j = 0; j < bullets.Count; j++)
            {
                if (bullets[j] == enemies[i])
                {
                    enemiesTilemap.SetTile(enemies[i], null);
                    bulletsTilemap.SetTile(bullets[j], null);
                    enemyIsCollided = true;
                }
                else
                {
                    newBullets2.Add(bullets[j]);
                }
            }

            if (!enemyIsCollided)
            {
                newEnemies2.Add(enemies[i]);
            }

            bullets = newBullets2;
        }

        enemies = newEnemies2;
    }
    // private void OnMouseDown()
    // {
    //     Debug.Log(Input.mousePosition);
    // }
}