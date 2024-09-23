using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public int ordemBotao;
    public int indicePlataforma;
    public bool ativado = false;
    private Renderer rendererBotao;
    private PuzzleManager gerenciadorDePuzzle;

    void Start()
    {
        rendererBotao = GetComponent<Renderer>();
        gerenciadorDePuzzle = FindObjectOfType<PuzzleManager>();
        AtualizarAparenciaDoBotao();
    }

    public void AlternarBotao()
    {
        if (!ativado)
        {
            ativado = true;
            gerenciadorDePuzzle.BotaoPressionado(ordemBotao, this, indicePlataforma);
            AtualizarAparenciaDoBotao();
        }
    }

    public void DesativarBotao()
    {
        ativado = false;
        AtualizarAparenciaDoBotao();
    }

    public void AtivarEEsperar()
    {
        StartCoroutine(AtivarEDesativarCoroutine());
    }

    private IEnumerator AtivarEDesativarCoroutine()
    {
        yield return new WaitForSeconds(1f);
        DesativarBotao();
    }

    private void AtualizarAparenciaDoBotao()
    {
        if (ativado)
        {
            rendererBotao.material.color = Color.green; 
        }
        else
        {
            rendererBotao.material.color = Color.red;
        }
    }
}