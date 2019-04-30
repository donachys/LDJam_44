using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
	public static GameData gameData;

	public static Hashtable Globals;

	void Awake () {
		if (gameData == null) {
			DontDestroyOnLoad (gameObject);
			gameData = this;
            Globals = new Hashtable();
            Globals["level"] = 0;
		} else if (gameData != this) {
			Destroy (gameObject);
		}
        _setTaxVals();
        _setBebeVals();
        _setEnemyVals();
        _setGenVals();
	}
    public int GetLevel()
    {
        return (int)Globals["level"];
    }
    public static void IncrementLevel()
    {
        Globals["level"] = (int)Globals["level"]+1;
    }

    private void _setGenVals()
    {
        Globals["countDownTime"] = 50.0f + (GetLevel() * 2.0f);
    }
    private void _setBebeVals()
    {
        Globals["bebeValue"] = Mathf.Max(1, 25.0f - GetLevel());
    }
    private void _setEnemyVals()
    {
        Globals["maxMoneyToSteal"] = 5.0f + (GetLevel() * 2.0f);
    }
    private void _setTaxVals()
    {
        Globals["baseRate"] = 10.0f + GetLevel();
        Globals["taxRate"] = 0.30f;
        Globals["taxMod"] = 1.0f + (GetLevel() / 20.0f);
        Globals["startingWorth"] = 100.0f;
    }

	void Start() {

	}
}