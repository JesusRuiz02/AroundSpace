using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Instructionss;
    void Start()
    {
        StartCoroutine("FirstInstruction");
    }

    private IEnumerator FirstInstruction()
    {
        Instructionss.text = "Hemos aterrizado";
        yield return new WaitForSeconds(3);
        Instructionss.text = "Necesitamos suministros";
        yield return new WaitForSeconds(3);
        Instructionss.text = "Avanza hacia a los arboles rosas y recoge sus frutos";
        yield return new WaitForSeconds(5f);
        Instructionss.text = "Ubiación exacta, Noreste de la nave";
        yield return new WaitForSeconds(5f);
        Instructionss.text = "Suerte.";
        yield return new WaitForSeconds(3f);
        Instructionss.enabled = false;
    }

    public IEnumerator LastInstructions()
    {
        Instructionss.text = "Se detectó un objeto que podria funcionarnos como combustible";
        Instructionss.enabled = true;
        yield return new WaitForSeconds(5f);
        Instructionss.text = "Ubicación exacta, Sureste de la nave en una cueva";
        yield return new WaitForSeconds(5f);
        Instructionss.text = "Tómala y vamonos de aquí.";
        yield return new WaitForSeconds(3f);
        Instructionss.enabled = false;

    }

}
