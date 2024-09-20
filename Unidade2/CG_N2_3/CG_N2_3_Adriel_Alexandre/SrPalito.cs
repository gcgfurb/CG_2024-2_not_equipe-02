#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class SrPalito : Objeto
    {
        private double raio;
        private double angulo;
        private Ponto4D ptoFim;
        private Ponto4D ptoIni;

        private Ponto4D pontoAtualizado;

        public SrPalito(Objeto _paiRef, ref char _rotulo, double raio, double angulo) : this(_paiRef, ref _rotulo, new Ponto4D(0.0, 0.0), raio, angulo)
        {
        }

        public SrPalito(Objeto _paiRef, ref char _rotulo, Ponto4D ptoIni, double raio, double angulo) : base(_paiRef, ref _rotulo)
        {
            this.raio = raio;
            this.angulo = angulo;
            this.ptoIni = ptoIni;

            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 1;

            base.PontosAdicionar(ptoIni);
            // Gera o ponto final usando a função de cálculo de vetor
            var (ptoFim, _) = Matematica.GerarPontoVetor(ptoIni, raio, angulo);
            this.ptoFim = ptoFim;
            base.PontosAdicionar(ptoFim);

            Atualizar();
        }

        // Função para atualizar o ângulo e recalcular o ponto final
        // Função para atualizar o ângulo e recalcular o ponto final
        public void AtualizarAngulo(double novoAngulo)
        {
            this.angulo = novoAngulo;

            // Limpa todos os pontos antigos
            base.PontosApagar();

            // Verifica se pontoAtualizado foi inicializado, caso contrário, usa ptoIni
            if (pontoAtualizado == null)
            {
                pontoAtualizado = ptoIni;
            }

            // Adiciona o ponto inicial atualizado ou o ponto inicial original
            base.PontosAdicionar(pontoAtualizado);

            // Recalcula o novo ponto final com o novo ângulo e o ponto inicial atualizado
            var (novoPtoFim, _) = Matematica.GerarPontoVetor(pontoAtualizado, raio, novoAngulo);
            this.ptoFim = novoPtoFim;

            // Adiciona o novo ponto final
            base.PontosAdicionar(ptoFim);

            // Atualiza a geometria do objeto
            Atualizar();
        }



        // Função para atualizar o ponto inicial e recalcular o ponto final
        // Função para atualizar o eixo X do ponto inicial e recalcular o ponto final
        public void AtualizarPe(double pInc)
        {
            // Atualiza apenas o eixo X do ponto inicial, mantendo Y e Z inalterados

            // Limpa todos os pontos antigos
            base.PontosApagar();

            // Adiciona o ponto inicial atualizado com o novo X, preservando Y e Z
            pontoAtualizado = new Ponto4D(pInc, ptoIni.Y);
            base.PontosAdicionar(pontoAtualizado);

            // Recalcula o ponto final com base no novo ponto inicial, raio e ângulo atuais
            var (novoPtoFim, _) = Matematica.GerarPontoVetor(pontoAtualizado, raio, angulo);
            this.ptoFim = novoPtoFim;

            // Adiciona o novo ponto final
            base.PontosAdicionar(ptoFim);

            // Atualiza a geometria do objeto
            Atualizar();
        }

        // Método para atualizar o raio e recalcular o ponto final
        public void AtualizarRaio(double raioInc)
        {
            // Atualiza o valor do raio
            this.raio += raioInc;

            // Limpa todos os pontos antigos
            base.PontosApagar();

            // Verifica se pontoAtualizado foi inicializado, caso contrário, usa ptoIni
            if (pontoAtualizado == null)
            {
                pontoAtualizado = ptoIni;
            }

            // Adiciona o ponto inicial atualizado ou o ponto inicial original
            base.PontosAdicionar(pontoAtualizado);

            // Recalcula o novo ponto final com o novo raio e o ângulo atual
            var (novoPtoFim, _) = Matematica.GerarPontoVetor(pontoAtualizado, raio, angulo);
            this.ptoFim = novoPtoFim;

            // Adiciona o novo ponto final
            base.PontosAdicionar(ptoFim);

            // Atualiza a geometria do objeto
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
            retorno = "__ Objeto SrPalito _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif
    }
}
