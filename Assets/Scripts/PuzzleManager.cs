using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class puzzleManager
    {
        public GameObject plataforma;
        public float velocidade = 2.0f;
        public int[] ordemCorreta;
        public List<ButtonScript> botoesAtivados = new List<ButtonScript>();
        public int passoAtual = 0;
        public bool plataformaAtiva = false;
        public Vector3 posicaoInicial;
        public Vector3 posicaoFinal;
    }

    public puzzleManager[] plataformasPuzzle;

    void Start()
    {
        foreach (var puzzle in plataformasPuzzle)
        {
            puzzle.posicaoInicial = puzzle.plataforma.transform.position;
        }
    }
    private void Update()
    {
        Mover();
    }

    public void BotaoPressionado(int ordemDoBotao, ButtonScript scriptBotao, int indicePlataforma)
    {
        var puzzle = plataformasPuzzle[indicePlataforma];
        if (ordemDoBotao == puzzle.ordemCorreta[puzzle.passoAtual] && LanternaPlayer.lanternaPlayer.naPosicao)
        {
            puzzle.passoAtual++;
            puzzle.botoesAtivados.Add(scriptBotao);
            if (puzzle.passoAtual == puzzle.ordemCorreta.Length)
            {
                AtivarPlataforma(indicePlataforma);
            }
        }
        else
        {
            ResetarPuzzle(indicePlataforma);
            scriptBotao.AtivarEEsperar();
        }
    }

    private void AtivarPlataforma(int indicePlataforma)
    {
        var puzzle = plataformasPuzzle[indicePlataforma];
        puzzle.plataformaAtiva = true;
    }

    private void ResetarPuzzle(int indicePlataforma)
    {
        var puzzle = plataformasPuzzle[indicePlataforma];
        puzzle.passoAtual = 0;
        foreach (ButtonScript botao in puzzle.botoesAtivados)
        {
            botao.DesativarBotao();
        }
        puzzle.botoesAtivados.Clear();
    }

    private void ResetarTodosOsPuzzles()
    {
        foreach (var puzzle in plataformasPuzzle)
        {
            puzzle.passoAtual = 0;
            foreach (ButtonScript botao in puzzle.botoesAtivados)
            {
                botao.DesativarBotao();
            }
            puzzle.botoesAtivados.Clear();
            puzzle.plataforma.transform.position = Vector3.MoveTowards( puzzle.plataforma.transform.position,puzzle.posicaoInicial, puzzle.velocidade * Time.deltaTime );
        }
    }

    void Mover()
    {
        foreach (var puzzle in plataformasPuzzle)
        {
            if (puzzle.plataformaAtiva)
            {
                if (puzzle.plataformaAtiva)
                {
                    puzzle.plataforma.transform.position = Vector3.MoveTowards( puzzle.plataforma.transform.position,puzzle.posicaoFinal,puzzle.velocidade * Time.deltaTime);

                    if (Vector3.Distance(puzzle.plataforma.transform.position, puzzle.posicaoFinal) < 0.01f)
                    {
                        puzzle.plataformaAtiva = false;
                    }
                }
            }
        }

        if (!LanternaPlayer.lanternaPlayer.naPosicao)
        {
            ResetarTodosOsPuzzles();
        }

    }
}