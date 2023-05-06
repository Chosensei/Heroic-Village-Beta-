using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

namespace RPG.Movement
{
    public class PlayerMovement : MonoBehaviour, IAction
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] float movementSpeed;
        [SerializeField] float rotationSpeed;
        [Header("Animation Smoothing")]
        [Range(0, 1f)]
        public float HorizontalAnimSmoothTime = 0.2f;
        [Range(0, 1f)]
        public float VerticalAnimTime = 0.2f;
        [Range(0, 1f)]
        public float StartAnimTime = 0.3f;
        public static bool IsMoving = false;
        private bool isCastingSpell = false;
        private Animator anim;
        private CharacterController controller;
        private float InputX;
        private float InputZ;
        private float inputMagnitude;
        private float speed;
        private Vector3 movementDirection;
        private Vector3 velocity;
        private Quaternion toRotation;


        void Start()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            //Calculate Input Vectors
            InputX = Input.GetAxis("Horizontal");
            InputZ = Input.GetAxis("Vertical");

            // move if not attacking
            if (Fighter.isAttacking == false)
            {
                StartMoveAction();
            }

        }

        public void StartMoveAction()
        {
            GetComponent<ActionScheduler>().StartAction(this);
            StartLocomotion(InputX, InputZ);
        }

        public void StartLocomotion(float inputX, float inputZ)
        {
            // Get the forward direction of the virtual camera
            Vector3 cameraForward = virtualCamera.transform.forward;
            cameraForward.y = 0f; // make sure the direction is horizontal
            cameraForward.Normalize();

            // Calculate the movement direction relative to the camera
            movementDirection = inputZ * cameraForward + inputX * virtualCamera.transform.right;

            // Calculate the input magnitude
            inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
            anim.SetFloat("InputMagnitude", inputMagnitude, StartAnimTime, Time.deltaTime);

            // Rotate the movement direction towards the camera
            if (movementDirection != Vector3.zero)
            {
                toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            // Calculate the speed and velocity
            speed = inputMagnitude * movementSpeed;
            velocity = movementDirection * speed;

            // Move the character
            controller.Move(velocity * Time.deltaTime);

            // Update the animator
            anim.SetFloat("InputZ", inputZ, VerticalAnimTime, Time.deltaTime * 2f);
            anim.SetFloat("InputX", inputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);
        }

        public void Cancel()
        {
            Fighter.isAttacking = false;
            velocity = Vector3.zero;
            Debug.Log("Cancel Movement");
        }
    }
}
