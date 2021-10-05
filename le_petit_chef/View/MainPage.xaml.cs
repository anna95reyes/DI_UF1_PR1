using le_petit_chef.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace le_petit_chef
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<Ingredient> ingredients = new ObservableCollection<Ingredient>();
        private ObservableCollection<Plat> plats = new ObservableCollection<Plat>();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            inicialitzacioLlistaIngredients();
            lsbIngredients.ItemsSource = ingredients;
            cbxUnitatMesura.ItemsSource = Enum.GetValues(typeof(Unitat)).Cast<Unitat>();
            
            inicialitzacioLlistaPlats();

            desactivarBotons();

        }

        private void inicialitzacioLlistaIngredients()
        {
            Ingredient pebrotVermell = new Ingredient("pebrot vermell", Unitat.UDS);
            Ingredient tomaquet = new Ingredient("tomaquet", Unitat.UDS);
            Ingredient graAll = new Ingredient("gra d'all", Unitat.UDS);
            Ingredient arroz = new Ingredient("arroz", Unitat.G);
            Ingredient oli = new Ingredient("oli", Unitat.ML);
            Ingredient formatge = new Ingredient("formatge", Unitat.G);
            Ingredient brouMarisc = new Ingredient("brou de marisc", Unitat.ML);
            Ingredient ou = new Ingredient("ou", Unitat.UDS);

            ingredients.Add(pebrotVermell);
            ingredients.Add(tomaquet);
            ingredients.Add(graAll);
            ingredients.Add(arroz);
            ingredients.Add(oli);
            ingredients.Add(formatge);
            ingredients.Add(brouMarisc);
            ingredients.Add(ou);
        }

        private void inicialitzacioLlistaPlats()
        {
            Plat paella = new Plat("AA0001", "Paella");
            Plat pizza = new Plat("AA0002", "Pizza");

            plats.Add(paella);
            plats.Add(pizza);
        }

        private void desactivarBotons()
        {
            btnAltaIngredient.IsEnabled = false;
            btnBaixaIngredient.IsEnabled = false;
        }

        private bool validarFormulariAltaIngredients()
        {
            return Ingredient.validaNom(txtNouIngredient.Text) && cbxUnitatMesura.SelectedItem != null && !ingredients.Contains(new Ingredient(txtNouIngredient.Text, Unitat.G));
        }

        private void activarDesactivarButtonBaixaIngredients()
        {
            btnBaixaIngredient.IsEnabled = ingredients.Count > 0 && lsbIngredients.SelectedValue != null;
        }

        private void activarDesactivarButtonAltaIngredients()
        {
            btnAltaIngredient.IsEnabled = validarFormulariAltaIngredients();
        }

        private void lsbIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsbIngredients.SelectedValue != null)
            {
                activarDesactivarButtonBaixaIngredients();
            }
        }


        private void btnAltaIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient nouIngredient = new Ingredient(txtNouIngredient.Text, (Unitat)cbxUnitatMesura.SelectedItem);
            ingredients.Add(nouIngredient);
            buidarCampsFormulariAltaIngredients();
            lsbIngredients.SelectedItem = null;
        }

        private void buidarCampsFormulariAltaIngredients()
        {
            txtNouIngredient.Text = "";
            cbxUnitatMesura.SelectedItem = null;
        }

        //TODO: No hem de poder esborrar un ingredient que s’està fent servir a un plat !
        private void btnBaixaIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (lsbIngredients.SelectedValue != null)
            {
                ingredients.Remove((Ingredient)lsbIngredients.SelectedItem);
            }
            activarDesactivarButtonBaixaIngredients();
        }

        private void txtNouIngredient_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaIngredients();
        }

        private void cbxUnitatMesura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonAltaIngredients();
        }

        
    }
}
