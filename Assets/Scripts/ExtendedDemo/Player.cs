using System;
using UnityEngine;

namespace DialogueSystem
{
    public class Player : MonoBehaviour
    {
        private readonly int MOVE = Animator.StringToHash("move");

        [SerializeField] private InputReader inputReader;
        [SerializeField] private float walkSpeed = 2f;
        [Tooltip("How fast the animations will transition")]
        [SerializeField] private float moveDamp = 0.1f;
        [Tooltip("How fast the character is turning to the correct direction")]
        [SerializeField] private float rotationSpeed = 10f;
        [Tooltip("How far the character can detect nearby npc")]
        [SerializeField] private float detectionRadius = 2f;
        [SerializeField] private LayerMask npcLayer;
        private Animator animator;
        private CharacterController characterController;
        private Vector2 moveInput;
        private NPC targetNPC;

        void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            inputReader.MoveEvent += HandleMove;
            inputReader.InteractEvent += InteractEvent;
        }

        private void InteractEvent()
        {


            if (targetNPC != null)
            {
                targetNPC.Interact();
            }
        }

        private void HandleMove(Vector2 input)
        {
            moveInput = -input;
        }


        void Update()
        {
            //Move player
            if (moveInput != Vector2.zero)
            {
                Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y) * walkSpeed;
                characterController.SimpleMove(direction);
            }
            else
            {
                characterController.SimpleMove(Vector3.zero);
            }

            //Play animation
            float target = moveInput != Vector2.zero ? 1f : 0f;
            animator.SetFloat(MOVE, target, moveDamp, Time.deltaTime);

            // Rotate player to face movement direction
            Vector3 lookDir = new Vector3(moveInput.x, 0f, moveInput.y);
            if (lookDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            Collider[] results = Physics.OverlapSphere(transform.position, detectionRadius, npcLayer);
            if (results.Length > 0)
            {
                targetNPC = results[0].GetComponent<NPC>();
                //TODO make the npc detect the player and display the prompt
                //ShowPrompt inside Player looks wrong
                targetNPC.ShowPrompt();
                //print("npc found");
            }
            else
            {
                if (targetNPC != null)
                {
                    targetNPC.HidePrompt();
                    targetNPC = null;
                    //print("npc lost");
                }
            }

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            // 1. Draw the origin sphere
            Gizmos.DrawWireSphere(origin, detectionRadius);

            // 2. Draw the path and the "End" sphere
            RaycastHit hit;
            if (Physics.SphereCast(origin, detectionRadius, direction, out hit, 0, npcLayer))
            {
                // Draw line to the hit point
                Gizmos.DrawLine(origin, hit.point);
                // Draw sphere where it hit
                Gizmos.DrawWireSphere(origin + direction * hit.distance, detectionRadius);
            }
            else
            {
                // Draw the full length if nothing was hit
                Gizmos.DrawLine(origin, origin + direction * 0);
                Gizmos.DrawWireSphere(origin + direction * 0, detectionRadius);
            }
        }

        void OnDestroy()
        {
            inputReader.MoveEvent -= HandleMove;
            inputReader.InteractEvent -= InteractEvent;
        }
    }
}
