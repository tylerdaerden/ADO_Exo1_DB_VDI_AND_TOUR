using Exos_ADO_1_1_2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

#region EXO 1.2

//Afficher l’« ID », le « Nom », le « Prenom » de chaque étudiant depuis la vue « V_Student » en utilisant la méthode connectée


// définition de la connexion vers la DB

string connectionString = @"Data Source=FORMA-VDI303\TFTIC;Initial Catalog=ADO_Exo1;User ID=Chris;Password=Test1234=;TrustServerCertificate=true;";

using (SqlConnection connection = new SqlConnection())
{
    connection.ConnectionString = connectionString;
    Console.WriteLine("Mode Connecté");
    Console.WriteLine("*************");
    Console.WriteLine();
    using (SqlCommand command = connection.CreateCommand())
    {

        command.CommandText = "SELECT * FROM [V_Student]";
        command.CommandType = CommandType.Text;
        // OU
        // command.CommandType = System.Data.CommandType.Text;


        connection.Open(); // J'ouvre ici la connection (je ne la ferme qu'à la toute fin car je fais mes 3 exos dans la même connection de ma ligne 14)

        using (SqlDataReader reader = command.ExecuteReader())
        {
            //Lecture de la DB
            while (reader.Read())
            {
                Student student = new Student()
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    YearResult = (int)reader["YearResult"],
                    Birthdate = (DateTime)reader["BirthDate"]

                };

                Console.WriteLine($" {student.Id} : {student.FirstName} : {student.LastName} ");
            }
        }
    }

    //Afficher l’« ID », le « Nom » de chaque section en utilisant la méthode déconnectée

    Console.WriteLine();
    Console.WriteLine("Mode Déconnecté");
    Console.WriteLine("*************");
    Console.WriteLine();

    //using (SqlConnection connection = new SqlConnection())
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_Student]";
        command.CommandType = CommandType.Text;

        //Créer l'adapter avec la config pour la récupération de donnée
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;

        //Utilisation de l'adapter pour populer un "DataSet" / "DataTable"
        DataTable table = new DataTable();
        adapter.Fill(table);

        //Parcours des resultats
        foreach (DataRow row in table.Rows)
        {
            Console.WriteLine($"{row["Id"]} : {row["LastName"]}");
        }
        connection.Close(); // Fermeture de la connection 
    }

    //Afficher la moyenne annuelle des étudiants

    Console.WriteLine();
    Console.WriteLine("Moyenne annuelle des étudiants");
    Console.WriteLine("******************************");


    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT AVG(CONVERT(FLOAT, [YearResult])) FROM [V_Student] ";
        command.CommandType = CommandType.Text;

        connection.Open();
        double moyenne = (double)command.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"moyenne {moyenne}");
    }
}

#endregion