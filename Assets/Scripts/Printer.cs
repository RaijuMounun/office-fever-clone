using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Printer : MonoBehaviour
{
    [SerializeField] private Transform[] papersPlace = new Transform[10];
    [SerializeField] private GameObject paper;
    public float paperDeliveryTime, YAxis;
    public int countPapers;

    private void Awake()
    {
        for (int i = 0; i < papersPlace.Length; i++)
        {
            papersPlace[i] = transform.GetChild(0).GetChild(i);
        }
    }

    private void Start()
    {
        StartCoroutine(PrintPaper(paperDeliveryTime));
    }


    public IEnumerator PrintPaper(float Time)
    {
        var PP_index = 0;

        while (countPapers < 100)
        {
            GameObject NewPaper = Instantiate(paper, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity, transform.GetChild(1));

            NewPaper.transform.DOJump(new Vector3(papersPlace[PP_index].position.x, papersPlace[PP_index].position.y + YAxis, papersPlace[PP_index].position.z), 2f, 1, .5f).SetEase(Ease.OutQuad);

            if (PP_index < 9)
                PP_index++;
            else
            {
                PP_index = 0;
                YAxis += .026f;
            }

            yield return new WaitForSecondsRealtime(Time);
        }
    }
}
