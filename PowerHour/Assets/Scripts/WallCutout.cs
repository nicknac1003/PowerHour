using System.Collections.Generic;
using UnityEngine;

public class WallCutout : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private List<Material> wallMaterials;

    private float playerHeight;
    private float playerRadius;

    [SerializeField] private float cutoutRadius;
    [SerializeField] private float cutoutFade;

    private void Awake()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        playerHeight = playerController.playerHeight;
        playerRadius = playerController.playerRadius;
    }

    private void Update()
    {
        Vector2 cutoutCenter = Camera.main.WorldToViewportPoint(player.position + Vector3.up * playerHeight / 2);
        cutoutCenter.y /= Screen.width / Screen.height;

        foreach (Material material in wallMaterials)
        {
            material.SetVector("_PlayerPosition", player.position);// + playerBounds); // send flat position + bounds to shader to determine depth -- point closest to camera
            material.SetVector("_CutoutCenter", cutoutCenter);
            material.SetFloat("_CutoutRadius", cutoutRadius);
            material.SetFloat("_CutoutFade", cutoutFade);
        }
    }
}
