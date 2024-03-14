using ChrisConverter.Repositories;
using ChrisConverter.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisConverter.Repositories
{
    /// <summary>
    /// Classe permettant de contrôler la liaison du code avec la base de données (Database First)
    /// Implémentation de l'interface ICrud pour plus de flexibilité
    /// </summary>
    public class ExtensionsDB: ICrud<Audioextension>
    {
        private readonly DBAccess _accessDB;
        public ExtensionsDB(DBAccess accessDB)
        {
            _accessDB = accessDB;    
        }

        /// <summary>
        /// Récupérer toutes les extensions avec leurs noms et descriptions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Audioextension> GetAll()
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

        public Audioextension GetID(int ID)
        {
            throw new NotImplementedException();
        }

        public void Add(Audioextension entityToAdd)
        {
            throw new NotImplementedException();
        }

        public void Update(int ID, Audioextension newEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
