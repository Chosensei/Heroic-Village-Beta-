using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Movement
{
    public class PlayerMovement : MonoBehaviour, IAction
    {
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
            anim.SetFloat("InputZ", inputZ, VerticalAnimTime, Time.deltaTime * 2f);
            anim.SetFloat("InputX", inputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

            //Calculate the Input Magnitude
            movementDirection = new Vector3(inputX, 0, inputZ);
            inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
            anim.SetFloat("InputMagnitude", inputMagnitude, StartAnimTime, Time.deltaTime);
            speed = inputMagnitude * movementSpeed;
            movementDirection.Normalize();

            if (movementDirection != Vector3.zero)
            {
                //Rotate player
                toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            //Physically move player
            velocity = movementDirection * speed;
            controller.Move(velocity * Time.deltaTime);

        }
        public void Cancel()
        {
            Fighter.isAttacking = false;
            velocity = Vector3.zero;
            Debug.Log("Cancel Movement");
        }
    }

}
