using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ItemVideogame : MonoBehaviour
{
    private string playerChoice;
    private string computerChoice;
    private int tie;
    public GameObject _player;
    public GameObject _computer;
    public TextMeshProUGUI _textMesh;
    public GameObject _rock;

    public GameObject _paper;

    public GameObject _scissors;
    private GameObject clone1;

    private GameObject clone2;
    void Start()
    {
        _textMesh.text = "J is Rock,K is Paper,L is Scissors ";
        StartGame();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            playerChoice = "Rock";
            clone1 = Instantiate(_rock, _player.transform);
            StartCoroutine(PlayerTurn());
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            playerChoice = "Paper";
            clone1 = Instantiate(_paper, _player.transform);
            StartCoroutine(PlayerTurn());
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            playerChoice = "Scissors";
            clone1 = Instantiate(_scissors, _player.transform);
            StartCoroutine(PlayerTurn());
        }

    }
    void StartGame()
    {

    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(2f);
        ComputerTurn();
        yield return new WaitForSeconds(1f);
        DetermineWinner();

    }

    void ComputerTurn()
    {
        int randomChoice = Random.Range(0, 3);
        switch (randomChoice)
        {
            case 0:
                computerChoice = "Rock";
                clone2 = Instantiate(_rock, _computer.transform);
                break;
            case 1:
                computerChoice = "Paper";
                clone2 = Instantiate(_paper, _computer.transform);
                break;
            case 2:
                computerChoice = "Scissors";
                clone2 = Instantiate(_scissors, _computer.transform);
                break;
        }
    }

    void DetermineWinner()
    {
        if (playerChoice == computerChoice)
        {
            tie++;
            _textMesh.text = "Graw,choose again";
            Destroy(clone1);
            Destroy(clone2);
            if (tie >= 3)
            {
                _textMesh.text = "You win";
            }
        }
        else if ((playerChoice == "Rock" && computerChoice == "Scissors") ||
                 (playerChoice == "Paper" && computerChoice == "Rock") ||
                 (playerChoice == "Scissors" && computerChoice == "Paper"))
        {
            GetComponent<ItemController>().AddItem();
            _textMesh.text = "You Win";
        }
        else
        {
            
            _textMesh.text = "You loose";
        }
        GetComponent<ItemController>().DestroyItem(gameObject);
    }
  
}
