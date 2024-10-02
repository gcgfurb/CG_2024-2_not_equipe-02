using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Spline : Objeto
    {
        private int numLinhas;
        private List<Ponto4D> pontosPoligono;
        private List<Ponto4D> pontosSpline; // Armazena os pontos da spline calculada
        private int pontoAtualIndex = 0;
        private List<SegReta> linhasControle = new List<SegReta>(); // Lista de linhas de controle
        private List<Ponto> pontosControle = new List<Ponto>(); // Lista de pontos de controle

        Shader _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
        Shader _shaderBranco = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
        Shader _shaderVermelho = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");

        public Spline(Objeto _paiRef, ref char _rotulo, List<Ponto4D> pontosPoligono, int numLinhas) : base(_paiRef, ref _rotulo)
        {
            if (pontosPoligono.Count < 4)
            {
                throw new InvalidOperationException("Número de pontos de controle deve ser pelo menos 4.");
            }
            if (numLinhas <= 0)
            {
                throw new ArgumentOutOfRangeException("numLinhas deve ser maior que zero.");
            }

            PrimitivaTipo = PrimitiveType.LineStrip;
            PrimitivaTamanho = 1;
            this.pontosPoligono = pontosPoligono;
            this.numLinhas = numLinhas;

            GerarPontosControle();
            GerarLinhasControle();
            GerarSpline(); // Calcula os pontos da spline na inicialização
            Atualizar();
        }

        public void Atualizar()
        {
            base.ObjetoAtualizar();
        }

        private void GerarPontosControle()
        {
            pontosControle.Clear(); // Limpar antes de gerar novos pontos

            for (int i = 0; i < pontosPoligono.Count; i++)
            {
                Ponto4D pontoControle = pontosPoligono[i];

                char rotulo = (char)('P' + i);
                Ponto ponto = new Ponto(this, ref rotulo, pontoControle);
                ponto.PrimitivaTamanho = 20;
                pontosControle.Add(ponto); // Adiciona o ponto à lista
            }

            // Após gerar todos os pontos, ajuste a cor do ponto atual
            if (pontosControle.Count > 0)
            {
                // Ajusta a cor apenas se o índice estiver dentro dos limites
                if (pontoAtualIndex >= 0 && pontoAtualIndex < pontosControle.Count)
                {
                    pontosControle[pontoAtualIndex].ShaderObjeto = _shaderVermelho;
                }
            }
        }

        private void GerarLinhasControle()
        {
            if (pontosPoligono.Count < 4) return; // Verifica se existem pelo menos 4 pontos de controle

            char rotulo = 'L'; // Rótulo inicial para as linhas de controle

            // Itera pelos pontos de controle para criar as linhas
            for (int i = 0; i < pontosPoligono.Count - 1; i++)
            {
                Ponto4D pontoIni = pontosPoligono[i];
                Ponto4D pontoFim = pontosPoligono[i + 1];

                SegReta linha = new SegReta(this, ref rotulo, pontoIni, pontoFim);
                linha.ShaderObjeto = _shaderCiano;
                linhasControle.Add(linha); // Adiciona a linha à lista
            }
        }

        private void GerarSpline()
        {
            // Gera os pontos da spline (Bézier) e armazena em pontosSpline
            pontosSpline = GerarPontosBezier(pontosPoligono, numLinhas);

            // Adiciona os pontos calculados à lista de pontos do objeto para renderização
            foreach (var ponto in pontosSpline)
            {
                base.PontosAdicionar(ponto);
            }
        }

        public void AtualizarSpline(Ponto4D ptoInc, bool proximo)
        {
            // Verifica se há pontos de controle suficientes para alterar
            if (pontosPoligono.Count == 0)
                return;

            // Atualiza o índice do ponto atual ou próximo
            if (proximo)
            {
                pontoAtualIndex = (pontoAtualIndex + 1) % pontosPoligono.Count;
            }

            // Atualiza o ponto na lista de pontos de controle
            if (pontoAtualIndex < pontosPoligono.Count)
            {
                Console.WriteLine("Ponto atual: " + pontoAtualIndex);
                pontosPoligono[pontoAtualIndex].X += ptoInc.X;
                pontosPoligono[pontoAtualIndex].Y += ptoInc.Y;
                pontosPoligono[pontoAtualIndex].Z += ptoInc.Z;

                // Limpa os pontos da spline e de controle antes de regenerar
                LimparPontosEControles();

                // Recalcula os pontos da spline
                GerarSpline();

                // Recalcula os pontos e linhas de controle
                GerarPontosControle();
                GerarLinhasControle();

                // Atualiza o objeto para renderizar novamente
                Atualizar();
            }
        }

        private void LimparPontosEControles()
        {
            // Limpa os pontos anteriores da spline
            base.PontosApagar();

            // Limpa as linhas de controle anteriores
            foreach (var linha in linhasControle)
            {
                linha.PontosApagar();
            }
            linhasControle.Clear(); // Limpa a lista de linhas de controle

            // Limpa os pontos de controle anteriores
            foreach (var ponto in pontosControle)
            {
                ponto.PontosApagar();
            }
            pontosControle.Clear(); // Limpa a lista de pontos de controle
        }

        private List<Ponto4D> GerarPontosBezier(List<Ponto4D> pontosControle, int numPontos)
        {
            List<Ponto4D> pontosSpline = new List<Ponto4D>();
            for (int i = 0; i < numPontos; i++)
            {
                float t = i / (float)(numPontos - 1);
                Ponto4D ponto = CalcularBezier(pontosControle, t);
                pontosSpline.Add(ponto);
            }
            return pontosSpline;
        }

        public void AumentarPontosSpline()
        {
            numLinhas++; // Aumenta o número de linhas
            AtualizarSplineComNovoNumeroDePontos();
        }

        public void DiminuirPontosSpline()
        {
            if (numLinhas > 1) // Certifique-se de que o número de linhas não fique abaixo de 1
            {
                numLinhas--; // Diminui o número de linhas
                AtualizarSplineComNovoNumeroDePontos();
            }
        }

        private void AtualizarSplineComNovoNumeroDePontos()
        {
            LimparPontosEControles(); // Limpa pontos e controles anteriores
            GerarSpline(); // Gera a spline com o novo número de pontos
            GerarPontosControle(); // Regenera os pontos de controle
            GerarLinhasControle(); // Regenera as linhas de controle
            Atualizar(); // Atualiza o objeto para renderização
        }

        public static Ponto4D MultiplicarPorEscalar(float escalar, Ponto4D ponto)
        {
            return new Ponto4D(
                ponto.X * escalar,
                ponto.Y * escalar,
                ponto.Z * escalar,
                ponto.W
            );
        }

        private Ponto4D CalcularBezier(List<Ponto4D> pontosControle, float t)
        {
            int n = pontosControle.Count;

            List<Ponto4D> pontos = new List<Ponto4D>(pontosControle);
            for (int k = 1; k < n; k++)
            {
                for (int i = 0; i < n - k; i++)
                {
                    pontos[i] = MultiplicarPorEscalar(1 - t, pontos[i]) + MultiplicarPorEscalar(t, pontos[i + 1]);
                }
            }

            return pontos[0];
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Spline (Bézier) _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif

    }
}
