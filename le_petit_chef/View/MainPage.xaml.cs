using le_petit_chef.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
            inicialitzacioLlistes();
            lsbIngredients.ItemsSource = ingredients;
            cbxUnitatMesura.ItemsSource = Enum.GetValues(typeof(Unitat)).Cast<Unitat>();
            lsbPlats.ItemsSource = plats;
            cbxIngredientsPlat.ItemsSource = ingredients;

            desactivarBotons();


        }

        private void inicialitzacioLlistes()
        {
            Ingredient pebrotVermell = new Ingredient("pebrot vermell", Unitat.UDS);
            Ingredient tomaquet = new Ingredient("tomaquet", Unitat.UDS);
            Ingredient all = new Ingredient("all", Unitat.UDS);
            Ingredient arroz = new Ingredient("arroz", Unitat.G);
            Ingredient oli = new Ingredient("oli", Unitat.ML);
            Ingredient brouMarisc = new Ingredient("brou de marisc", Unitat.ML);
            Ingredient mozzarella = new Ingredient("mozzarella", Unitat.G);
            Ingredient salsaTomaquet = new Ingredient("salsaTomaquet", Unitat.G);
            Ingredient formatgeBlau = new Ingredient("formatge blau", Unitat.G);
            Ingredient formatgeParmesa = new Ingredient("formatge parmesà", Unitat.G);
            Ingredient formatgeRuloCabra = new Ingredient("formatge rulo de cabra", Unitat.G);
            Ingredient orenga = new Ingredient("orenga", Unitat.G);
            Ingredient farina = new Ingredient("farina", Unitat.G);
            Ingredient llevat = new Ingredient("llevat", Unitat.G);
            Ingredient sal = new Ingredient("sal", Unitat.G);
            Ingredient aigua = new Ingredient("aigua", Unitat.ML);
            Ingredient ou = new Ingredient("ou", Unitat.UDS);
            Ingredient llet = new Ingredient("llet", Unitat.UDS);

            ingredients.Add(pebrotVermell);
            ingredients.Add(tomaquet);
            ingredients.Add(all);
            ingredients.Add(arroz);
            ingredients.Add(oli);
            ingredients.Add(brouMarisc);
            ingredients.Add(mozzarella);
            ingredients.Add(salsaTomaquet);
            ingredients.Add(formatgeBlau);
            ingredients.Add(formatgeParmesa);
            ingredients.Add(formatgeRuloCabra);
            ingredients.Add(orenga);
            ingredients.Add(farina);
            ingredients.Add(llevat);
            ingredients.Add(sal);
            ingredients.Add(aigua);
            ingredients.Add(ou);
            ingredients.Add(llet);

            Plat paella = new Plat("AA0001", "Paella");
            Plat pizza = new Plat("AA0002", "Pizza");

            paella.afegirIngredient(arroz, 100);
            paella.afegirIngredient(brouMarisc, 250);
            paella.afegirIngredient(pebrotVermell, 1);
            paella.afegirIngredient(all, 2);
            paella.afegirIngredient(oli, 30);
            paella.afegirIngredient(sal, 2);

            pizza.afegirIngredient(salsaTomaquet, 150);
            pizza.afegirIngredient(mozzarella, 150);
            pizza.afegirIngredient(formatgeBlau, 70);
            pizza.afegirIngredient(formatgeParmesa, 70);
            pizza.afegirIngredient(formatgeRuloCabra, 70);
            pizza.afegirIngredient(orenga, 15);
            pizza.afegirIngredient(farina, 300);
            pizza.afegirIngredient(llevat, 7);
            pizza.afegirIngredient(sal, 2);
            pizza.afegirIngredient(oli, 15);
            pizza.afegirIngredient(aigua, 200);

            plats.Add(paella);
            plats.Add(pizza);
        }

        private void desactivarBotons()
        {
            btnAltaIngredient.IsEnabled = false;
            btnBaixaIngredient.IsEnabled = false;
            btnAltaPlat.IsEnabled = false;
            btnBaixaPlat.IsEnabled = false;
            btnAfegirIngredientPlat.IsEnabled = false;
            btnBaixaIngredientPlat.IsEnabled = false;
        }

        private bool validarFormulariAltaIngredients()
        {
            return Ingredient.validaNom(txtNouIngredient.Text) && cbxUnitatMesura.SelectedItem != null && !ingredients.Contains(new Ingredient(txtNouIngredient.Text, Unitat.G));
        }

        private void activarDesactivarButtonBaixaIngredients()
        {
            btnBaixaIngredient.IsEnabled = ingredients.Count > 0 && lsbIngredients.SelectedValue != null && !ingredientDinsDelPlat((Ingredient)lsbIngredients.SelectedItem);
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

        private void btnBaixaIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (lsbIngredients.SelectedValue != null)
            {
                ingredients.Remove((Ingredient)lsbIngredients.SelectedItem);
            }
            activarDesactivarButtonBaixaIngredients();
        }

        private bool ingredientDinsDelPlat(Ingredient ingredient)
        {
            for (int i = 0; i < plats.Count; i++)
            {
                if (plats[i].Ingredients.ContainsKey(ingredient))
                {
                    return true;
                }
            }

            return false;
        }

        private void txtNouIngredient_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaIngredients();
        }

        private void cbxUnitatMesura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonAltaIngredients();
        }

        private void lsbPlats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsbPlats.SelectedValue != null)
            {
                lsbIngredientsPlat.ItemsSource = plats[lsbPlats.SelectedIndex].getLlistaIngredients();
                activarDesactivarButtonBaixaPlats();
                activarDesactivarButtonAfegirIngredient();
                activarDesactivarButtonEsborrarIngredient();
            }
        }

        private void activarDesactivarButtonAfegirIngredient()
        {
            btnAfegirIngredientPlat.IsEnabled = validarFormulariAfegirIngredients();
        }

        private bool validarFormulariAfegirIngredients()
        {
            if (txtQtatIngredientsPlat.Text != "")
            {
                return cbxIngredientsPlat.SelectedItem != null && Convert.ToInt32(txtQtatIngredientsPlat.Text) > 0 && lsbPlats.SelectedItem != null;
            }
            return false;
        }

        private void activarDesactivarButtonBaixaPlats()
        {
            btnBaixaPlat.IsEnabled = plats.Count > 0 && lsbPlats.SelectedValue != null;
        }

        private void txtCodiPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaPlats();
        }

        private void txtNomPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaPlats();
        }

        private void txtDescPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaPlats();
        }

        private void activarDesactivarButtonAltaPlats()
        {
            btnAltaPlat.IsEnabled = validarFormulariAltaPlats();
        }

        private bool validarFormulariAltaPlats()
        {
            return Plat.validaCodi(txtCodiPlat.Text) && Plat.validaNom(txtNomPlat.Text) && !plats.Contains(new Plat(txtCodiPlat.Text, txtNomPlat.Text, txtDescPlat.Text));
        }

        private void btnAltaPlat_Click(object sender, RoutedEventArgs e)
        {
            Plat nouPlat = new Plat(txtCodiPlat.Text, txtNomPlat.Text, txtDescPlat.Text);
            plats.Add(nouPlat);
            buidarCampsFormulariAltaPlats();
            lsbPlats.SelectedItem = null;
        }

        private void buidarCampsFormulariAltaPlats()
        {
            txtCodiPlat.Text = "";
            txtNomPlat.Text = "";
            txtDescPlat.Text = "";
        }

        private void btnBaixaPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsbPlats.SelectedValue != null)
            {
                plats.Remove((Plat)lsbPlats.SelectedItem);
            }
            activarDesactivarButtonBaixaPlats();
        }

        private void cbxIngredientsPlat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonAfegirIngredient();
        }

        private void txtQtatIngredientsPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(txtQtatIngredientsPlat.Text, "^[0-9]{1,}$"))
            {
                txtQtatIngredientsPlat.Text = "";
            }
            activarDesactivarButtonAfegirIngredient();
        }

        private void activarDesactivarButtonEsborrarIngredient()
        {
            btnBaixaIngredientPlat.IsEnabled = lsbIngredientsPlat.SelectedItem != null;
        }

        private void btnAfegirIngredientPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsbPlats.SelectedValue != null)
            {
                if (!ingredientDinsDelPlat((Ingredient)cbxIngredientsPlat.SelectedItem))
                {
                    plats[lsbPlats.SelectedIndex].afegirIngredient((Ingredient)cbxIngredientsPlat.SelectedItem, Convert.ToInt32(txtQtatIngredientsPlat.Text));
                    lsbIngredientsPlat.ItemsSource = plats[lsbPlats.SelectedIndex].getLlistaIngredients();
                }
                netejarFormulariIngredientPlat();
            }
        }

        private void btnABaixaIngredientPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsbIngredientsPlat.SelectedItem != null)
            {

                plats[lsbPlats.SelectedIndex].esborrarIngredient(plats[lsbPlats.SelectedIndex].Ingredients.ElementAt(lsbIngredientsPlat.SelectedIndex).Key);
                lsbIngredientsPlat.ItemsSource = plats[lsbPlats.SelectedIndex].getLlistaIngredients();
            }
            activarDesactivarButtonEsborrarIngredient();
        }

        private void netejarFormulariIngredientPlat()
        {
            cbxIngredientsPlat.SelectedItem = null;
            txtQtatIngredientsPlat.Text = "";
        }

        private void sdrComanda_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (txbSliderComanda != null)
            {
                txbSliderComanda.Text = sdrComanda.Value.ToString();
                btnComandaCompres.IsEnabled = sdrComanda.Value != 0;
            }
        }

        private void btnComandaCompres_Click(object sender, RoutedEventArgs e)
        {

            String informe = "";
            Dictionary<Ingredient, int> ingredientsAComprar = new Dictionary<Ingredient, int>();
            for (int i = 0; i < plats.Count; i++)
            {
                foreach (Ingredient ing in plats[i].Ingredients.Keys)
                {
                    if (ingredientsAComprar.TryAdd(ing, plats[i].Ingredients[ing]))
                    {
                        ingredientsAComprar.TryAdd(ing, plats[i].Ingredients[ing]); //Faig el TryAdd encomptes del Add peque si no peta
                    }
                    else
                    {
                        ingredientsAComprar[ing] += plats[i].Ingredients[ing];
                    }
                }
            }

            int q = 1;
            foreach (KeyValuePair<Ingredient,int> ing in ingredientsAComprar)
            {
                informe += q + ".- " + ing.Key.Nom + ": " + ing.Value * sdrComanda.Value + " " + EnumDescriptionConverter.getDesc(ing.Key.Unitat) + "\n\n";
                q++;
            }
            txbComanda.Text = informe;
        }

        private void lsbIngredientsPlat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonEsborrarIngredient();
        }
    }
}
