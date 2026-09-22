using UnityEngine;
using System.Collections;

public class Conveyerbelt : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    public float moveTime = 0.5f;
    public float tileSize = 2f;
    private bool movingPlayer = false;

    //moves player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !movingPlayer)
        {
            StartCoroutine(MovePlayer(other.transform));
        }
    }

    //moves player one tile and centers them on the grid
    IEnumerator MovePlayer(Transform player)
    {
        movingPlayer = true;

        yield return new WaitForSeconds(moveTime);

        //moves player one tile
        player.position += direction.normalized * tileSize;

        //snaps player exactly to the grid
        Player playerScript = player.GetComponent<Player>();

        if (playerScript != null)
        {
            playerScript.SnapToGrid();
            playerScript.CheckTile();
        }

        movingPlayer = false;
    }
}