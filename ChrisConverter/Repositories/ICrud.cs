using ChrisConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisConverter.Repositories
{
    /// <summary>
    /// Interface du CRUD
    /// </summary>
    /// <typeparam name="T">Objet, entité</typeparam>
    interface ICrud<T>
    {
        /// <summary>
        /// Récupérer tout les éléments de la table
        /// </summary>
        /// <returns>Une liste composant tous les éléments de la table</returns>
        List<T> GetAll();

        /// <summary>
        /// Récupérer un objet dont l'ID est spécifié
        /// </summary>
        /// <param name="ID">ID de l'entité que l'on veut récupérer</param>
        /// <returns>Objet spécifié</returns>
        T GetID(int ID);

        /// <summary>
        /// Ajoute un objet T dans la table correspondant
        /// </summary>
        /// <param name="entityToAdd">L'entité à ajouter</param>
        void Add(T entityToAdd);

        /// <summary>
        /// Mettre à jour une entité dans la base de données
        /// </summary>
        /// <param name="ID">ID de l'entité</param>
        /// <param name="newEntity">Nouvelle entité qui va la remplacer</param>
        void Update(int ID, T newEntity);
   
        /// <summary>
        /// Supprime une entité dont l'ID est passé en arguments.
        /// </summary>
        /// <param name="ID">ID de l'entité à supprimer.</param>
        void Delete(int ID);
    }
}
