using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        rowButtons = new List<Button>();
        columnButtons = new List<Button>();
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
                //CheckDiagonal(ButtonNo);
                CheckRowCompleted(ButtonNo);
                CheckColumnCompleted(ButtonNo);
            }
            else if (player2Turn)
            {
                GameObject spawnedGO = (GameObject)Instantiate(SecondGO, ButtonList[ButtonNo].transform);
                spawnedGO.transform.position = new Vector2(ButtonList[ButtonNo].transform.position.x, ButtonList[ButtonNo].transform.position.y + 0.5f);
                spawnedGO.transform.localScale = new Vector2(0.16f, 0.16f);
                player1Turn = true;
                player2Turn = false;
                //CheckDiagonal(ButtonNo);
                CheckRowCompleted(ButtonNo);
                CheckColumnCompleted(ButtonNo);
            }
        }
        else
        {
            Debug.Log("Already has val");
        }
    }

    //private void CheckDiagonal(int clickedButton)
    //{
    //    if(clickedButton == 0 || clickedButton == 2 || clickedButton == 4 || clickedButton == 6 || clickedButton == 8)
    //    {
    //        for (int i = 0; i < 2; i++)
    //        {

    //        }
    //    }
    //}
    private void CheckRowCompleted(int clickedButton)
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
    private void CheckColumnCompleted(int clickedButton)
    {
        int c1XCount = 0, c2XCount = 0, c3XCount = 0, c1YCount = 0, c2YCount = 0, c3YCount = 0;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < columnButtons.Count; j += 3)
            {
                Debug.Log("CCC" + c1XCount + " " + c2XCount + " " + c3XCount + " " + c1YCount + " " + c2YCount + " " + c3YCount);
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
