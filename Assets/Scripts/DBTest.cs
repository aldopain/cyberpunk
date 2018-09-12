using UnityEngine;
using System.Collections;

public class DBTest : MonoBehaviour {
    public static string dbName = "test.db";
    public static string tableName = "test";

    private void Start() {
        exampleTwo ();
    }

    // insertAll - readAll - deleteAll - readAll
    void exampleOne () {
        InsertAllExample ();
        Debug.Log ("All was inserted");
        ReadExample (); 
        DeleteAllExample ();
        Debug.Log ("All was deleted, if you see after that something like data, then script works not properly");
        ReadExample ();
    }

    void exampleTwo () {
        InsertAllExample ();
        Debug.Log ("All was inserted");
        ReadExample ();
        DeleteExample ("testF1", "41");
        Debug.Log ("Now you should see only 51 52 53");
        ReadExample ();
        DeleteExample ("testF2", "52");
        Debug.Log ("All was deleted, if you see after that something like data, then script works not properly");
        ReadExample ();
    }

    void InsertAllExample () {
        var dbm = new DatabaseManager (dbName);
        dbm.OpenConnection ();
        var data = new ArrayList ();
        var buf = new ArrayList ();
        buf.Add (41); buf.Add (42); buf.Add (43);
        data.Add (buf);
        buf = new ArrayList ();
        buf.Add (51); buf.Add (52); buf.Add (53);
        data.Add (buf);
        dbm.InsertAll (tableName, data);
        dbm.CloseConnection ();
    }

    void InsertExample () {
        var dbm = new DatabaseManager (dbName);
        dbm.OpenConnection ();
        var data = new ArrayList ();
        data.Add (31); data.Add (32); data.Add (33);
        dbm.Insert (tableName, data);
        dbm.CloseConnection ();
    }

    //read from db example
    void ReadExample () {
        var dbm = new DatabaseManager (dbName);
        dbm.OpenConnection ();
        var a = dbm.Read (tableName, DatabaseManager.ALL_COLLUMNS);
        foreach (var item in a) {
            var arr = item as ArrayList;
            string row = "";
            foreach (var i in arr)
                row += i + "   ";
            Debug.Log (row);
        }
        dbm.CloseConnection ();
    }

    void DeleteAllExample () {
        var dbm = new DatabaseManager (dbName);
        dbm.OpenConnection ();
        dbm.DeleteAll (tableName);
        dbm.CloseConnection ();
    }

    void DeleteExample (string key, string value) {
        var dbm = new DatabaseManager (dbName);
        dbm.OpenConnection ();
        dbm.Delete (tableName, key, value);
        dbm.CloseConnection ();
    }
}