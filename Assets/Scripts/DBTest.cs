using UnityEngine;
using System.Collections;

public class DBTest : MonoBehaviour {
    private void Start() {
        var dbm = new DatabaseManager ("test.db");
        dbm.OpenConnection ();
        var a = dbm.Read ("test", DatabaseManager.SELECT_ALL_COLLUMNS);
        foreach (var item in a) {
            var arr = item as ArrayList;
            foreach (var i in arr)
                Debug.Log("v " + i);
        }
        dbm.CloseConnection ();
    }
}