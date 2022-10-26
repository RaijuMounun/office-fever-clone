using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private Vector3 direction;
    private Camera cam;
    private Animator playerAnimator;
    [FoldoutGroup("Player"), SerializeField] private float playerSpeed = 20f;
    [SerializeField] private List<Transform> papers = new List<Transform>();
    [SerializeField] private Transform paperPlace;

    private void Awake()
    {
        cam = Camera.main;
        playerAnimator = GetComponent<Animator>();

        papers.Add(paperPlace);
    }

    private void Update()
    {
        #region Player Movement
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
        #endregion

        #region Running Anim
        if (Input.GetMouseButton(0))
        {
            playerAnimator.SetBool("isRunning", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("isRunning", false);
        }

        if (papers.Count <= 1)
        {
            playerAnimator.SetBool("carry", false);
        }
        #endregion

        

        #region Separating papers from printer
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            if (hit.collider.CompareTag("Table") && papers.Count < 21)
            {
                if (hit.collider.transform.parent.GetChild(1).childCount > 2)
                {
                    var paper = hit.collider.transform.parent.GetChild(1).GetChild(1);
                    paper.rotation = Quaternion.Euler(paper.rotation.x, Random.Range(0f, 180f), paper.rotation.z);
                    papers.Add(paper);
                    paper.parent = null;

                    if (hit.collider.transform.parent.GetComponent<Printer>().countPapers > 1)
                        hit.collider.transform.parent.GetComponent<Printer>().countPapers--;

                    if (hit.collider.transform.parent.GetComponent<Printer>().YAxis > 0f)
                        hit.collider.transform.parent.GetComponent<Printer>().YAxis -= 0.025f;

                    playerAnimator.SetBool("carry", true);
                    playerAnimator.SetBool("isRunning", false);
                }
            }
        }
        #endregion

        #region Paper anim
        if (papers.Count > 1)
        {
            for (int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i - 1);
                var secondPaper = papers.ElementAt(i);

                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * 150f),         //X
                                                   Mathf.Lerp(secondPaper.position.y, firstPaper.position.y + 0.025f, Time.deltaTime * 15f),//y
                                                   Mathf.Lerp(secondPaper.position.z, firstPaper.position.z, Time.deltaTime * 150f));                                                                 //z
            }
        }
        #endregion  
    }
}
