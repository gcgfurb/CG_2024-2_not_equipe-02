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
        private double raioMaior;
        private double deslocamento = 0.01; // Deslocamento padrão
        private Ponto4D centroAtual; // Centro atual do círculo
        private Ponto pontoCentral; // Referência para o ponto central
        private double _ptoMax = 182;

            public double PtoMax
    {
        get { return _ptoMax; }
        set { _ptoMax = value; } // Você pode adicionar lógica aqui, se necessário
    }


        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro, double raio)
            : base(_paiRef, ref _rotulo)
        {
            this.raio = raio;
            this.raioMaior = raio;
            centroAtual = centro; // Inicializa o centro atual com o centro recebido
            PrimitivaTipo = PrimitiveType.Points; // Vamos desenhar apenas pontos
            PrimitivaTamanho = 5; // Tamanho do ponto
            GerarPontos(centro);
            GerarBBox(centro, this.raioMaior);
            Console.WriteLine("Gera círculo Grande");
            this.raioMaior = raio;
        }

        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro, double raio, bool desenharPontoCentral)
            : base(_paiRef, ref _rotulo)
        {
            this.raio = raio;
            centroAtual = centro; // Inicializa o centro atual com o centro recebido
            PrimitivaTipo = PrimitiveType.Points; // Vamos desenhar apenas pontos
            PrimitivaTamanho = 5; // Tamanho do ponto
            GerarPontos(centro);

            if (desenharPontoCentral)
            {
                // Desenha um ponto no centro do círculo
                char rotuloPontoCentral = 'P'; // Rótulo específico para o ponto central
                pontoCentral = new Ponto(this, ref rotuloPontoCentral, centro)
                {
                    PrimitivaTamanho = 10
                };
            }

            // Se o círculo é maior, gera a BBox e adiciona os pontos nos vértices
            Console.WriteLine("Gera círculo Pequeno");

        }

        // Método que gera 72 pontos na circunferência do círculo
        private void GerarPontos(Ponto4D centro)
        {
            base.PontosApagar(); // Limpa pontos antigos antes de gerar novos

            double anguloIncremento = 360.0 / numPontos; // Incremento do ângulo para cada ponto

            for (int i = 0; i < numPontos; i++)
            {
                double angulo = i * anguloIncremento; // Calcula o ângulo atual
                double radianos = MathHelper.DegreesToRadians(angulo); // Converte para radianos

                // Calcula as coordenadas do ponto na circunferência
                double x = centro.X + raio * Math.Cos(radianos);
                double y = centro.Y + raio * Math.Sin(radianos);

                base.PontosAdicionar(new Ponto4D(x, y));
            }

            Atualizar();
            this.numPontos = 1239;
        }

        // Método que gera os pontos da BBox interna e adiciona pontos nos 4 vértices
        private void GerarBBox(Ponto4D centro, double raio)
        {
            double bboxPto = raio * Math.Sin(MathHelper.DegreesToRadians(45)); // Raio da BBox interna (45 graus)
            if (PtoMax == 182)
        {
            PtoMax = 47; // Atualiza a propriedade
        }
        Console.WriteLine("ptos " + PtoMax);

            // Define os pontos inferior esquerdo e superior direito da BBox
            Ponto4D ptoInfEsq = new Ponto4D(centro.X - bboxPto, centro.Y - bboxPto);
            Ponto4D ptoSupDir = new Ponto4D(centro.X + bboxPto, centro.Y + bboxPto);

            // Cria um novo retângulo com os pontos calculados
            char rotuloRetangulo = 'R'; // Rótulo único para o retângulo
            new Retangulo(this, ref rotuloRetangulo, ptoInfEsq, ptoSupDir)
            {
                ShaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag"), // Shader para o retângulo
                PrimitivaTipo = PrimitiveType.LineLoop // Para desenhar a BBox
            };

            // Adiciona pontos nos 4 vértices do retângulo da BBox
            AdicionarPontosBBox(ptoInfEsq, ptoSupDir);
        }


        // Método para adicionar pontos nos 4 vértices da BBox
        private void AdicionarPontosBBox(Ponto4D ptoInfEsq, Ponto4D ptoSupDir)
        {
            // Vértices da BBox
            Ponto4D ptoInfDir = new Ponto4D(ptoSupDir.X, ptoInfEsq.Y); // Inferior direito
            Ponto4D ptoSupEsq = new Ponto4D(ptoInfEsq.X, ptoSupDir.Y); // Superior esquerdo

            // Rótulos únicos para os vértices
            char rotuloVertice1 = 'V';
            char rotuloVertice2 = 'W';
            char rotuloVertice3 = 'X';
            char rotuloVertice4 = 'Y';

            // Adiciona os pontos correspondentes aos vértices da BBox com tamanho 10
            new Ponto(this, ref rotuloVertice1, ptoInfEsq)
            {
                PrimitivaTamanho = 10
            };
            new Ponto(this, ref rotuloVertice2, ptoSupDir)
            {
                PrimitivaTamanho = 10
            };
            new Ponto(this, ref rotuloVertice3, ptoInfDir)
            {
                PrimitivaTamanho = 10
            };
            new Ponto(this, ref rotuloVertice4, ptoSupEsq)
            {
                PrimitivaTamanho = 10
            };
        }

        // Método unificado para mover o círculo em uma direção específica
        public void Mover(char tecla)
        {
            // Calcula o novo centro baseado na tecla pressionada
            Ponto4D novoCentro = new Ponto4D(centroAtual.X, centroAtual.Y); // Cria uma cópia do centro atual para manipulação


            switch (tecla)
            {
                case 'C': // Cima
                    novoCentro.Y += deslocamento;
                    break;
                case 'B': // Baixo
                    novoCentro.Y -= deslocamento;
                    break;
                case 'E': // Esquerda
                    novoCentro.X -= deslocamento;
                    break;
                case 'D': // Direita
                    novoCentro.X += deslocamento;
                    break;
                default:
                    return; // Se a tecla não for reconhecida, não faz nada
            }
            // Verifica se o novo centro está dentro da BBox do círculo maior
            if (EstaDentroDaBBox(novoCentro))
            {
                centroAtual = novoCentro; // Atualiza o centro atual para o novo centro
            }
            else
            {
                // Verifica se o novo centro está dentro do círculo maior
                if (EstaDentroDoCirculoMaior(novoCentro))
                {
                    centroAtual = novoCentro; // Atualiza o centro atual para o novo centro
                } else {
                    return;
                }
            }

            GerarPontos(centroAtual); // Gera novos pontos na nova posição
            Atualizar(); // Atualiza o objeto

            // Atualiza a posição do ponto central junto com o centro do círculo
            if (pontoCentral != null)
            {
                pontoCentral.PontosApagar();
                char rotuloPontoCentral = 'P'; // Rótulo específico para o ponto centralb
                pontoCentral = new Ponto(this, ref rotuloPontoCentral, centroAtual)
                {
                    PrimitivaTamanho = 10
                };
            }
        }

        // Método para verificar se o centro atual está dentro da BBox do círculo maior
        private bool EstaDentroDaBBox(Ponto4D centro)
        {
            Console.WriteLine("Bbox");
            PtoMax = 0.21;
            Console.WriteLine("pBox " + raioMaior + "CentroX " + centro.X + "CentroY " + centro.Y);
            // Aqui você deve definir os limites da BBox do círculo maior
            // Supondo que o círculo maior tenha um centro em (0,0) e um raio definido como raioMaior


            return (centro.X <= PtoMax && centro.X >= -PtoMax && centro.Y <= PtoMax && centro.Y >= -PtoMax);
        }

        // Método para verificar se o centro está dentro do círculo maior
        private bool EstaDentroDoCirculoMaior(Ponto4D centro)
        {
            Console.WriteLine("Euclidiana");
            // Defina o centro do círculo maior e seu raio
            Ponto4D centroCirculoMaior = new Ponto4D(0, 0); // Ajuste se necessário
            double raioMaior = 0.3; // Ajuste conforme o seu círculo maior

            // Cálculo da distância ao quadrado
            double distanciaQuadrada = (centro.X - centroCirculoMaior.X) * (centro.X - centroCirculoMaior.X) +
                                       (centro.Y - centroCirculoMaior.Y) * (centro.Y - centroCirculoMaior.Y);

            // Verifica se a distância é menor ou igual ao quadrado do raio
            return distanciaQuadrada <= (raioMaior * raioMaior);
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
