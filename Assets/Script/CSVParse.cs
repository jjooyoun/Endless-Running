﻿// This code automatically generated by TableCodeGen
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class CSVParse : MonoBehaviour 
{
	public TextAsset file;

	void Start() {
		file = Setting.gameSetting.spawnCsvFile;
		Load (file);
	}

	public class Row
	{
		public int[] lanes; 
	}

	public List<Row> rowList = new List<Row>();
	public bool isLoaded = false;

	public bool IsLoaded()
	{
		return isLoaded;
	}

	public List<Row> GetRowList()
	{
		return rowList;
	}

	public void Load(TextAsset csv)
	{
		if(!csv)
			return;
		rowList.Clear();
		string[][] grid = CsvParser2.Parse(csv.text);
		// Debug.Log ("grid.length:" + grid.Length);
		int spawnerNum = 0;
		for(int i = 1 ; i < grid.Length; i++)
		{
			// Debug.Log ("row index:" + i);
			Row row = new Row();
			row.lanes = new int[grid [i].Length];
			//read a row
			for(int j = 0; j < row.lanes.Length; j++){
				//Debug.Log ("grid[" + i + "][" + j + "]:" + grid[i][j]   );
				//Debug.Log ("j:" + j + ":" + );
				row.lanes [j] = Convert.ToInt32 (grid [i] [j]);
				if (row.lanes[j] != -1) {
					spawnerNum += 1;
				}
			}
			rowList.Add(row);
		}
		// Debug.Log ("row length:" + rowList.Count);
		isLoaded = true;
		EventManager.Instance.spawningNumEvent.Invoke(spawnerNum);
	}

	public int NumRows()
	{
		return rowList.Count;
	}

	public Row GetAt(int i)
	{
		if(rowList.Count <= i)
			return null;
		return rowList[i];
	}


}