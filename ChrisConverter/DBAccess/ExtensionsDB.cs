using ChrisConverter.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisConverter.DBAccess
{
    public class ExtensionsDB
    {
        private readonly DataAccess _accessDB;
        public ExtensionsDB(DataAccess accessDB)
        {
            _accessDB = accessDB;    
        }

        public List<Audioextension> GET_extensions()
        {
            // Création de la connexion à la base de données
            using (var conn = new NpgsqlConnection(_accessDB.connectionString))
            {
                try
                {
                    List< Audioextension > ListeExtensions = new List<Audioextension>();
                    // Ouverture de la connexion
                    conn.Open();

                    // Exemple de requête SQL
                    string sql = "SELECT nom_extension, description_extension from fichierextension ";

                    // Création de la commande SQL
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Exécution de la commande et récupération des résultats
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Traitement des résultats
                            while (reader.Read())
                            {
                                ListeExtensions.Add(new Audioextension(reader.GetString(0), reader.GetString(1)));
                            }
                        }
                    }
                    return ListeExtensions;
                }
                catch (Exception ex)
                {
                    // Gestion des exceptions
                    throw new Exception("Erreur: " + ex.Message);
                }
            }
        }
    }
}
