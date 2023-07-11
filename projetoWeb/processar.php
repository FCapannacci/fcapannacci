using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        Console.WriteLine("Informe o nome:");
        string nome = Console.ReadLine();

        Console.WriteLine("Informe o telefone:");
        string telefone = Console.ReadLine();

        string connectionString = "server=localhost;database=seu_banco_de_dados;user=root;password=sua_senha;";
        
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO tabela (nome, telefone) VALUES (@nome, @telefone)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@telefone", telefone);

                command.ExecuteNonQuery();
            }

            Console.WriteLine("Dados inseridos no banco de dados com sucesso!");
        }
    }
}