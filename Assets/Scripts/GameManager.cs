using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public GameObject FirstGO;
    public GameObject SecondGO;
    public GameObject Won;

    public List<Button> ButtonList = new List<Button>();

    //private bool player1Turn = false;
    //private bool player2Turn = false;

    public int playerNumber; // 1 for Player 1 (host), 2 for Player 2 (client)
    public bool isFirstPlayerTurn;

    private List<Button> rowButtons;
    private List<Button> columnButtons;
    private List<Button> diagonalButtons;

    private GameObject player;

    private PhotonView photonView;

    void Start()
    {
        rowButtons = new List<Button>();
        columnButtons = new List<Button>();
        diagonalButtons = new List<Button>();
        Won.SetActive(false);

        photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            // The host player is always Player 1
            playerNumber = 1;
        }
        else
        {
            // The client player is always Player 2
            playerNumber = 2;
        }

        // Set the initial turn based on the player number
        isFirstPlayerTurn = (playerNumber == 1);

        rowButtons = ButtonList;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < ButtonList.Count; j += 3)
            {
                columnButtons.Add(ButtonList[i + j]);
            }
        }
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < ButtonList.Count; j += 4)
            {
                diagonalButtons.Add(ButtonList[i + j]);
            }
            for(int k = 2; k < ButtonList.Count -1; k += 2)
            {
                diagonalButtons.Add(ButtonList[i + k]);
            }
        }
    }

    public void SpawnGO(int ButtonNo)
    {
        Debug.Log("Clicked" + PhotonNetwork.IsMasterClient + " " + isFirstPlayerTurn);
        int childCount = ButtonList[ButtonNo].transform.childCount;
        Debug.Log("ChildCount" + ButtonList[ButtonNo].transform.childCount);
        if (childCount == 0)
        {
            if (playerNumber == 1 && isFirstPlayerTurn)
            {
                // Player 1 (host) logic
                GameObject spawnedGO = PhotonNetwork.Instantiate(FirstGO.name, ButtonList[ButtonNo].transform.position, Quaternion.identity);
                spawnedGO.transform.SetParent(ButtonList[ButtonNo].transform);
                //spawnedGO.transform.position = new Vector2(ButtonList[ButtonNo].transform.position.x, ButtonList[ButtonNo].transform.position.y + 0.5f);
                //spawnedGO.transform.localScale = new Vector2(0.16f, 0.16f);
                isFirstPlayerTurn = false;
            }
            else if (playerNumber == 2 && !isFirstPlayerTurn)
            {
                // Player 2 (client) logic
                GameObject spawnedGO = PhotonNetwork.Instantiate(SecondGO.name, ButtonList[ButtonNo].transform.position, Quaternion.identity);
                spawnedGO.transform.SetParent(ButtonList[ButtonNo].transform);
                //spawnedGO.transform.position = new Vector2(ButtonList[ButtonNo].transform.position.x, ButtonList[ButtonNo].transform.position.y + 0.5f);
                //spawnedGO.transform.localScale = new Vector2(0.16f, 0.16f);
                isFirstPlayerTurn = true;
            }

            // Perform other game logic here
            CheckDiagonal();
            CheckRowCompleted();
            CheckColumnCompleted();

            // Call the method to synchronize the isFirstPlayerTurn variable across the network
            photonView.RPC("SyncTurn", RpcTarget.All, isFirstPlayerTurn);

        }
        else
        {
            Debug.Log("Already has value");
        }
    }

    // Remote Procedure Call (RPC) method to synchronize the isFirstPlayerTurn variable across the network
    [PunRPC]
    private void SyncTurn(bool turn)
    {
        isFirstPlayerTurn = turn;
    }

    private void CheckDiagonal()
    {
        Debug.Log("Diagonal Called");
        int d1XCount = 0, d1YCount = 0;
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < diagonalButtons.Count; j++)
            {
                int rowAndColumn = i + j;
                //Debug.Log("DDD" + d1XCount + " " + d1YCount);
                if (diagonalButtons[rowAndColumn].transform.childCount > 0)
                {
                    if (diagonalButtons[rowAndColumn].transform.GetChild(0).transform.CompareTag("Set1"))
                    {
                        if(rowAndColumn < 3)
                        {
                            d1XCount++;
                        }
                    }
                    else if(diagonalButtons[rowAndColumn].transform.GetChild(0).transform.CompareTag("Set2"))
                    {
                        if(rowAndColumn < 3)
                        {
                            d1YCount++;
                        }
                    }

                    if (d1XCount == 3 || d1YCount == 3)
                    {
                        GameWon();
                    }
                }
                else
                {
                   //Debug.Log("No Child Found");
                }
                
            }
        }
    }
    private void CheckRowCompleted()
    {
        Debug.Log("Row Called");
        int r1XCount = 0, r2XCount = 0, r3XCount = 0, r1YCount = 0, r2YCount = 0, r3YCount = 0;

       // Debug.Log("row" + rowButtons.Count);
        for(int i = 0; i < rowButtons.Count; i += 3)
        {
            for(int j=0; j < 3; j++)
            {
                //Debug.Log("Inside row");
                int rowAndColumn = i + j;
                //Debug.Log("i +j" + (i + j));
                if (rowButtons[rowAndColumn].transform.childCount > 0)
                {
                   // Debug.Log("rrr" + r1XCount + " " + r2XCount + " " + r3XCount + " " + r1YCount + " " + r2YCount + " " + r3YCount + " " + rowButtons[rowAndColumn].transform.GetChild(0).transform.tag);
                    //Debug.Log("ii" + (i + j));
                    if (rowButtons[rowAndColumn].transform.GetChild(0).transform.CompareTag("Set1")){

                        if(rowAndColumn < 3)
                        {
                            r1XCount++;

                        }
                        else if(rowAndColumn < 6)
                        {
                            r2XCount++;
                        }
                        else if(rowAndColumn < 9)
                        {
                            r3XCount++;
                        }
                        //Debug.Log(xCount + " " + rowButtons[i + j].transform.GetChild(0).transform.tag);
                        if (r1XCount == 3 || r2XCount == 3 || r3XCount == 3)
                        {
                            Debug.Log("Wonn");
                            GameWon();
                        }
                    }
                    else if (rowButtons[rowAndColumn].transform.GetChild(0).transform.CompareTag("Set2"))
                    {
                        if (rowAndColumn < 3)
                        {
                            r1YCount++;

                        }
                        else if (rowAndColumn < 6)
                        {
                            r2YCount++;
                        }
                        else if (rowAndColumn < 9)
                        {
                            r3YCount++;
                        }
                        //Debug.Log(xCount + " " + rowButtons[i + j].transform.GetChild(0).transform.tag);
                        if (r1YCount == 3 || r2YCount == 3 || r3YCount == 3)
                        {
                            GameWon();
                        }
                    }
                }
                else
                {
                    //Debug.Log("No Child Found");
                }
            }
        }
    }    
    private void CheckColumnCompleted()
    {
        //Debug.Log("Column Called");
        int c1XCount = 0, c2XCount = 0, c3XCount = 0, c1YCount = 0, c2YCount = 0, c3YCount = 0;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < columnButtons.Count; j += 3)
            {
                //Debug.Log("CCC" + c1XCount + " " + c2XCount + " " + c3XCount + " " + c1YCount + " " + c2YCount + " " + c3YCount);
                int rowAndColumn = i + j;
                if (columnButtons[rowAndColumn].transform.childCount > 0)
                {
                    Debug.Log("rc" + rowAndColumn);
                    if (columnButtons[rowAndColumn].transform.GetChild(0).transform.CompareTag("Set1"))
                    {

                        if (rowAndColumn < 3)
                        {
                            c1XCount++;

                        }
                        else if (rowAndColumn < 6)
                        {
                            c2XCount++;
                        }
                        else if (rowAndColumn < 9)
                        {
                            c3XCount++;
                        }
                        //Debug.Log(xCount + " " + rowButtons[i + j].transform.GetChild(0).transform.tag);
                        if (c1XCount == 3 || c2XCount == 3 || c3XCount == 3)
                        {
                            GameWon();
                        }
                    }
                    else if (columnButtons[rowAndColumn].transform.GetChild(0).transform.CompareTag("Set2"))
                    {
                        if (rowAndColumn < 3)
                        {
                            c1YCount++;

                        }
                        else if (rowAndColumn < 6)
                        {
                            c2YCount++;
                        }
                        else if (rowAndColumn < 9)
                        {
                            c3YCount++;
                        }
                        //Debug.Log(xCount + " " + rowButtons[i + j].transform.GetChild(0).transform.tag);
                        if (c1YCount == 3 || c2YCount == 3 || c3YCount == 3)
                        {
                            GameWon();
                        }
                    }
                }
                else
                {
                   //Debug.Log("No Child Found");
                }
            }
        }
    }

    private void GameWon()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("HandleGameWon", RpcTarget.All);
        }
    }
    [PunRPC]
    private void HandleGameWon()
    {
        Debug.Log("Game Won");
        Won.SetActive(true);
    }
}
