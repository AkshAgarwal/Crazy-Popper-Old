using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour
{
    public GameObject bluePrefab, yellowPrefab, purplePrefab;
    [SerializeField] int column;
    [SerializeField] TextAsset textAsset;
    public GameObject parentHolder;
    public void LevelMap(int levelNo)
    {
        textAsset = (Resources.Load("Map/Level" + levelNo)) as TextAsset;
        string[] map = textAsset.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int iLine = 0;

        try
        {// Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            if (map.Length > 0)
            {
                string line;
                column = 0;
                // Read and display lines from the file until the end of the file is reached.
                while (iLine <= map.Length)
                {
                    line = map[iLine];
                    if (line == "[numoftap]")
                    {
                        iLine++;
                        GameManager.instance.numOfTapInLevel = int.Parse(map[iLine]);
                        iLine++;
                        line = map[iLine]; //Type
                        if (line == "type=Tile Layer 1")
                        {
                            iLine++;
                            bool ended = false;
                            while (!ended)
                            {
                                iLine++;
                                line = map[iLine];
                                //check if it is the last line
                                if (line.EndsWith("."))
                                {
                                    line = line.Substring(0, line.Length - 1);
                                    ended = true;
                                }
                                string[] casillas = line.Split(',');

                                int row = 0;

                                foreach (string _aux in casillas)
                                {
                                    if (_aux != "0" && _aux != "")
                                    {
                                        if (_aux == "1")
                                        {
                                            GameObject grid = Instantiate(purplePrefab) as GameObject;
                                            grid.transform.parent = parentHolder.transform;
                                            grid.transform.position = new Vector3(row, column, 0f);
                                        }
                                        else if (_aux == "2")
                                        {
                                            GameObject grid = Instantiate(bluePrefab) as GameObject;
                                            grid.transform.parent = parentHolder.transform;
                                            grid.transform.position = new Vector3(row, column, 0f);
                                        }
                                        else if (_aux == "3")
                                        {
                                            GameObject grid = Instantiate(yellowPrefab) as GameObject;
                                            grid.transform.parent = parentHolder.transform;
                                            grid.transform.position = new Vector3(row, column, 0f);
                                        }
                                        GameManager.instance.totalPlayerInLevel++;
                                    }
                                    row++;
                                }
                                column++;
                            }
                        }
                    }
                    iLine++;
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        parentHolder.transform.position = new Vector2(-3f, -3f);
    }
}
