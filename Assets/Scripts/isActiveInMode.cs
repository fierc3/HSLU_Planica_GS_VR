using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isActiveInMode : MonoBehaviour
{
    private Globals globals;
    // Start is called before the first frame update
    void Start()
    {
       globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();   
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Canvas>().enabled = Mode.Interactive == globals.Mode;
    }
}
