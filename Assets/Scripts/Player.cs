using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CircleController playerController;
    [SerializeField] private GameController gameController;
    [SerializeField] private float speedPlayer;
    
    private Transform cur_trans;
    private Animator anim;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        // this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -3f);

        gameController.playerCanMove = false;
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2f);

        // this.gameObject.SetActive(true);
        gameController.playerCanMove = true;
        cur_trans = playerController.GetTransCircle(-3);
        audioSource.Play();
        this.transform.position = cur_trans.position;
        // this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.playerCanMove) return;
        
        if (cur_trans.position != playerController.GetTransCircle(-3).position){
            PlayerRotation(playerController.GetTransCircle(-3));
            gameController.isFly = true;
            anim.SetBool("Jump", true);
            this.transform.position = Vector3.MoveTowards(this.transform.position, playerController.GetTransCircle(-3).position, speedPlayer * Time.deltaTime);
            if (this.transform.position == playerController.GetTransCircle(-3).position){
                cur_trans = playerController.GetTransCircle(-3);
                gameController.machineCanMove = true;
                gameController.playerCanMove = true;
                gameController.isFly = false;
                anim.SetBool("Jump", false);
                audioSource.Play();
            }
        }
    }

    private void PlayerRotation(Transform target){
        // Lấy vị trí của nhân vật và target
        Vector3 direction = target.position - transform.position;
        
        // Tính toán góc cần xoay (tính bằng radian)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Cập nhật hướng quay ngay lập tức (lưu ý trục Z)
        transform.rotation = Quaternion.Euler(0, 0, angle-270);
    }
}
