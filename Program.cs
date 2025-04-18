using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Cliente
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public List<Aluguel> Alugueis { get; set; } = new();
}

class Veiculo
{
    public string Modelo { get; set; }
    public string Placa { get; set; }
    public bool Disponivel { get; set; } = true;
}

class Funcionario
{
    public string Nome { get; set; }
    public string Matricula { get; set; }
}

class Aluguel
{
    public Veiculo Veiculo { get; set; }
    public DateTime DataAluguel { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public decimal Valor { get; set; }
}

class Program
{
    static List<Cliente> clientes = new();
    static List<Veiculo> veiculos = new();
    static List<Funcionario> funcionarios = new();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        CarregarDados();
        Menu();
        SalvarDados();
    }

    static void Menu()
    {
        string? opcao;
        do
        {
            Console.Clear();
            Console.WriteLine("=== LOCADORA DE VEÍCULOS ===");
            Console.WriteLine("1 - Cadastrar cliente");
            Console.WriteLine("2 - Cadastrar veículo");
            Console.WriteLine("3 - Cadastrar funcionário");
            Console.WriteLine("4 - Alugar veículo");
            Console.WriteLine("5 - Listar aluguéis");
            Console.WriteLine("6 - Devolver veículo");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha: ");
            opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": CadastrarCliente(); break;
                case "2": CadastrarVeiculo(); break;
                case "3": CadastrarFuncionario(); break;
                case "4": AlugarVeiculo(); break;
                case "5": ListarAlugueis(); break;
                case "6": DevolverVeiculo(); break;
                default:
                    if (opcao != "0")
                    {
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }
                    break;
            }

        } while (opcao != "0");
    }

    static void CadastrarCliente()
    {
        Console.Clear();
        Console.Write("Nome do cliente: ");
        string? nome = Console.ReadLine();
        Console.Write("CPF: ");
        string? cpf = Console.ReadLine();
        Console.Write("Email: ");
        string? Email = Console.ReadLine();

        if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(cpf))
        {
            clientes.Add(new Cliente { Nome = nome, Cpf = cpf });
            Console.WriteLine("Cliente cadastrado com sucesso.");
        }
        else
        {
            Console.WriteLine("Erro: Nome e CPF são obrigatórios.");
        }
        Console.ReadKey();
    }

    static void CadastrarVeiculo()
    {
        Console.Clear();
        Console.Write("Modelo do veículo: ");
        string? modelo = Console.ReadLine();
        Console.Write("Placa: ");
        string? placa = Console.ReadLine();

        if (!string.IsNullOrEmpty(modelo) && !string.IsNullOrEmpty(placa))
        {
            veiculos.Add(new Veiculo { Modelo = modelo, Placa = placa });
            Console.WriteLine("Veículo cadastrado com sucesso.");
        }
        else
        {
            Console.WriteLine("Erro: Modelo e Placa são obrigatórios.");
        }
        Console.ReadKey();
    }

    static void CadastrarFuncionario()
    {
        Console.Clear();
        Console.Write("Nome do funcionário: ");
        string? nome = Console.ReadLine();
        Console.Write("Matrícula: ");
        string? matricula = Console.ReadLine();

        if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(matricula))
        {
            funcionarios.Add(new Funcionario { Nome = nome, Matricula = matricula });
            Console.WriteLine("Funcionário cadastrado com sucesso.");
        }
        else
        {
            Console.WriteLine("Erro: Nome e Matrícula são obrigatórios.");
        }
        Console.ReadKey();
    }

    static void AlugarVeiculo()
    {
        Console.Clear();
        Console.WriteLine("ALUGAR VEÍCULO");

        Console.Write("Nome do cliente: ");
        string? nomeCliente = Console.ReadLine();
        var cliente = clientes.FirstOrDefault(c => c.Nome.Equals(nomeCliente, StringComparison.OrdinalIgnoreCase));

        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");
            Console.ReadKey();
            return;
        }

        var veiculosDisponiveis = veiculos.Where(v => v.Disponivel).ToList();

        if (!veiculosDisponiveis.Any())
        {
            Console.WriteLine("Não há veículos disponíveis.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Veículos disponíveis:");
        for (int i = 0; i < veiculosDisponiveis.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {veiculosDisponiveis[i].Modelo} - Placa: {veiculosDisponiveis[i].Placa}");
        }

        Console.Write("Escolha o número do veículo: ");
        if (!int.TryParse(Console.ReadLine(), out int opcao) || opcao < 1 || opcao > veiculosDisponiveis.Count)
        {
            Console.WriteLine("Opção inválida.");
            Console.ReadKey();
            return;
        }

        var veiculo = veiculosDisponiveis[opcao - 1];
        veiculo.Disponivel = false;

        var aluguel = new Aluguel
        {
            Veiculo = veiculo,
            DataAluguel = DateTime.Now
        };

        cliente.Alugueis.Add(aluguel);

        Console.WriteLine("Veículo alugado com sucesso.");
        Console.ReadKey();
    }

    static void ListarAlugueis()
    {
        Console.Clear();
        Console.WriteLine("LISTA DE ALUGUÉIS:");

        if (!clientes.Any(c => c.Alugueis.Any()))
        {
            Console.WriteLine("Não há aluguéis cadastrados.");
        }
        else
        {
            foreach (var cliente in clientes)
            {
                if (cliente.Alugueis.Any())
                {
                    Console.WriteLine($"Aluguéis do cliente: {cliente.Nome}");
                    foreach (var aluguel in cliente.Alugueis)
                    {
                        Console.WriteLine($"- Veículo: {aluguel.Veiculo.Modelo}, Placa: {aluguel.Veiculo.Placa}, Alugado em: {aluguel.DataAluguel.ToShortDateString()}, Devolução: {(aluguel.DataDevolucao.HasValue ? aluguel.DataDevolucao.Value.ToShortDateString() : "Em aberto")}, Valor: R${aluguel.Valor}");
                    }
                }
            }
        }

        Console.ReadKey();
    }

    static void DevolverVeiculo()
    {
        Console.Clear();
        Console.WriteLine("DEVOLUÇÃO DE VEÍCULO");

        Console.Write("Nome do cliente: ");
        string? nomeCliente = Console.ReadLine();

        var cliente = clientes.FirstOrDefault(c => c.Nome.Equals(nomeCliente, StringComparison.OrdinalIgnoreCase));
        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");
            Console.ReadKey();
            return;
        }

        var alugueisAtivos = cliente.Alugueis.Where(a => a.DataDevolucao == null).ToList();
        if (!alugueisAtivos.Any())
        {
            Console.WriteLine("Esse cliente não possui aluguéis ativos.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Aluguéis ativos:");
        for (int i = 0; i < alugueisAtivos.Count; i++)
        {
            Console.WriteLine($"{i + 1} - Veículo: {alugueisAtivos[i].Veiculo.Modelo}, Placa: {alugueisAtivos[i].Veiculo.Placa}, Alugado em: {alugueisAtivos[i].DataAluguel.ToShortDateString()}");
        }

        Console.Write("Escolha o número do aluguel para devolver: ");
        if (!int.TryParse(Console.ReadLine(), out int opcao) || opcao < 1 || opcao > alugueisAtivos.Count)
        {
            Console.WriteLine("Opção inválida.");
            Console.ReadKey();
            return;
        }

        var aluguelSelecionado = alugueisAtivos[opcao - 1];

        Console.Write("Data de devolução (dd/MM/yyyy): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataDevolucao) || dataDevolucao < aluguelSelecionado.DataAluguel)
        {
            Console.WriteLine("Data inválida.");
            Console.ReadKey();
            return;
        }

        Console.Write("Valor do aluguel: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal valor))
        {
            Console.WriteLine("Valor inválido.");
            Console.ReadKey();
            return;
        }

        aluguelSelecionado.DataDevolucao = dataDevolucao;
        aluguelSelecionado.Valor = valor;
        aluguelSelecionado.Veiculo.Disponivel = true;

        Console.WriteLine("Veículo devolvido com sucesso.");
        Console.ReadKey();
    }

    static void SalvarDados()
    {
        try
        {
            File.WriteAllText("clientes.json", JsonConvert.SerializeObject(clientes, Formatting.Indented));
            File.WriteAllText("veiculos.json", JsonConvert.SerializeObject(veiculos, Formatting.Indented));
            File.WriteAllText("funcionarios.json", JsonConvert.SerializeObject(funcionarios, Formatting.Indented));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar os dados: {ex.Message}");
            Console.ReadKey();
        }
    }

    static void CarregarDados()
    {
        try
        {
            if (File.Exists("clientes.json"))
                clientes = JsonConvert.DeserializeObject<List<Cliente>>(File.ReadAllText("clientes.json")) ?? new List<Cliente>();

            if (File.Exists("veiculos.json"))
                veiculos = JsonConvert.DeserializeObject<List<Veiculo>>(File.ReadAllText("veiculos.json")) ?? new List<Veiculo>();

            if (File.Exists("funcionarios.json"))
                funcionarios = JsonConvert.DeserializeObject<List<Funcionario>>(File.ReadAllText("funcionarios.json")) ?? new List<Funcionario>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar os dados: {ex.Message}");
            Console.ReadKey();
        }
    }
}