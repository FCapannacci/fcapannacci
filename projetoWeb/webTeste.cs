using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ConsultaCompradores
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configurar a conexão com o banco de dados
            string connectionString = "server=localhost;database=nome_do_banco;uid=usuario;pwd=senha";
            MySqlConnection connection = new MySqlConnection(connectionString);

            // Exibir a tela de listagem de compradores
            Console.WriteLine("Consulte os seus Clientes cadastrados na sua Loja ou realize o cadastro de novos Clientes");
            Console.WriteLine("1. Filtrar");
            Console.WriteLine("2. Adicionar Cliente");
            Console.WriteLine("Escolha uma opção:");

            int opcao = Convert.ToInt32(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    // Exibir a área de pesquisa de compradores
                    Console.WriteLine("Filtro de Compradores");
                    Console.WriteLine("Digite o Nome/Razão Social:");
                    string nomeRazaoSocial = Console.ReadLine();

                    Console.WriteLine("Digite o E-mail:");
                    string email = Console.ReadLine();

                    Console.WriteLine("Digite o Telefone:");
                    string telefone = Console.ReadLine();

                    Console.WriteLine("Digite a Data de Cadastro (formato: dd/mm/aaaa):");
                    string dataCadastroStr = Console.ReadLine();
                    DateTime dataCadastro = DateTime.ParseExact(dataCadastroStr, "dd/MM/yyyy", null);

                    Console.WriteLine("Digite 1 para filtrar por Compradores bloqueados, 0 para não bloqueados:");
                    int clienteBloqueado = Convert.ToInt32(Console.ReadLine());

                    // Realizar a consulta no banco de dados
                    List<Comprador> compradoresFiltrados = ConsultarCompradores(connection, nomeRazaoSocial, email, telefone, dataCadastro, clienteBloqueado);

                    // Exibir os resultados
                    Console.WriteLine("Compradores encontrados:");
                    foreach (Comprador comprador in compradoresFiltrados)
                    {
                        Console.WriteLine($"Nome/Razão Social: {comprador.NomeRazaoSocial}");
                        Console.WriteLine($"E-mail: {comprador.Email}");
                        Console.WriteLine($"Telefone: {comprador.Telefone}");
                        Console.WriteLine($"Data de Cadastro: {comprador.DataCadastro.ToShortDateString()}");
                        Console.WriteLine($"Cliente Bloqueado: {(comprador.ClienteBloqueado ? "SIM" : "NÃO")}");
                        Console.WriteLine();
                    }

                    break;
                case 2:
                    // Lógica para adicionar um novo cliente
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            // Fechar a conexão com o banco de dados
            connection.Close();

            Console.ReadLine();
        }

        static List<Comprador> ConsultarCompradores(MySqlConnection connection, string nomeRazaoSocial, string email, string telefone, DateTime dataCadastro, int clienteBloqueado)
        {
            List<Comprador> compradores = new List<Comprador>();

            // Construir a consulta SQL
            string query = "SELECT * FROM Compradores WHERE 1=1";
            if (!string.IsNullOrEmpty(nomeRazaoSocial))
                query += $" AND NomeRazaoSocial LIKE '%{nomeRazaoSocial}%'";
            if (!string.IsNullOrEmpty(email))
                query += $" AND Email LIKE '%{email}%'";
            if (!string.IsNullOrEmpty(telefone))
                query += $" AND Telefone LIKE '%{telefone}%'";
            if (dataCadastro != DateTime.MinValue)
                query += $" AND DataCadastro = '{dataCadastro.ToString("yyyy-MM-dd")}'";
            if (clienteBloqueado == 1)
                query += " AND ClienteBloqueado = 1";
            else if (clienteBloqueado == 0)
                query += " AND ClienteBloqueado = 0";

            // Executar a consulta
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Ler os dados retornados
            while (reader.Read())
            {
                Comprador comprador = new Comprador();
                comprador.NomeRazaoSocial = reader.GetString("NomeRazaoSocial");
                comprador.Email = reader.GetString("Email");
                comprador.Telefone = reader.GetString("Telefone");
                comprador.DataCadastro = reader.GetDateTime("DataCadastro");
                comprador.ClienteBloqueado = reader.GetBoolean("ClienteBloqueado");

                compradores.Add(comprador);
            }

            reader.Close();

            return compradores;
        }
    }

    class Comprador
    {
        public string NomeRazaoSocial { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool ClienteBloqueado { get; set; }
    }
}
