using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainelConquistas : MonoBehaviour
{
    public List<Sprite> spriteConquistas;
    public List<ItemPainelConquista> itensPainel;

    private void Start()
    {
        ResetarPainel();
        AtualizarPainel();
    }
    private void OnEnable()
    {
        AtualizarPainel();
    }
    private void ResetarPainel()
    {
        for (int i = 0; i < itensPainel.Count; i++)
        {
            itensPainel[i].gameObject.SetActive(false);
        }
    }

    public void AtualizarPainel()
    {
        List<Conquista> conquistas = GerenciadorDeConquistas.instance.conquistas;
        for (int i = 0; i < conquistas.Count; i++)
        {
            Sprite sprite = spriteConquistas[conquistas[i].codIcone];
            itensPainel[i].Setup(sprite, conquistas[i].nome, conquistas[i].dscConquista, conquistas[i].conquistaAtivada);
            itensPainel[i].gameObject.SetActive(true);
        }
    }
}
