using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPainelConquista : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private Text _text;
    [SerializeField] private string _dscTexto;
    [SerializeField] private Button btn;




    public void Setup(Sprite sprite, string text, string dscTxt, bool ativado)
    {
        gameObject.SetActive(true);
        _sprite.sprite = sprite;
        _text.text = text;
        _dscTexto = dscTxt;
        btn.interactable = ativado;

    }

    public void AtualizarTextoDsc(Text texto)
    {
        texto.text = _dscTexto;
    }
}
