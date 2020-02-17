using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLocalization : MonoBehaviour
{
    void Start()
    {
        int sold = Random.Range(2000, 230123);

        Debug.Log("#MENU_SOLD".Localized(sold));
    }
}