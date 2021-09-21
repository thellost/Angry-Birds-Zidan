using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    public Button nextButton;
    public GameObject GameOverPanel;

    private Bird _shotBird;

    public BoxCollider2D TapCollider;

    private bool _isGameEnded = false;
    void Start()
    {
        nextButton.interactable = false;
        for (int i = 0; i < Birds.Count; i++)
        {
            //object game controller akan meng "subscribe" fungsi onbirddestroyed , dan menjalankan fungsi Change bird
            Birds[i].OnBirdDestroyed += ChangeBird;

            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }
        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    public void ChangeBird()
    {
        _shotBird = null;

        TapCollider.enabled = false;

        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
            return;
        }
        if (Enemies.Count > 0 && Birds.Count <= 0 && !_isGameEnded)
        {
            GameOverPanel.SetActive(true);

            _isGameEnded = true;
            return;
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }
        if(Enemies.Count > 0 && Birds.Count <= 0)
        {
            GameOverPanel.SetActive(true);

            _isGameEnded = true;
            return;
        }
        if (Enemies.Count == 0)
        {
            _isGameEnded = true;
            nextButton.interactable = true;
        }
    }
    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }
    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

}
