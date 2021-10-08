using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace le_petit_chef.Model
{
    public class Ingredient
    {
        String nom;
        Unitat unitat;

        private const int NOM_CARACTERS_MINIM = 2;

        //Constructor amb els camps obligatoris passant pels setters
        public Ingredient(string nom, Unitat unitat)
        {
            Nom = nom;
            Unitat = unitat;
        }

        // Setter i Getter de Nom comprobant que el nom tingui una llargada minima de 4 caracters.
        public string Nom {
            get
            { 
                return nom; 
            }
            set
            {
                if (!validaNom(value)) throw new Exception("Format del nom incorrecte.");
                nom = value;
            }
        }

        // Setter i Getter de unitat
        public Unitat Unitat {
            get
            {
                return unitat;
            }
            set
            {
                unitat = value;
            }
        }

        //Validacio del nom, retornant true si te 2 caracters o mes i retornant false en cas contrari.
        //Poso 2 caracters encomptes de 4 per poder afegir ingredients com: ou, oli, ...
        public static bool validaNom(string nom)
        {
            return nom.Length >= NOM_CARACTERS_MINIM;
        }

        public override bool Equals(object obj)
        {
            return obj is Ingredient ingredient &&
                   Nom == ingredient.Nom;
        }

        public override int GetHashCode()
        {
            return 217408413 + EqualityComparer<string>.Default.GetHashCode(Nom);
        }

        public String NomComplet
        {
            get
            {
                return Nom;
            }
        }

        public override string ToString()
        {
            return NomComplet;
        }
    }
}
