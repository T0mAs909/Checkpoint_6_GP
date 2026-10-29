using Fusion;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazePlayer : NetworkBehaviour
{
    [Header("Move")]
    [SerializeField] float speed;
    [SerializeField] CharacterController characterController;

    [Header("Look")]
    [SerializeField] MeshRenderer[] meshRenderer;
    [Networked, OnChangedRender(nameof(ApplyColor))]
    public byte TeamColor {  get; set; }


    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            TeamColor = (byte)(Runner.IsSharedModeMasterClient ? 0 : 1);
        }
        ApplyColor();
    }

    private void ApplyColor()
    {
        if(meshRenderer == null) return;
        meshRenderer[0].material.color = TeamColor == 0 ? Color.red : Color.green;
        meshRenderer[1].material.color = TeamColor == 0 ? Color.red : Color.green;
        meshRenderer[2].material.color = TeamColor == 0 ? Color.red : Color.green;
    }

    void Start()
    {

    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        MazeMatch match = MazeMatch.Instance;
        if(match == null || !match.CanPlay) return;
        Move();
    }

    void Move()
    {
        if (Keyboard.current == null) return;

        float x = (Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - (Keyboard.current.leftArrowKey.isPressed ? 1 : 0);
        float z = (Keyboard.current.upArrowKey.isPressed ? 1 : 0) - (Keyboard.current.downArrowKey.isPressed ? 1 : 0);

        Vector3 direction = new Vector3(x, 0, z).normalized;
        Vector3 velocity = direction * speed + Vector3.down * 2;

        characterController.Move(velocity * Runner.DeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "MazeEnd")
        {
            MazeMatch match = MazeMatch.Instance;
            match.RPC_ReportVictory(Object.StateAuthority);
        }
    }
}