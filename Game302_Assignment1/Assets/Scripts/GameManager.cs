using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawningPoints;
    public GameObject enemy;
    public GameObject missile;

    public Text scoreText;
    private int playerScore = 0;


    AudioSource audioSource;
    public AudioClip launchSound;
    public AudioClip destroySound;

    private static GameManager instance_;

    public static GameManager Instance
    {
        get
        {
            return instance_;
        }
    }


    private void Awake()
    {
        if (instance_ != null && instance_ != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance_ = this;
        }
    }
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RespawnMissile()
    {
        GameObject tempMissile = Instantiate(missile, spawningPoints[0].transform.position, spawningPoints[0].transform.rotation);
    }

    public void RespawnEnemy()
    {
        GameObject tempEnemy = Instantiate(enemy, spawningPoints[1].transform.position, spawningPoints[1].transform.rotation);
    }

    public void AddScore()
    {
        playerScore++;
        scoreText.text = playerScore.ToString();
    }

    public void PlayMissileLaunchSound()
    {
        audioSource.clip = launchSound;
        audioSource.Play();
    }

    public void PlayMissileDestroySound()
    {
        audioSource.clip = destroySound;
        audioSource.Play();
    }
}
