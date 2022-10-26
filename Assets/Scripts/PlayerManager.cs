using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerManager : MonoBehaviour
{
    private Vector3 direction;
    private Camera cam;
    private Animator playerAnimator;
    [FoldoutGroup("Player"), SerializeField] private float playerSpeed = 20f;

    private void Awake()
    {
        cam = Camera.main;
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
                direction = ray.GetPoint(distance);


            transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x, 0f, direction.z), playerSpeed * Time.deltaTime);

            #region Fix
            //If the mouse is over the character, the character lies on the ground.
            var offset = direction - transform.position;

            if (offset.magnitude > .1f)
                transform.LookAt(direction);
            #endregion
            
        }

        #region Running Anim
        if (Input.GetMouseButton(0))
        {
            playerAnimator.SetBool("isRunning", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("isRunning", false);
        }
        #endregion
    }
}
