using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    [SerializeField] private GameObject passPanel;

    [HideInInspector]
    public string[] circleName = {"Circle-0", "Circle-1", "Circle-2", "Circle-3", "Circle-4", "Circle-5", "Circle-6", "Circle-7", "Circle-8", "Circle-9"};
    private static Transform[] transCircle = new Transform[10];
    private AudioSource audioSource;
    

    private static string cur_pos_click;
    private static List<string> machineHistory = new List<string>();
    private static int Level;
    private int countStep;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        Level = 0;
        countStep = 0;
    }

    public void SetClickPosition(string new_pos){
        cur_pos_click = new_pos;
        // Debug.Log("Click : " + cur_pos_click);
    }

    public string GetCurrentPosition(){
        return cur_pos_click;
    }

    
    public void AddMachine(string circle){
        machineHistory.Add(circle);
        // ShowMachineHistory();
    }

    public void ShowMachineHistory(){
        Debug.Log("Machine history: " + string.Join(", ", machineHistory));
    }

    public List<string> GetMachineHistory(){
        return machineHistory;
    }

    public void ClearMachineHistory(){
        machineHistory.Clear();
    }

    public void SetTransCircle(Transform trans, int number){
        transCircle[number] = trans;
    }

    public Transform GetTransCircle(int value){
        if (value == -1)
            return transCircle[GetNumberInName(machineHistory[machineHistory.Count - 1])];
        else if (value == -2)
            return transCircle[GetNumberInName(machineHistory[machineHistory.Count - 2])];
        else if (value == -3)
            return transCircle[GetNumberInName(cur_pos_click)];
        else
            return transCircle[GetNumberInName(machineHistory[value])];
    }


    public int GetLevel(){
        return Level;
    }

    public void LevelUp(){
        Level += 1;
    }

    public int GetNumberInName(string name){
        string[] parts = name.Split('-');
        if (parts.Length > 1 && int.TryParse(parts[parts.Length - 1], out int number)){
            return number;
        }
        return 0;
    }

    public void CheckStep(string new_pos){
        if (machineHistory.Count > 0 && machineHistory[0] == new_pos){
            RightMove(new_pos);
            passPanel.SetActive(false);
        }
        else{
            audioSource.Play();
            passPanel.SetActive(true);
        }
    }

    private void RightMove(string new_pos){
        SetClickPosition(new_pos);

        string pos_move;
        while (true){
            pos_move = circleName[Random.Range(0, 10)];
            if (!GetMachineHistory().Contains(pos_move)){
                AddMachine(pos_move);
                break;
            }
        }
        machineHistory.RemoveAt(0);
        

        countStep += 1;
        if (Level == 0 || Level == 1){
            if (countStep == 3){
                LevelUp();
                countStep = 0;
            }
        }
        else{
            if (countStep == 5){
                LevelUp();
                countStep = 0;
            }
        }
    }
}
