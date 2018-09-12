using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;

//i have not even tried this shit
public class DatabaseManager {
	public static string SELECT_ALL_COLLUMNS = "*";
	private IDbConnection dbconn;
	private string dbName;
	private IDbCommand dbcmd;

	public DatabaseManager (string name) {
		dbName = name;
	}

	public void OpenConnection () {
		string conn = "URI=file:" + Application.dataPath + "/Databases/" + dbName;
		Debug.Log(conn);
		dbconn = (IDbConnection) new SqliteConnection (conn);
		dbconn.Open ();
	}

	public void CloseConnection () {
		dbconn.Close ();
		dbconn = null;
	}

	private void DisposeCommand () {
		dbcmd.Dispose ();
		dbcmd = null;
	}

	private void CreateCommand (string sqlQuery) {
		dbcmd = dbconn.CreateCommand ();
		dbcmd.CommandText = sqlQuery;
	}

	private ArrayList Read () {
		ArrayList result = new ArrayList();
		IDataReader reader = dbcmd.ExecuteReader ();
		while (reader.Read ()) {
			object[] bufArr = new object[5];
			reader.GetValues (bufArr);
			result.Add (new ArrayList(bufArr));
		}
		reader.Close ();
		reader = null;
		Debug.Log (result.Count);
		return result;
	}

	public ArrayList Read (string tableName, string collumns) {
		CreateCommand ("SELECT " + collumns + " FROM " + tableName);
		var result = Read ();
		DisposeCommand ();
		return result;
	}
}