using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] private CircleController machineController;
    [SerializeField] private GameController gameController;
    [SerializeField] private ParticleSystem yourParticleSystem;
    [SerializeField] private ParticleSystem yourParticleSystemBig;
    [SerializeField] private float speedMachine;
    [SerializeField] private AudioClip audioLevelUp, AudioJump;

    private Animator anim;
    private AudioSource audioSource;

    private int cur_level;
    private bool is_2_step;
    private Transform cur_trans;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        
        machineController.ClearMachineHistory();
        // this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -4f);
        int pos_move_int = Random.Range(1, 10);
        string pos_move = machineController.circleName[pos_move_int];
        machineController.AddMachine(pos_move);
        machineController.SetClickPosition(machineController.circleName[pos_move_int-1]);
        while (true){
            pos_move = machineController.circleName[Random.Range(0, 10)];
            if (!machineController.GetMachineHistory().Contains(pos_move) && !machineController.GetMachineHistory().Contains(machineController.GetCurrentPosition())){
                machineController.AddMachine(pos_move);
                break;
            }
        }

        gameController.machineCanMove = false;
        StartCoroutine(WaitToStart());
        StartCoroutine(LetStartGame());
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1f);
        // this.gameObject.SetActive(true);
        cur_level = 0;
        is_2_step = false;
        cur_trans = machineController.GetTransCircle(0);
        audioSource.clip = AudioJump;
        audioSource.Play();
        this.transform.position = cur_trans.position;
        // this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -4f);
    }

    IEnumerator LetStartGame()
    {
        yield return new WaitForSeconds(2f);
        gameController.machineCanMove = true;
    }


    void Update(){
        if (!gameController.machineCanMove) return;
        
        if (cur_level < machineController.GetLevel()){
            if (cur_level < 6){
                is_2_step = true;
                string pos_move;
                while (true){
                    pos_move = machineController.circleName[Random.Range(0, 10)];
                    if (!machineController.GetMachineHistory().Contains(pos_move) && !machineController.GetMachineHistory().Contains(machineController.GetCurrentPosition())){
                        machineController.AddMachine(pos_move);
                        break;
                    }
                }
            }
            else{
                speedMachine += 5;
                yourParticleSystemBig.Play();
            }
            cur_level = machineController.GetLevel();
        }

        if (cur_trans.position != machineController.GetTransCircle(-1).position){
            gameController.playerCanMove = false;
            if (is_2_step){
                MachineRotation(machineController.GetTransCircle(-2));
                anim.SetBool("Jump", true);
                this.transform.position = Vector3.MoveTowards(this.transform.position, machineController.GetTransCircle(-2).position, speedMachine * Time.deltaTime);
                if (this.transform.position == machineController.GetTransCircle(-2).position){
                    is_2_step = false;
                    gameController.playerCanMove = true;
                    anim.SetBool("Jump", false);
                    yourParticleSystemBig.Play();
                    audioSource.clip = audioLevelUp;
                    audioSource.Play();
                }
            }
            else{
                MachineRotation(machineController.GetTransCircle(-1));
                anim.SetBool("Jump", true);
                this.transform.position = Vector3.MoveTowards(this.transform.position, machineController.GetTransCircle(-1).position, speedMachine * Time.deltaTime);
                if (this.transform.position == machineController.GetTransCircle(-1).position){
                    cur_trans = machineController.GetTransCircle(-1);
                    gameController.playerCanMove = true;
                    anim.SetBool("Jump", false);
                    yourParticleSystem.Play();
                    audioSource.clip = AudioJump;
                    audioSource.Play();
                }
            }
        }
    }

    private void MachineRotation(Transform target){
        // Lấy vị trí của nhân vật và target
        Vector3 direction = target.position - transform.position;
        
        // Tính toán góc cần xoay (tính bằng radian)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Cập nhật hướng quay ngay lập tức (lưu ý trục Z)
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
    }
}
