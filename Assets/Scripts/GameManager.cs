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

    private bool player1Turn = false;
    private bool player2Turn = false;

    private bool gameWon = false;

    private List<Button> rowButtons;
    private List<Button> columnButtons;
    private List<Button> diagonalButtons;

    // Start is called before the first frame update
    void Start()
    {
        rowButtons = new List<Button>();
        columnButtons = new List<Button>();
        diagonalButtons = new List<Button>();
        Won.SetActive(false);

        player1Turn = true;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpwanGO(int ButtonNo)
    {
        Debug.Log("Clicked");
        int childCount = ButtonList[ButtonNo].transform.childCount;
        if(childCount == 0)
        {
            if (player1Turn)
            {
                GameObject spawnedGO = (GameObject) Instantiate(FirstGO, ButtonList[ButtonNo].transform);
                spawnedGO.transform.position = new Vector2(ButtonList[ButtonNo].transform.position.x, ButtonList[ButtonNo].transform.position.y + 0.5f);
                spawnedGO.transform.localScale = new Vector2(0.16f, 0.16f);
                player1Turn = false;
                player2Turn = true;
                CheckDiagonal();
                CheckRowCompleted();
                CheckColumnCompleted();
            }
            else if (player2Turn)
            {
                GameObject spawnedGO = (GameObject)Instantiate(SecondGO, ButtonList[ButtonNo].transform);
                spawnedGO.transform.position = new Vector2(ButtonList[ButtonNo].transform.position.x, ButtonList[ButtonNo].transform.position.y + 0.5f);
                spawnedGO.transform.localScale = new Vector2(0.16f, 0.16f);
                player1Turn = true;
                player2Turn = false;
                CheckDiagonal();
                CheckRowCompleted();
                CheckColumnCompleted();
            }
        }
        else
        {
            Debug.Log("Already has val");
        }
    }

    private void CheckDiagonal()
    {
        int d1XCount = 0, d1YCount = 0;
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < diagonalButtons.Count; j++)
            {
                int rowAndColumn = i + j;
                Debug.Log("DDD" + d1XCount + " " + d1YCount);
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
                    Debug.Log("No Child Found");
                }
                
                //if(diagonalButtons[i + k].transform.childCount > 0)
                //{
                //    if (diagonalButtons[i + k].transform.GetChild(0).transform.CompareTag("Set1"))
                //    {
                //        d2XCount++;
                //    }
                //    else if (diagonalButtons[i + k].transform.GetChild(0).transform.CompareTag("Set2"))
                //    {
                //        d2YCount++;
                //    }

                //    if (d2XCount == 3 || d2YCount == 3)
                //    {
                //        GameWon();
                //    }
                //}
            }
        }
    }
    private void CheckRowCompleted()
    {
        int r1XCount = 0, r2XCount = 0, r3XCount = 0, r1YCount = 0, r2YCount = 0, r3YCount = 0;

        Debug.Log("row" + rowButtons.Count);
        for(int i = 0; i < rowButtons.Count; i += 3)
        {
            for(int j=0; j < 3; j++)
            {
                int rowAndColumn = i + j;
                if (rowButtons[rowAndColumn].transform.childCount > 0)
                {
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
                    Debug.Log("No Child Found");
                }
            }
        }
    }    
    private void CheckColumnCompleted()
    {
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
                    Debug.Log("No Child Found");
                }
            }
        }
    }

    private void GameWon()
    {
        Debug.Log("Game WOn");
        Won.SetActive(true);
    }
}
