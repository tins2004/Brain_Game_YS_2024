using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{

    [SerializeField] private string circleName;
    [SerializeField] private CircleController circleController;
    [SerializeField] private GameController gameController;

    void Start()
    {
        circleName = this.gameObject.name;

        circleController.SetTransCircle(this.gameObject.transform, circleController.GetNumberInName(circleName));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown(){
        if(Input.GetMouseButtonDown(0) && gameController.playerCanMove && !gameController.isFly){
            
            if (circleController.GetCurrentPosition() != circleName){
                gameController.isClick = true;
                gameController.machineCanMove = false;
                circleController.CheckStep(circleName);
            }
        }
    }
}
