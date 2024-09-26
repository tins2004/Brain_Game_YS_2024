using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private CircleController circleController;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject passPanel;
    private float timeElapsed;

    public bool isShowNumber { get; set; }
    public bool isClick { get; set; }
    public bool isFly { get; set; }
    public bool playerCanMove { get; set; }
    public bool machineCanMove { get; set; }

    private AudioSource audioSource;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform YSS, YYS;
    private bool isYS;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        isYS = false;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(passPanel.active) playerCanMove = false;

        // Debug.Log(playerCanMove + "; Machine " + machineCanMove);
        levelText.text = "Level: " + circleController.GetLevel().ToString();
        
        // Tăng thời gian đã trôi qua
        timeElapsed += Time.deltaTime;

        // Chuyển đổi thời gian thành phút và giây
        int minutes = Mathf.FloorToInt(timeElapsed / 60F);
        int seconds = Mathf.FloorToInt(timeElapsed % 60F);

        // Hiển thị thời gian dưới định dạng "mm:ss"
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (isYS){
            YSS.position = Vector3.MoveTowards(YSS.position, new Vector3(0f,0f,0f), 10 * Time.deltaTime);
            YYS.position = Vector3.MoveTowards(YYS.position, new Vector3(0f,0f,0f), 10 * Time.deltaTime);
        }
        else{
            if (YSS.position.x != 11f && YYS.position.x != -11f ){
            YSS.position = Vector3.MoveTowards(YSS.position, new Vector3(11f,0f,0f), 10 * Time.deltaTime);
            YYS.position = Vector3.MoveTowards(YYS.position, new Vector3(-11f,0f,0f), 10 * Time.deltaTime);
            }
            if (YSS.position.x >= 8f || YYS.position.x >= 8f) canvas.SetActive(true);
        }
        
    }

    private void ShowNumber(){
        audioSource.Play();
        isShowNumber = true;
    }

    public void BackMenu(){
        audioSource.Play();
        canvas.SetActive(false);
        isYS = true;
        StartCoroutine(BackToMenu());
    }

    public void InputPass(string pass){
        if (pass != "pass") return;

        passPanel.SetActive(false);
        ShowNumber();
        playerCanMove = true;
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Menu");
    }
}
