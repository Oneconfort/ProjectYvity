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
    [SerializeField] private Sprite _spriteInteragivel;
    [SerializeField] private Image _spriteT;




    public void Setup(Sprite sprite, string text, string dscTxt, bool ativado)
    {
        gameObject.SetActive(true);
        _text.text = text;
        _dscTexto = dscTxt;
        btn.interactable = ativado;
        if (ativado)
        {
            _sprite.sprite = _spriteInteragivel; // Use o sprite interagível
        }
        else
        {
            _sprite.sprite = sprite; // Use o sprite padrão
        }

    }

    public void AtualizarTextoDsc(Text texto)
    {
        texto.text = _dscTexto;
    }
    public void AtualizarSprite(Sprite novoSprite)
    {
        _spriteT.sprite = novoSprite;
    }
}
