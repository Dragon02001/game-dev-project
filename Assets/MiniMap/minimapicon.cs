using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapicon : MonoBehaviour
{

    public Transform player;
    public Camera miniMapCamera;

    private void LateUpdate()
    {
        // Transform the player's position to the mini-map space
        Vector3 miniMapPosition = miniMapCamera.WorldToViewportPoint(player.position);
        transform.position = miniMapPosition;
    }


}
