using System;
using System.Collections.Generic;

namespace DesafioProjetoHospedagem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Criando hóspedes
            Pessoa p1 = new Pessoa("João");
            Pessoa p2 = new Pessoa("Maria");
            Pessoa p3 = new Pessoa("Pedro");

            // Criando suíte (capacidade e valor da diária)
            Suite suite = new Suite("Premium", 2, 250.00m);

            try
            {
                // Tentativa de reserva com 3 hóspedes em suíte para 2 pessoas -> deve lançar exception
                Reserva reservaInvalida = new Reserva(suite, 3);
                reservaInvalida.Pessoas.Add(p1);
                reservaInvalida.Pessoas.Add(p2);
                reservaInvalida.Pessoas.Add(p3);

                Console.WriteLine("Quantidade hóspedes (inválida): " + reservaInvalida.ObterQuantidadeHospedes());
                Console.WriteLine("Valor diária (inválida): " + reservaInvalida.CalcularValorDiaria());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Erro ao criar reserva inválida: " + ex.Message);
            }

            // Cria uma reserva válida com 2 hóspedes por 12 dias (para aplicar desconto)
            Suite suite2 = new Suite("Master", 3, 300.00m);
            Reserva reserva = new Reserva(suite2, 12); // 12 dias -> desconto 10%
            reserva.Pessoas.Add(p1);
            reserva.Pessoas.Add(p2);

            Console.WriteLine();
            Console.WriteLine("=== Reserva Válida ===");
            Console.WriteLine("Suíte: " + suite2.Nome);
            Console.WriteLine("Capacidade: " + suite2.Capacidade);
            Console.WriteLine("Hóspedes: " + reserva.ObterQuantidadeHospedes());
            Console.WriteLine("Dias: " + reserva.DiasReservados);
            Console.WriteLine("Valor por diária: R$ " + suite2.ValorDiaria.ToString("F2"));
            Console.WriteLine("Valor total (com desconto se aplicável): R$ " + reserva.CalcularValorDiaria().ToString("F2"));

            // Outra reserva sem desconto (menos que 10 dias)
            Reserva reservaSemDesconto = new Reserva(suite2, 5);
            reservaSemDesconto.Pessoas.Add(p1);
            Console.WriteLine();
            Console.WriteLine("=== Reserva Sem Desconto ===");
            Console.WriteLine("Hóspedes: " + reservaSemDesconto.ObterQuantidadeHospedes());
            Console.WriteLine("Dias: " + reservaSemDesconto.DiasReservados);
            Console.WriteLine("Valor total: R$ " + reservaSemDesconto.CalcularValorDiaria().ToString("F2"));
        }
    }

    public class Pessoa
    {
        public string Nome { get; private set; }

        public Pessoa(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome inválido", nameof(nome));

            Nome = nome;
        }
    }

    public class Suite
    {
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public decimal ValorDiaria { get; set; }

        public Suite(string nome, int capacidade, decimal valorDiaria)
        {
            if (capacidade <= 0) throw new ArgumentException("Capacidade deve ser maior que zero.", nameof(capacidade));
            if (valorDiaria < 0) throw new ArgumentException("Valor da diária não pode ser negativo.", nameof(valorDiaria));

            Nome = nome;
            Capacidade = capacidade;
            ValorDiaria = valorDiaria;
        }
    }

    public class Reserva
    {
        public List<Pessoa> Pessoas { get; private set; } = new List<Pessoa>();
        public Suite Suite { get; private set; }
        public int DiasReservados { get; private set; }

        public Reserva(Suite suite, int diasReservados)
        {
            if (suite == null) throw new ArgumentNullException(nameof(suite));
            if (diasReservados <= 0) throw new ArgumentException("Dias reservados deve ser maior que zero.", nameof(diasReservados));

            Suite = suite;
            DiasReservados = diasReservados;
        }

        // Retorna quantidade total de hóspedes
        public int ObterQuantidadeHospedes()
        {
            return Pessoas.Count;
        }

        // Calcula o valor total da diária (dias * valor) aplicando desconto de 10% se dias >= 10
        public decimal CalcularValorDiaria()
        {
            // Validar capacidade: não deve permitir reserva com mais hóspedes que a capacidade da suíte.
            if (ObterQuantidadeHospedes() > Suite.Capacidade)
            {
                throw new InvalidOperationException($"Número de hóspedes ({ObterQuantidadeHospedes()}) excede a capacidade da suíte ({Suite.Capacidade}).");
            }

            decimal total = DiasReservados * Suite.ValorDiaria;

            if (DiasReservados >= 10)
            {
                decimal desconto = total * 0.10m; // 10%
                total -= desconto;
            }

            return total;
        }
    }
}
