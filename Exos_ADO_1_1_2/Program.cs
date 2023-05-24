using Exos_ADO_1_1_2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

#region EXO 1.2

//Afficher l’« ID », le « Nom », le « Prenom » de chaque étudiant depuis la vue « V_Student » en utilisant la méthode connectée


// définition de la connexion vers la DB

//Pour ma tour PC
string connectionString = @"Data Source=TOURPCDANY\DATAVIZ;Initial Catalog=ADO_Exo1;Integrated Security=True;TrustServerCertificate=true;";

//Pour la VDI
//string connectionString = @"Data Source=FORMA-VDI303\TFTIC;Initial Catalog=Demo_ADO_DB;Integrated Security=True;TrustServerCertificate=true;;";

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

    //Inserer un nouveau student
    //Instancier un student
//    Student moi = new Student
//    {
//        FirstName = "Denys",
//        LastName = "Desmecht",
//        YearResult = 17,
//        Birthdate = new DateTime(1986, 02, 17),
//        SectionID = 1010
//    };

//    ////Insertion en DB (pas ouf , on oublie , on sait juste que ça existe)
//    ///🛑🛑🛑 BONNE METHODE A LA LIGNE 143 
//    using (SqlCommand command = connection.CreateCommand())
//    {
//        string query = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
//            " OUTPUT inserted.Id " +
//            "VALUES(@prenom, '" + moi.LastName + "', '" +
//            moi.Birthdate.ToString("yyyy-MM-dd") + "', '" + moi.YearResult + "', '" + moi.SectionID + "')";
//        Console.WriteLine(query);

//        //    //command.Parameters.Add (DOUBLON DE DEMO A NE PAS GARDER)
//        //    SqlParameter PPrenom = new SqlParameter()
//        //{
//        //    ParameterName = "prenom",
//        //    Value = moi.FirstName,
//        //    Direction = ParameterDirection.Input
//        //};

//        //command.Parameters.Add(PPrenom);

//        command.Parameters.AddWithValue("prenom", moi.FirstName);

//    command.CommandText = query;
//    connection.Open();
//    moi.Id = (int)command.ExecuteScalar();
//    connection.Close();
//    Console.WriteLine("Nouvel Id : " + moi.Id);
//}

//ICI LA BONNE METHODE
//je fais le meme que en haut mais avec requête paramétrée
//using (SqlCommand command = connection.CreateCommand())
//    {
//        command.CommandText = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
//            "VALUES (@FirstName, @LastName, @BirthDate, @YearResult, @SectionId)";

//        command.Parameters.AddWithValue("FirstName", moi.FirstName);
//        command.Parameters.AddWithValue("LastName", moi.LastName);
//        command.Parameters.AddWithValue("BirthDate", moi.Birthdate);
//        command.Parameters.AddWithValue("YearResult", moi.YearResult);
//        command.Parameters.AddWithValue("SectionId", moi.SectionID);

//        connection.Open();

//        command.ExecuteNonQuery();
//        connection.Close();
//    }


    ////Utilisation Requête parametrée

    //Student voisin = new Student
    //{
    //    FirstName = "Arthur",
    //    LastName = "Pendragon",
    //    Birthdate = new DateTime(2000, 01, 01),
    //    YearResult = 19,
    //    SectionID = 1010
    //};

    //using (SqlCommand command = connection.CreateCommand())
    //{
    //    command.CommandText = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
    //        "VALUES (@FirstName, @LastName, @BirthDate, @YearResult, @SectionId)";

    //    command.Parameters.AddWithValue("FirstName", voisin.FirstName);
    //    command.Parameters.AddWithValue("LastName", voisin.LastName);
    //    command.Parameters.AddWithValue("BirthDate", voisin.Birthdate);
    //    command.Parameters.AddWithValue("YearResult", voisin.YearResult);
    //    command.Parameters.AddWithValue("SectionId", voisin.SectionID);

    //    connection.Open();

    //    command.ExecuteNonQuery();
    //    connection.Close();
    //}

    //using (SqlCommand command = connection.CreateCommand())
    //{
    //    command.CommandText = "dbo.DeleteStudent";
    //    command.CommandType = CommandType.StoredProcedure;
    //    command.Parameters.AddWithValue("@Student_ID",27);
    //    connection.Open();
    //    try
    //    {
    //        command.ExecuteNonQuery();
    //    }
    //    catch (SqlException ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //    }
    //    connection.Close();
    //}



    //using (SqlCommand command = connection.CreateCommand() )
    //{
    //    //je récupère le nom de ma proccédure en regardant son nom à la première ligne inside
    //    command.CommandText = "[dbo].[UpdateStudent]";
    //    command.CommandType = CommandType.StoredProcedure;
    //    SqlParameter sp_id = new SqlParameter("id", moi.Id);
    //    command.Parameters.Add(sp_id);
    //    SqlParameter sp_yr = new SqlParameter("year_result", moi.YearResult);
    //    command.Parameters.Add(sp_yr);
    //    SqlParameter sp_section = new SqlParameter("section_id", moi.SectionID);
    //    command.Parameters.Add(sp_section);
    //    connection.Open();
    //    int update_row = command.ExecuteNonQuery();

    //    connection.Close() ;

    //    if (update_row > 0) Console.WriteLine("Mis à jour!");
    //}

    //using (SqlCommand command = connection.CreateCommand())
    //{
    //    command.CommandType= CommandType.StoredProcedure;
    //    command.CommandText = "dbo.DeleteStudent";
    //    command.Parameters.AddWithValue("Student_id", voisin.Id);

    //    connection.Open();
    //    int delete_row = command.ExecuteNonQuery();
    //    connection.Close();

    //    if (delete_row > 0) Console.WriteLine("Suppresion effectuée");
    //}

    //using (SqlCommand command = connection.CreateCommand())
    //{
    //    command.CommandText = "SELECT * FROM [V_Student]";
    //    connection.Open();
    //    using ( SqlDataReader reader =command.ExecuteReader())
    //    {
    //        while (reader.Read()) 
    //        {
    //            Console.WriteLine($"{reader["Id"]} : {reader["FirstName"]} {reader["LastName"]}");
    //        }
    //        connection.Close();
    //    }
    //}

}

#endregion