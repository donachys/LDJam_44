using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroProgress : MonoBehaviour {

    public Text textbox;

    void Update () {

    }

    public void Proceed() {
            Application.LoadLevel (1);
    }
}