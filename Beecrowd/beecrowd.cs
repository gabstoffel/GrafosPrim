using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public class Program
{
    public class aresta
    {
        public int nodoPartida;
        public int nodoChegada;
        public int peso;

        public void incializaAresta(int partida, int chegada, int peso)
        {
            this.nodoPartida = partida;
            this.nodoChegada = chegada;
            this.peso = peso;
        }
    }
    public class Graph
    {


        public List<int> vertices;
        public List<List<int>> listaDeAdjanecias;
        public List<List<int>> pesos;
        public int custoTotal;

        public void inicializaListaDeAdjancencia(int[][] input, int m)
        {

            this.vertices = new List<int>();
            this.listaDeAdjanecias = new List<List<int>>();
            this.pesos = new List<List<int>>();


            for (int ii = 0; ii < input.Length; ii++)
            {
                if (!vertices.Contains(input[ii][0]))
                    this.vertices.Add(input[ii][0]);

                if (!vertices.Contains(input[ii][1]))
                    this.vertices.Add(input[ii][1]);

                vertices.Sort();
            }

            for (int j = 0; j < vertices.Count; j++)
            {
                List<int> list = new List<int>();
                this.listaDeAdjanecias.Add(list);

                var listaPesos = new List<int>();
                this.pesos.Add(listaPesos);

            }

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Length < 3)
                {

                    continue;
                }

                var verticePartida = input[i][0];
                var verticeChegada = input[i][1];
                var peso = input[i][2];

                if (verticePartida >= 0 && verticePartida < vertices.Count &&
                    verticeChegada >= 0 && verticeChegada < vertices.Count)
                {
                    if (verticeChegada != verticePartida)
                    {
                        listaDeAdjanecias[verticePartida].Add(verticeChegada);
                        listaDeAdjanecias[verticeChegada].Add(verticePartida);

                        pesos[verticePartida].Add(peso);
                        pesos[verticeChegada].Add(peso);
                    }
                }
            }

            int custo = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Length == 3)
                {
                    custo += input[i][2];
                }
            }

            this.custoTotal = custo;
        }

    }
    public class Matriz
    {
        public int m;
        public int n;
        public int linhaAtual;
        public int[][] matrizProblema;

        public void inicializaMatriz(int paramM, int paramN)
        {
            this.matrizProblema = new int[paramN][];
            this.m = paramM;
            this.n = paramN;
            this.linhaAtual = 0;

            for (int i = 0; i < paramN; i++)
            {
                this.matrizProblema[i] = new int[3];
            }
        }
        public void inicializaLinha()
        {
            this.linhaAtual++;
        }
    }
    static int Main(string[] args)
    {
        Matriz matriz = new Matriz();
        int m = 0;
        int n = 0;
        int counter_ = 0;
        bool primeirasVariaveis = true;
        while (true)
        {

            string[] dimensoes = Console.ReadLine().Split(' ');

            m = int.Parse(dimensoes[0]);
            n = int.Parse(dimensoes[1]);


            if (primeirasVariaveis)
            {
                matriz.inicializaMatriz(m, n);
                primeirasVariaveis = false;
                continue;
            }
            if ((m == 0 && n == 0) || counter_ == matriz.n)
            {
                break;
            }

            counter_++;
            matriz.inicializaLinha();

            int x = int.Parse(dimensoes[0]);
            int y = int.Parse(dimensoes[1]);
            int z = int.Parse(dimensoes[2]);

            matriz.matrizProblema[matriz.linhaAtual - 1][0] = x;
            matriz.matrizProblema[matriz.linhaAtual - 1][1] = y;
            matriz.matrizProblema[matriz.linhaAtual - 1][2] = z;

        }


        int custoFinal = algoritmoPrim(matriz.m, matriz.n, matriz.matrizProblema);
        return custoFinal;
    }




    static int algoritmoPrim(int m, int n, int[][] input)
    {

        Graph grafo = new Graph();
        grafo.inicializaListaDeAdjancencia(input, m);

        int verticeAtual = grafo.vertices[0];

        List<int> adjancenciasVericeAtual = grafo.listaDeAdjanecias[0];
        List<int> pesosVerticeAtual = grafo.pesos[0];


        bool[] verticesVisitados = new bool[m];

        verticesVisitados[verticeAtual] = true;


        int custo = 0;


        heap GraphHeap = new heap();
        GraphHeap.inicializaHeap();


        int counter = 0;
        foreach (var proximoVertice in grafo.listaDeAdjanecias[0])
        {
            aresta arestaAtual = new aresta();
            arestaAtual.incializaAresta(verticeAtual, proximoVertice, grafo.pesos[verticeAtual][counter]);
            counter++;

            GraphHeap.insereElemento(arestaAtual);
        }

        while (GraphHeap.size > 0)
        {


            aresta menorArestaRetirada = GraphHeap.retiraElemento();

            int verticeDeChegada = menorArestaRetirada.nodoChegada;

            if (verticesVisitados[verticeDeChegada])
                continue;

            verticesVisitados[verticeDeChegada] = true;


            custo += menorArestaRetirada.peso;

            List<int> listaDeAdjacenciaProximoVertice = grafo.listaDeAdjanecias[verticeDeChegada];

            int outroCounter = 0;
            foreach (var proximoVertice in listaDeAdjacenciaProximoVertice)
            {
                aresta arestaProximoNodo = new aresta();
                arestaProximoNodo.incializaAresta(verticeDeChegada, proximoVertice, grafo.pesos[verticeDeChegada][outroCounter]);
                outroCounter++;

                GraphHeap.insereElemento(arestaProximoNodo);
            }
        }

        int custoTotal = grafo.custoTotal;
        return custoTotal - custo;


    }


    public class heap
    {
        public int size;

        public List<aresta> elementosDoGrafo;
        public void inicializaHeap()
        {
            elementosDoGrafo = new List<aresta>();
            size = 0;

        }

        public void insereElemento(aresta aresta_)
        {
            elementosDoGrafo.Add(aresta_);

            size++;

            heapfyUp();
        }

        public aresta retiraElemento()
        {
            aresta minElement = new aresta();
            if (elementosDoGrafo.Count > 0)
            {
                minElement = elementosDoGrafo[0];
                reorganizaElementosHeap();

            }

            return minElement;
        }

        public void reorganizaElementosHeap()
        {

            int indexUltimoElemento = elementosDoGrafo.Count - 1;

            elementosDoGrafo[0] = elementosDoGrafo[indexUltimoElemento];
            elementosDoGrafo.RemoveAt(indexUltimoElemento);

            size += -1;

            heapfyDown();

        }
        public void heapfyDown()
        {

            int index = 0;
            int indexLeftChild = index * 2 + 1;
            int indexRightChild = index * 2 + 2;

            if (!(indexLeftChild < elementosDoGrafo.Count))
            {
                return;
            }

            while ((indexLeftChild < elementosDoGrafo.Count))
            {
                int smallerChildIndex = indexLeftChild;

                if (indexRightChild < elementosDoGrafo.Count)
                {
                    if (elementosDoGrafo[indexRightChild] != null && elementosDoGrafo[indexRightChild].peso < elementosDoGrafo[indexLeftChild].peso)
                    {

                        smallerChildIndex = indexRightChild;
                    }
                }
                if (elementosDoGrafo[index].peso < elementosDoGrafo[smallerChildIndex].peso)
                {
                    break;
                }
                else
                {
                    if (elementosDoGrafo[index].peso == elementosDoGrafo[smallerChildIndex].peso)
                    {
                        break;
                    }
                    aresta temp = elementosDoGrafo[smallerChildIndex];
                    elementosDoGrafo[smallerChildIndex] = elementosDoGrafo[index];
                    elementosDoGrafo[index] = temp;
                }
                index = smallerChildIndex;
                indexLeftChild = index * 2 + 1;
                indexRightChild = index * 2 + 2;
            }
        }

        public void heapfyUp()
        {
            int index = elementosDoGrafo.Count - 1;
            int indexPai = (index - 1) / 2;

            while ((indexPai >= 0) && (elementosDoGrafo[indexPai].peso > elementosDoGrafo[index].peso))
            {
                aresta temp = elementosDoGrafo[indexPai];
                elementosDoGrafo[indexPai] = elementosDoGrafo[index];
                elementosDoGrafo[index] = temp;

                index = indexPai;
                indexPai = (index - 1) / 2;
            }
        }
    }
}



