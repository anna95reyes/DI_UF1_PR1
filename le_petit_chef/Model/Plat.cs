using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace le_petit_chef.Model
{
    public class Plat
    {
        String codi; // (en format AA0000, dos lletres i 4 xifres)
        String nom; // (mínim 5 lletres)
        String descripcio; // (opcional)
        Dictionary<Ingredient, int> ingredients;

        private const int NOM_CARACTERS_MINIM = 5;

        public Plat(string codi, string nom)
        {
            Codi = codi;
            Nom = nom;
            ingredients = new Dictionary<Ingredient, int>();
        }

        public Plat(string codi, string nom, string descripcio)
        {
            Codi = codi;
            Nom = nom;
            Descripcio = descripcio;
            ingredients = new Dictionary<Ingredient, int>();
        }

        public string Codi { 
            get
            {
                return codi;
            } 
            set 
            {
                if (!validaCodi(value)) throw new Exception("Format del codi incorrecte.");
                codi = value; 
            }
        }

        

        public string Nom {
            get
            {
                return nom;
            }
            set {
                if (!validaNom(value)) throw new Exception("Format del nom incorrecte.");
                nom = value; 
            } 
        }
        public string Descripcio {
            get
            {
                return descripcio;
            }
            set
            {
                descripcio = value;
            }
        }

        public void afegirIngredient(Ingredient nouIngredient, int qtat)
        {
            if (IngredientRepetit(nouIngredient)) throw new Exception("L'ingredient no pot estar repetit.");
            ingredients.Add(nouIngredient, qtat);
        }

        public Dictionary<Ingredient, int> getIngredients()
        {
            return ingredients;
        }

        private bool IngredientRepetit(Ingredient nouIngredient)
        {
            return ingredients.ContainsKey(new Ingredient(nouIngredient.Nom, Unitat.G));
        }

        // Validacio del Codi en format AA0000, dos lletres i 4 xifres.
        public static bool validaCodi(string codi)
        {
            return Regex.Match(codi, "^[A-Z]{2}[0-9]{4}", RegexOptions.IgnoreCase).Success;
        }

        //Validacio del nom, retornant true si te 5 caracters o mes i retornant false en cas contrari.
        public static bool validaNom(string nom)
        {
            return nom.Length >= NOM_CARACTERS_MINIM;
        }

        public override bool Equals(object obj)
        {
            return obj is Plat plat &&
                   Codi == plat.Codi;
        }

        public override int GetHashCode()
        {
            return -1172468304 + EqualityComparer<string>.Default.GetHashCode(Codi);
        }

        public String NomComplet
        {
            get
            {
                return Codi + " - " + Nom;
            }
        }

        public override string ToString()
        {
            return NomComplet;
        }

    }
}
