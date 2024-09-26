using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberHistory : MonoBehaviour
{
    [SerializeField] private CircleController circleController;
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject[] imgNumber;
    [SerializeField] private GameObject helperBlock;

    private bool wait;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController.isShowNumber = false;
        wait = true;
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(0.5f);
        wait = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (wait) return;

        if (circleController.GetLevel() == 0){
            helperBlock.transform.position = circleController.GetTransCircle(0).position;
            helperBlock.SetActive(true);
        }else helperBlock.SetActive(false);

        if (gameController.isShowNumber){
            int i = 0;
            while (i < circleController.GetMachineHistory().Count-1){
                imgNumber[i].transform.position = circleController.GetTransCircle(i).position;
                imgNumber[i].SetActive(true);
                i++;
            }
            gameController.isShowNumber = false;
        }

        if (!gameController.isShowNumber && gameController.isClick){
            foreach (GameObject img in imgNumber){
                img.SetActive(false);
            }
            gameController.isClick = false;
        }
    }
}
