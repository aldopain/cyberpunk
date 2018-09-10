using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;

public class DatabaseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string conn = "URI=file:" + Application.dataPath + "/PickAndPlaceDatabase.s3db"; //Path to database.
		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		
		//string sqlQuery = <SQL QUERY>;
		//dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read())
		{
				//add here reading
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;
	}
}
