using FracaoApp;

// Teste 1: Construtor (int, int) com simplificação
var f1 = new Fracao(5, 10);
Console.WriteLine(f1.Numerador == 1); // True
Console.WriteLine(f1.Denominador == 2); // True
Console.WriteLine(f1.ToString() == "1/2"); // True
Console.WriteLine(f1); // 1/2

// Teste 2: Construtor (int)
var f2 = new Fracao(3); // 3 inteiros
Console.WriteLine(f2); // "3/1"

// Teste 3: Construtor (string)
var f3 = new Fracao("30/40"); // Necessário fazer o split e parse da string
Console.WriteLine(f3.Numerador == 3); // True
Console.WriteLine(f3.Denominador == 4); // True
Console.WriteLine(f3); // "3/4"

// Teste 4: Construtor (double)
var f4 = new Fracao(0.345); // https://www.wolframalpha.com/input/?i=rationalize+0.345
Console.WriteLine(f4); // "69/200"

var f5 = new Fracao(0.4);
Console.WriteLine(f5.Numerador == 2); // True
Console.WriteLine(f5.Denominador == 5); // True
Console.WriteLine(f5); // "2/5"

// Teste 5: Operações aritméticas
// https://www.wolframalpha.com/input?i=1%2F2+%2B+2%2F1
var f6 = f1.Somar(2); // 1/2 + 2/1
Console.WriteLine(f6); // "5/2"

var f7 = f1 + 2;
Console.WriteLine(f7); // "5/2"

var f8 = f1 + 0.5; // 1/2 + 1/2
Console.WriteLine(f8); // "1/1"

var f9 = f1 + 0.2862; // https://www.wolframalpha.com/input/?i=rationalize+0.2862
Console.WriteLine(f9); // Resultado baseado na conversão de 0.2862

var f10 = f3 + "7/8";
Console.WriteLine(f10); // 3/4 + 7/8 = 13/8

var f11 = f3 + 6.45;
Console.WriteLine(f11); // 3/4 + 6.45

// Teste 6: Operadores de comparação
Fracao f12 = new(1, 5);
Fracao f13 = new(1, 3);
Fracao f14 = new(125, 375); // Simplifica para 1/3
Fracao f15 = new(15, 75);   // Simplifica para 1/5

Console.WriteLine(f12.Equals(f14)); // false
Console.WriteLine(f12 == f14); // false
Console.WriteLine(f12 != f14); // true
Console.WriteLine(f12 == f15); // true
Console.WriteLine(f13 == f14); // true
Console.WriteLine(new Fracao("3/19").Equals(new Fracao(3, 19))); // true

// Teste 7: Operadores de comparação (<, >, <=, >=)
var f16 = new Fracao(2, 12); // 1/6
var f17 = new Fracao(3, 4);
var f18 = new Fracao(9, 10);
var f19 = new Fracao(5);
var f20 = new Fracao(24, 18); // 4/3
var f21 = new Fracao(16, 8);  // 2/1
var f22 = new Fracao(1, 8);
var f23 = new Fracao(10, 80); // 1/8

// Todas estas assertivas devem imprimir true
Console.WriteLine(f16 < f17); // 1/6 < 3/4
Console.WriteLine(f18 > f17); // 9/10 > 3/4
Console.WriteLine(f19 > f18); // 5 > 9/10
Console.WriteLine(f12 >= f15); // 1/5 >= 1/5
Console.WriteLine(f16 < f20); // 1/6 < 4/3

// Teste 8: Propriedades de consulta
Console.WriteLine(f16.IsImpropria); // False (1/6)
Console.WriteLine(f16.IsPropria); // True (1/6)
Console.WriteLine(f20.IsImpropria); // True (4/3)
Console.WriteLine(f20.IsAparente); // False (4/3 não é número inteiro)
Console.WriteLine(f21.IsPropria); // False (2/1)
Console.WriteLine(f21.IsAparente); // True (2/1 = 2)
Console.WriteLine(f21.IsUnitaria); // False (numerador não é 1)
Console.WriteLine(f22.IsUnitaria); // True (1/8)
Console.WriteLine(f23.IsUnitaria); // True (1/8)

// Teste 9: Validação de denominador zero
try
{
    var f24 = new Fracao(5, 0); // Usar ArgumentOutOfRangeException.ThrowIf...
    Console.WriteLine("ERRO: Deveria ter lançado exceção!");
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Exceção capturada corretamente: {ex.Message}");
}
