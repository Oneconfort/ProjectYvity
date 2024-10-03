using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecanismo : MonoBehaviour
{
    public GameObject plataforma;
    public float velocidade = 2.0f;
    public int[] ordemCorreta;
    public List<ButtonScript> alavancasAtivadas = new List<ButtonScript>();
    public int passoAtual = 0;
    public bool plataformaAtiva = false;
    public Vector3 posicaoInicial;
    public Vector3 posicaoFinal;
    public bool movendoParaPosicaoFinal = true;
    public int tipo;

}
