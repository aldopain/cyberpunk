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
		string conn = "URI=file:" + Application.dataPath + "/" + dbName;
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
		IDbCommand dbcmd = dbconn.CreateCommand ();
		dbcmd.CommandText = sqlQuery;
	}

	private ArrayList Read () {
		ArrayList result = new ArrayList<ArrayList>();
		IDataReader reader = dbcmd.ExecuteReader ();
		while (reader.Read ()) {
			ArrayList buf = new ArrayList();
			reader.GetValues (buf);
			result.AddAll (buf);
		}
		reader.Close ();
		reader = null;
		return result;
	}

	public ArrayList Read (string tableName, string collumns) {
		CreateCommand ("SELECT " + collumns + " FROM " + tableName);
		var result = Read ();
		DisposeCommand ();
	}
}