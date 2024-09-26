using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform YSS, YYS;
    private bool isYS;
    private AudioSource audioSource;


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

    public void PlayGame(){
        audioSource.Play();
        canvas.SetActive(false);
        isYS = true;
        StartCoroutine(ContinueGame());
    }

    IEnumerator ContinueGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Game");
    }
       
}
