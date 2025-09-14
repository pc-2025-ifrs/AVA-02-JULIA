using System;

namespace FracaoApp
{
    public class Fracao : IEquatable<Fracao>, IComparable<Fracao>
    {
        public int Numerador { get; private set; }
        public int Denominador { get; private set; }

        // Construtor principal (int, int)
        public Fracao(int numerador, int denominador)
        {
            ArgumentOutOfRangeException.ThrowIfEqual(denominador, 0, nameof(denominador));
            
            Numerador = numerador;
            Denominador = denominador;
            Simplificar();
        }

        // Construtor para número inteiro
        public Fracao(int numero) : this(numero, 1)
        {
        }

        // Construtor para string "numerador/denominador"
        public Fracao(string fracao)
        {
            if (string.IsNullOrWhiteSpace(fracao))
                throw new ArgumentException("String não pode ser nula ou vazia", nameof(fracao));

            var partes = fracao.Split('/');
            if (partes.Length != 2)
                throw new ArgumentException("Formato inválido. Use 'numerador/denominador'", nameof(fracao));

            if (!int.TryParse(partes[0], out int numerador))
                throw new ArgumentException("Numerador inválido", nameof(fracao));

            if (!int.TryParse(partes[1], out int denominador))
                throw new ArgumentException("Denominador inválido", nameof(fracao));

            ArgumentOutOfRangeException.ThrowIfEqual(denominador, 0, nameof(denominador));

            Numerador = numerador;
            Denominador = denominador;
            Simplificar();
        }

        // Construtor para decimal/double
        public Fracao(double numero)
        {
            if (double.IsInfinity(numero) || double.IsNaN(numero))
                throw new ArgumentException("Número deve ser finito", nameof(numero));

            // Converter decimal para fração
            var fracaoRacional = ConverterDecimalParaFracao(numero);
            Numerador = fracaoRacional.numerador;
            Denominador = fracaoRacional.denominador;
            Simplificar();
        }

        // Método auxiliar para converter decimal em fração
        private static (int numerador, int denominador) ConverterDecimalParaFracao(double numero)
        {
            if (numero == 0) return (0, 1);
            
            bool negativo = numero < 0;
            numero = Math.Abs(numero);

            // Para números como 0.345, queremos converter precisamente
            const double tolerancia = 1e-15;
            int denominador = 1;
            double temp = numero;

            // Encontrar o denominador apropriado multiplicando por potências de 10
            while (Math.Abs(temp - Math.Round(temp)) > tolerancia && denominador <= 1000000)
            {
                denominador *= 10;
                temp = numero * denominador;
            }

            int numerador = (int)Math.Round(temp);
            
            // Se ainda não conseguimos uma representação exata, usar aproximação por frações contínuas
            if (Math.Abs(temp - numerador) > tolerancia)
            {
                return AproximarPorFracoesContinuas(numero);
            }

            if (negativo) numerador = -numerador;

            return (numerador, denominador);
        }

        // Método auxiliar para aproximação por frações contínuas (algoritmo mais preciso)
        private static (int numerador, int denominador) AproximarPorFracoesContinuas(double numero)
        {
            bool negativo = numero < 0;
            numero = Math.Abs(numero);

            const double tolerancia = 1e-15;
            const int maxIteracoes = 20;

            int a = (int)numero;
            if (Math.Abs(numero - a) < tolerancia)
                return negativo ? (-a, 1) : (a, 1);

            int p0 = 1, q0 = 0;
            int p1 = a, q1 = 1;

            double r = numero - a;
            
            for (int i = 0; i < maxIteracoes && Math.Abs(r) > tolerancia; i++)
            {
                r = 1.0 / r;
                a = (int)r;
                
                int p2 = a * p1 + p0;
                int q2 = a * q1 + q0;
                
                if (Math.Abs(numero - (double)p2 / q2) < tolerancia)
                {
                    return negativo ? (-p2, q2) : (p2, q2);
                }
                
                p0 = p1; p1 = p2;
                q0 = q1; q1 = q2;
                r = r - a;
            }

            return negativo ? (-p1, q1) : (p1, q1);
        }

        // Método para simplificar a fração usando MDC
        private void Simplificar()
        {
            if (Denominador < 0)
            {
                Numerador = -Numerador;
                Denominador = -Denominador;
            }

            int mdc = CalcularMDC(Math.Abs(Numerador), Math.Abs(Denominador));
            Numerador /= mdc;
            Denominador /= mdc;
        }

        // Calcular o Máximo Divisor Comum
        private static int CalcularMDC(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        // Calcular o Mínimo Múltiplo Comum
        private static int CalcularMMC(int a, int b)
        {
            return Math.Abs(a * b) / CalcularMDC(a, b);
        }

        // Propriedades de consulta
        public bool IsPropria => Math.Abs(Numerador) < Math.Abs(Denominador);
        public bool IsImpropria => Math.Abs(Numerador) >= Math.Abs(Denominador) && Denominador != 1;
        public bool IsAparente => Numerador % Denominador == 0;
        public bool IsUnitaria => Math.Abs(Numerador) == 1;

        // Método Somar
        public Fracao Somar(int numero)
        {
            return this + new Fracao(numero);
        }

        public Fracao Somar(double numero)
        {
            return this + new Fracao(numero);
        }

        public Fracao Somar(string fracao)
        {
            return this + new Fracao(fracao);
        }

        public Fracao Somar(Fracao outra)
        {
            return this + outra;
        }

        // Operadores aritméticos
        public static Fracao operator +(Fracao f1, Fracao f2)
        {
            int mmc = CalcularMMC(f1.Denominador, f2.Denominador);
            int numerador = (f1.Numerador * mmc / f1.Denominador) + (f2.Numerador * mmc / f2.Denominador);
            return new Fracao(numerador, mmc);
        }

        public static Fracao operator +(Fracao f, int numero)
        {
            return f + new Fracao(numero);
        }

        public static Fracao operator +(Fracao f, double numero)
        {
            return f + new Fracao(numero);
        }

        public static Fracao operator +(Fracao f, string fracaoStr)
        {
            return f + new Fracao(fracaoStr);
        }

        public static Fracao operator -(Fracao f1, Fracao f2)
        {
            int mmc = CalcularMMC(f1.Denominador, f2.Denominador);
            int numerador = (f1.Numerador * mmc / f1.Denominador) - (f2.Numerador * mmc / f2.Denominador);
            return new Fracao(numerador, mmc);
        }

        public static Fracao operator *(Fracao f1, Fracao f2)
        {
            return new Fracao(f1.Numerador * f2.Numerador, f1.Denominador * f2.Denominador);
        }

        public static Fracao operator /(Fracao f1, Fracao f2)
        {
            return new Fracao(f1.Numerador * f2.Denominador, f1.Denominador * f2.Numerador);
        }

        // Operadores de comparação
        public static bool operator ==(Fracao f1, Fracao f2)
        {
            if (ReferenceEquals(f1, f2)) return true;
            if (f1 is null || f2 is null) return false;
            return f1.Numerador == f2.Numerador && f1.Denominador == f2.Denominador;
        }

        public static bool operator !=(Fracao f1, Fracao f2)
        {
            return !(f1 == f2);
        }

        public static bool operator <(Fracao f1, Fracao f2)
        {
            if (f1 is null || f2 is null) return false;
            int mmc = CalcularMMC(f1.Denominador, f2.Denominador);
            int num1 = f1.Numerador * mmc / f1.Denominador;
            int num2 = f2.Numerador * mmc / f2.Denominador;
            return num1 < num2;
        }

        public static bool operator >(Fracao f1, Fracao f2)
        {
            if (f1 is null || f2 is null) return false;
            return f2 < f1;
        }

        public static bool operator <=(Fracao f1, Fracao f2)
        {
            return f1 < f2 || f1 == f2;
        }

        public static bool operator >=(Fracao f1, Fracao f2)
        {
            return f1 > f2 || f1 == f2;
        }

        // Implementação de IEquatable<Fracao>
        public bool Equals(Fracao? other)
        {
            if (other is null) return false;
            return this.Numerador == other.Numerador && this.Denominador == other.Denominador;
        }

        // Override de Equals
        public override bool Equals(object? obj)
        {
            return Equals(obj as Fracao);
        }

        // Override de GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Numerador, Denominador);
        }

        // Implementação de IComparable<Fracao>
        public int CompareTo(Fracao? other)
        {
            if (other is null) return 1;
            if (this < other) return -1;
            if (this > other) return 1;
            return 0;
        }

        // Override de ToString
        public override string ToString()
        {
            return $"{Numerador}/{Denominador}";
        }
    }
}