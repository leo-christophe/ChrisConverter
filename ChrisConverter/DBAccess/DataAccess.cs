using System;
using Npgsql;

namespace ChrisConverter.DBAccess
{
    public class DataAccess
    {
        // Chaîne de connexion à la base de données
        public string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=chris_converter";

        public void FetchDataFromTable()
        {
            // Création de la connexion à la base de données
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    // Ouverture de la connexion
                    conn.Open();

                    // Exemple de requête SQL
                    string sql = "SELECT nom_extension from fichierextension ";

                    // Création de la commande SQL
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Exécution de la commande et récupération des résultats
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Traitement des résultats
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.GetString(0)); // Exemple d'utilisation des données récupérées
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gestion des exceptions
                    Console.WriteLine("Erreur: " + ex.Message);
                }
            }
        }
    }
}
