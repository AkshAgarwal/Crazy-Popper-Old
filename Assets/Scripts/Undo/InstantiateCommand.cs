using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCommand : IAction
{
    private Player.PLAYERCOLOR oldColor, newColor;
    private Vector3 positionToSpawn;

    private GameObject tospawnedGameObject, projectile, spawnedGameObject, currentPlayer;

    public InstantiateCommand(Player.PLAYERCOLOR oldColor, Player.PLAYERCOLOR newColor, Vector3 positionToSpawn, GameObject tospawnedGameObject, GameObject currentPlayer, GameObject projectileObject = null)
    {
        this.oldColor = oldColor;
        this.newColor = newColor;
        this.positionToSpawn = positionToSpawn;
        this.projectile = projectileObject;
        this.tospawnedGameObject = tospawnedGameObject;
        this.currentPlayer = currentPlayer;
    }

    public void ExecuteCommand()
    {
        if (projectile != null)
            this.projectile = projectile;
    }

    public void UndoCommand()
    {
        if (oldColor == Player.PLAYERCOLOR.Purple)
            spawnedGameObject = GameObject.Instantiate(tospawnedGameObject, positionToSpawn, Quaternion.identity);
        else
            spawnedGameObject = currentPlayer;
        spawnedGameObject.transform.parent = GameObject.FindObjectOfType<GridLevel>().parentHolder.transform;
        Player playerScript = spawnedGameObject.GetComponent<Player>();
        CheckPlayerState(oldColor, playerScript);
        if (projectile != null)
            GameObject.Destroy(projectile);
    }
    public void CheckPlayerState(Player.PLAYERCOLOR color, Player player)
    {//CHANGE PLAYER STATE WHEN IT POPS
        switch (color)
        {
            case Player.PLAYERCOLOR.Yellow:
                player.tag = "yellow";
                player.playerTapCount = 3;
                player.GetComponent<SpriteRenderer>().sprite = GameManager.instance.yellow;
                break;
            case Player.PLAYERCOLOR.Blue:
                player.tag = "blue";
                player.playerTapCount = 2;
                player.GetComponent<SpriteRenderer>().sprite = GameManager.instance.blue;
                break;
            case Player.PLAYERCOLOR.Purple:
                player.tag = "purple";
                player.playerTapCount = 1;
                GameManager.instance.totalPlayerInLevel += 1;
                player.GetComponent<SpriteRenderer>().sprite = GameManager.instance.purple;
                break;
            default:
                Debug.Log("Invalid color");
                break;
        }
        GameManager.instance.OnUndo();
    }
}
