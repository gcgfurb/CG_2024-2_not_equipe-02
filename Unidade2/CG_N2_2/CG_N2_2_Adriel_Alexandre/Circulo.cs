#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private int numPontos = 72; // Número de pontos a serem desenhados
        private double raio; // Raio do círculo

        public Circulo(Objeto _paiRef, ref char _rotulo, double raio) : this(_paiRef, ref _rotulo, new Ponto4D(0, 0), raio) { }

        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro, double raio) : base(_paiRef, ref _rotulo)
        {
            this.raio = raio;
            PrimitivaTipo = PrimitiveType.Points; // Vamos desenhar apenas pontos
            PrimitivaTamanho = 10; // Tamanho do ponto
            GerarPontos(centro);
        }

        // Método que gera 72 pontos na circunferência do círculo
        private void GerarPontos(Ponto4D centro)
        {
            double anguloIncremento = 360.0 / numPontos; // Incremento do ângulo para cada ponto

            for (int i = 0; i < numPontos; i++)
            {
                double angulo = i * anguloIncremento; // Calcula o ângulo atual
                double radianos = MathHelper.DegreesToRadians(angulo); // Converte para radianos

                // Calcula as coordenadas do ponto na circunferência
                double x = centro.X + raio * Math.Cos(radianos);
                double y = centro.Y + raio * Math.Sin(radianos);

                // Cria um ponto na circunferência com tamanho 10
                char rotulo = ' '; // Defina o rótulo conforme necessário
                Ponto pontoFinal = new Ponto(this, ref rotulo, new Ponto4D(x, y, centro.Z, centro.W))
                {
                    PrimitivaTipo = PrimitiveType.Points,
                    PrimitivaTamanho = 5 // Define o tamanho do ponto
                };
            }

            Atualizar();
        }

        private void Atualizar()
        {
            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif
    }
}
