using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class LevelManager : MonoBehaviour
{

    //UI Specific Variables
    [Header("UI Specific Variables")]
    public int turretIndex;
    public int creditCount, portalHealth, waveLevel;
    public Text creditsText, portalText, waveText;
    public Button[] turretButtons;
    public int[] costValue;
    public Text[] costText;
    public bool fastForward;

    //Building Specific Variables
    [Header("Building Specific Variables")]
    public GameObject[] turretPrefabs;
    public GameObject checkmark;
    private GameObject tile;
    public LayerMask placementLayer;
    public bool canBuild;
    public AudioClip turretDrop;
    private AudioSource aud;


    //Wave Specific Variables
    [Header("Wave Specific Variables")]
    public GameObject portalOrange;
    public GameObject[] enemyPrefabs;
    public Transform spawnPoint;
    public float spawnTime = 2f;
    public float startSpawn = 5f;
    public int enemyCount;
    public int enemyPerWave = 7;
    public float waveLength = 30f, currentWave = 30f;
    public Image levelBar;
    public bool startTimer;

    //Game Over Specific Variables
    [Header("Game Over Specific Variables")]
    public Transform portal;
    public GameObject portalExplosion;
    public AudioClip gameOver;
    public GameObject gameOverDisplay;

    //Audio Variables
    GameObject exceptionObj;
    List<AudioSource> allAudioSources = new List<AudioSource>();





    // Use this for initialization
    void Start()
    {
        exceptionObj = gameOverDisplay;


        Time.timeScale = 1;
        turretIndex = 0;

        costText[0].text = costValue[0].ToString();
        costText[1].text = costValue[1].ToString();
        costText[2].text = costValue[2].ToString();


        gameOverDisplay.SetActive(false);
        portalOrange.SetActive(false);

        aud = GetComponent<AudioSource>();

        EnableAllExceptOne();
        //AudioListener.pause = false;

        StartCoroutine(SpawnWaves());


    }

    // Update is called once per frame
    void Update()
    {

        UIDisplay();
        CheckTurretCosts();
        TurretPlacement();
        SpawnTimer();

    }

    public void FastForwardGame()
    {

        fastForward = !fastForward;

        if (fastForward)
            Time.timeScale = 3;
        else
            Time.timeScale = 1;

    }


    public void TurretSelection(int turretButton)
    {
        turretIndex = turretButton;
        canBuild = true;

    }



    void UIDisplay()
    {
        creditsText.text = creditCount.ToString();
        portalText.text = portalHealth.ToString();
        waveText.text = waveLevel.ToString();
        levelBar.fillAmount = currentWave / waveLength;


    }


    void CheckTurretCosts()
    {

        for (int i = 0; i < costText.Length; i++)
        {

            if (costValue[i] > creditCount)
            {
                costText[i].color = Color.red;
                turretButtons[i].enabled = false;


            }
            else
            {
                costText[i].color = Color.green; //set cost text to green color
                turretButtons[i].enabled = true;

            }
        }
    }


    void TurretPayment()
    {

        if (creditCount >= costValue[turretIndex])
        {
            creditCount -= costValue[turretIndex];
            Instantiate(turretPrefabs[turretIndex], tile.transform.position, Quaternion.identity);
            tile.SetActive(false);
            canBuild = false;
            aud.PlayOneShot(turretDrop);
        }

    }



    void TurretPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, placementLayer) && canBuild)
        {
            tile = hit.collider.gameObject;
            //Debug.Log (hit.collider.gameObject.name + " tile selected");
            checkmark.transform.position = tile.transform.position;
            checkmark.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                TurretPayment();
            }
        }
        else
        {

            checkmark.SetActive(false);

        }

    }


    void SpawnTimer()
    {

        if (currentWave > 0 && startTimer)
            currentWave -= Time.deltaTime;

    }


    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startSpawn);
        startTimer = true;


        while (startTimer)
        {
            for (int i = 0; i < enemyPerWave; i++)
            {
                int randomEnemy = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[randomEnemy], spawnPoint.position, spawnPoint.rotation);
                portalOrange.SetActive(true);
                enemyCount++;
                spawnTime = Random.Range(2f, 5f);
                yield return new WaitForSeconds(spawnTime);
                portalOrange.SetActive(false);

            }

            yield return new WaitForSeconds(currentWave);
            currentWave = waveLength;
            waveLevel++;
            creditCount += waveLevel * 10;


        }


    }


    public void EnemyPortal(int damage)
    {
        portalHealth -= damage;

        if (portalHealth <= 0)
        {
            Instantiate(portalExplosion, portal.position, Quaternion.identity);
            Destroy(portal.gameObject);
            Invoke("GameOver", 3f);
        }
    }

    void GameOver()
    {
        gameOverDisplay.SetActive(true);
        aud.PlayOneShot(gameOver);

        DisableAllExceptOne();
        //AudioListener.pause = true;

        Time.timeScale = 0;
    }




    void DisableAllExceptOne()
    {
        allAudioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.gameObject != exceptionObj)
            {
                audioSource.enabled = false;
            }
        }
    }

    void EnableAllExceptOne()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.gameObject != exceptionObj)
            {
                audioSource.enabled = true;
            }
        }
    }

}
