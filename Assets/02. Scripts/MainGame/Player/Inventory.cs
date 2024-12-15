using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Text essenceUI;
    [SerializeField] Text kitUI;
    [SerializeField] Text ammunition762UI;
    [SerializeField] Text ammunition45UI;
    [SerializeField] Text ammunition12UI;

    // ½Àµæ ÇöÈ² count
    private int essence;
    private int kit;
    private int ammunition762;
    private int ammunition45;
    private int ammunition12;

    public void UpdateEssence(int n)
    {
        essence += n;
        essenceUI.text = essence + " / 10";
    }

    public void UpdateKit(int n)
    {
        kit += n;
        kitUI.text = " * " + kit;
    }

    public void Update762(int n)
    {
        ammunition762 += n;
        ammunition762UI.text = " * " + ammunition762;
    }

    public void Update45(int n)
    {
        ammunition45 += n;
        ammunition45UI.text = " * " + ammunition45;
    }

    public void Update12(int n)
    {
        ammunition12 += n;
        ammunition12UI.text = " * " + ammunition12;
    }
}
