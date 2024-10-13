using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject player;
    // UI
    UIController UI;
    //TextMeshProUGUI UI_Kill;
    //TextMeshProUGUI UI_Time;
    //TextMeshProUGUI UI_Subscript;
    //TextMeshProUGUI UI_Alert;
    //TextMeshProUGUI UI_HP;
    //[SerializeField] public Image UI_Weapon;
    //public Image UI_Dash;
    //public Image UI_Special;
    //public Sprite scytheSprite;
    //public Sprite swordSprite;

    AudioSource audioSource;

    public Spawner[] spawners;
    public GameObject boss;

    public int hp;
    public int currWave;
    public int killCount;

    public float timeLeft;
    public float alertTime;

    public enum GameState
    {
        Roam,
        Preparation,
        Battle,
        End
    }

    public GameState currGameState;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // UI
        UI = GetComponent<UIController>();
        //UI_Kill = GameObject.Find("Kill UI").GetComponent<TextMeshProUGUI>();
        //UI_Time = GameObject.Find("Time Left UI").GetComponent<TextMeshProUGUI>();
        //UI_Subscript = GameObject.Find("Subscript UI").GetComponent<TextMeshProUGUI>();
        //UI_Alert = GameObject.Find("Alert UI").GetComponent<TextMeshProUGUI>();
        //UI_HP = GameObject.Find("HP UI").GetComponent<TextMeshProUGUI>();

        // get spawners
        spawners = new Spawner[2];
        spawners[0] = GameObject.Find("Top Spawner").GetComponent<Spawner>();
        spawners[1] = GameObject.Find("Bottom Spawner").GetComponent<Spawner>();

        currWave = 0;
        killCount = 0;

        alertTime = 3f;
        timeLeft = 10f;
        currGameState = GameState.Roam;
    }

    void Update()
    {
        if (currGameState == GameState.Roam)
        {

        }
        else if (currGameState != GameState.End)
        {
            timeLeft -= Time.deltaTime;
            //UI_Time.text = ((int)timeLeft).ToString();
            UI.TimeLeft((int)timeLeft);

            // Change state once time is up
            if (timeLeft <= 0)
            {
                ChangeState();
            }

            // alert message UI
            alertTime -= Time.deltaTime;
            if (alertTime <= 0)
            {
                alertTime = 90f;
                UI.Alert("");
                //UI_Alert.text = "";
            }
        }

        // reload scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    public void IncreaseKillCount()
    {
        killCount++;
        UI.KillCount(killCount);
        //UI_Kill.text = "KILLS: " + killCount.ToString();
    }

    public void ChangeState()
    {
        // 4th wave is boss wave, completion means you win
        if (currWave == 4)
        {
            Debug.Log("YOU WIN");
            EndLevel("YOU WIN");
        }

        else if (currGameState == GameState.Preparation)
        {
            Debug.Log("BATTLE");
            StartWave();
        }

        else if (currGameState == GameState.Battle)
        {
            Debug.Log("END WAVE");
            EndWave();
        }
    }

    public void StartWave()
    {
        currGameState = GameState.Battle;
        currWave++;
        string sc = "SURVIVE WAVE " + currWave.ToString();
        string alert = "WAVE " + currWave.ToString() + " START";

        if (currWave == 1)
        {
            Debug.Log("WAVE 1");
            timeLeft = 35f;
            spawners[0].Activate(timeLeft);
            spawners[1].Activate(timeLeft);
            spawners[1].ChangeEnemyType(0);
        }
        else if (currWave == 2)
        {
            Debug.Log("WAVE 2");
            timeLeft = 45f;
            spawners[0].Activate(timeLeft);
            spawners[1].Activate(timeLeft);
            spawners[1].ChangeEnemyType(1);
        }
        else if (currWave == 3)
        {
            Debug.Log("WAVE 3");
            timeLeft = 60f;
            spawners[0].Activate(timeLeft);
            spawners[1].Activate(timeLeft);
            spawners[0].ChangeSpawnDelay(1.5f, 3.0f);
        }
        else if (currWave == 4)
        {
            timeLeft = 60f;
            spawners[0].Activate(timeLeft);
            boss.SetActive(true);
            sc = "KILL THE BOSS";
            alert = "FINAL WAVE START";
            //UI_Subscript.text = "KILL THE BOSS";
            //UI_Alert.text = "FINAL WAVE START";
            alertTime = 3f;
            return;
        }

        // UI text changes
        UI.Alert(alert);
        UI.Subscript(sc);
        //UI_Subscript.text = "SURVIVE WAVE " + currWave.ToString();
        //UI_Alert.text = "WAVE " + currWave.ToString() + " START";
        alertTime = 3f;
    }

    public void EndWave()
    {
        //UI_Subscript.text = "TIME TO NEXT WAVE";
        //UI_Alert.text = "WAVE " + currWave.ToString() + " CLEARED";
        UI.Alert("WAVE " + currWave.ToString() + " CLEARED");
        UI.Subscript("TIME TO NEXT WAVE");

        alertTime = 3f;

        currGameState = GameState.Preparation;
        timeLeft = 15f;
    }

    public void EndLevel(string alert)
    {
        //    UI_Time.text = "";
        //    UI_Subscript.text = "";
        //    UI_Alert.text = alert;

        UI.TimeLeft(-1);
        UI.Alert(alert);
        UI.Subscript("");

        currGameState = GameState.End;
        // do stuff 
    }

    public void LoseHP()
    {
        //UI_HP.text = "HP: " + hp.ToString();
        UI.HP(hp);
        if (hp <= 0)
        {
            EndLevel("YOU LOSE");
        }
    }

    // UI changes
    public void StartBattle()
    {
        currGameState = GameState.Preparation;
        audioSource.Play();
    }

}

