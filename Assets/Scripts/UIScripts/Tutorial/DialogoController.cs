using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogoController : MonoBehaviour
{
    public GameObject painelDialogo;
    public Image image;
    public Text texto;
    public Dialogo dialogo;

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tutorial"))
        {
            painelDialogo.SetActive(true);
            image.sprite = dialogo.sprite;
            texto.text = dialogo.texto;
            dialogo = dialogo.proxDialogo;
            Destroy(other.gameObject);
            Invoke("FimTutorial", 6);
        }
    }
    void FimTutorial()
    {
        painelDialogo.SetActive(false);
    }
}
