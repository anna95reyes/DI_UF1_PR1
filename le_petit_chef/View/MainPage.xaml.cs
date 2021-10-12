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
            //Aixo serveix per poder agafar el valor de la Enumeracio
            cbxUnitatMesura.ItemsSource = Enum.GetValues(typeof(Unitat)).Cast<Unitat>(); 
            lsbPlats.ItemsSource = plats;
            cbxIngredientsPlat.ItemsSource = ingredients;

            desactivarBotons();
        }

        /*
         * Inicialitzo totes les dades d'exemple perque la aplicació no estigui buida
         */
        private void inicialitzacioLlistes()
        {
            Ingredient pebrotVermell = new Ingredient("pebrot vermell", Unitat.UDS);
            Ingredient tomaquet = new Ingredient("tomaquet", Unitat.UDS);
            Ingredient all = new Ingredient("all", Unitat.UDS);
            Ingredient arroz = new Ingredient("arroz", Unitat.G);
            Ingredient oli = new Ingredient("oli", Unitat.ML);
            Ingredient brouMarisc = new Ingredient("brou de marisc", Unitat.ML);
            Ingredient mozzarella = new Ingredient("mozzarella", Unitat.G);
            Ingredient salsaTomaquet = new Ingredient("salsa tomaquet", Unitat.G);
            Ingredient formatgeBlau = new Ingredient("formatge blau", Unitat.G);
            Ingredient formatgeParmesa = new Ingredient("formatge parmesà", Unitat.G);
            Ingredient formatgeRuloCabra = new Ingredient("formatge rulo de cabra", Unitat.G);
            Ingredient orenga = new Ingredient("orenga", Unitat.G);
            Ingredient farina = new Ingredient("farina", Unitat.G);
            Ingredient llevat = new Ingredient("llevat", Unitat.G);
            Ingredient sal = new Ingredient("sal", Unitat.G);
            Ingredient aigua = new Ingredient("aigua", Unitat.ML);
            Ingredient ou = new Ingredient("ou", Unitat.UDS);
            Ingredient llet = new Ingredient("llet", Unitat.ML);

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

        /*
         * Aquesta funcio la crido nomes en el Page_Loaded i el que fa es desactivar tots els botons,
         * quan s'inicia l'aplicacio es pot veure que el btnComandaCompres esta activat, no el posso aqui perque li
         * tinc posat que el valor per defecte sigui 1 quantitat de plats (1 comensal), pero per exemple,
         * si li poses un valor 0 es queda desactivat perque no te sentit treure l'informe de compres si no tens cap comensal.
         */
        private void desactivarBotons()
        {
            btnAltaIngredient.IsEnabled = false;
            btnBaixaIngredient.IsEnabled = false;
            btnAltaPlat.IsEnabled = false;
            btnBaixaPlat.IsEnabled = false;
            btnAfegirIngredientPlat.IsEnabled = false;
            btnBaixaIngredientPlat.IsEnabled = false;
        }

        /*
         * Per poder activar el boto de la Baixa dels Ingredients, comprobo que hi haigui algun ingredient seleccionat
         * i que l'ingredient no estigui sent utilitzant en algun plat.
         * Si alguna d'aquestes dues comprovacions no es valida, el boto es quedara desactivat
         */
        private void activarDesactivarButtonBaixaIngredients()
        {
            btnBaixaIngredient.IsEnabled = lsbIngredients.SelectedValue != null && !ingredientUtilitzat((Ingredient)lsbIngredients.SelectedItem);
        }

        /*
         * Per poder activar el boto del Alta dels Ingredients, comprobo que el nom sigui valid, que tiguem una unitat sel·leccionada,
         * i que l'ingredient no estigui repetit, tambe comprobo que el nom no estigui en minuscules i es vulgui afegir el mateix plat en mayuscules o viceversa.
         * Si alguna d'aquestes tres comprobacions no son valides, el boto es quedara desactivat
         */
        private void activarDesactivarButtonAltaIngredients()
        {
            btnAltaIngredient.IsEnabled = Ingredient.validaNom(txtNouIngredient.Text) && cbxUnitatMesura.SelectedItem != null && !ingredients.Contains(new Ingredient(txtNouIngredient.Text.ToLower(), Unitat.G));
        }

        /*
         * Quan selecciono un ingredient, comprobo realment que tinc un ingredient i activo el boto de Baixa dels Ingredients.
         */
        private void lsbIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsbIngredients.SelectedValue != null)
            {
                activarDesactivarButtonBaixaIngredients();
            }
        }

        /*
         * Quan es pot clicar sobre el boto de l'Alta dels Ingredients, vol dir que tots els camps del formulari son correctes, per tant
         * arribats a aquest punt afegeixo el ingredient nou i buido el formulari.
         * Deixo el listBox de ingredents sense seleccionar perque considero que es l'usuari que ha de clicar sobre l'ingredient que vulgui per poder esborrar-lo,
         * ja que aquest programa no permet fer modificacions, i no podem fer cap validacio de si l'usuari realment vol esborrar l'ingredient,
         * d'aquesta manera, considero que si un ingredient es borra es perque l'usuari es conscient de fer-ho.
         */
        private void btnAltaIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient nouIngredient = new Ingredient(txtNouIngredient.Text, (Unitat)cbxUnitatMesura.SelectedItem);
            ingredients.Add(nouIngredient);
            buidarCampsFormulariAltaIngredients();
            lsbIngredients.SelectedItem = null;
        }

        /*
         * Aquesta funcio l'unic que fa es deixar en blanc el textBox dels ingredients i deseleccionar el combobox. Aquesta funcio la crido quan
         * es crea un nou ingredient per aixi deixar els camps en blanc despres de haber fer el add
         */
        private void buidarCampsFormulariAltaIngredients()
        {
            txtNouIngredient.Text = "";
            cbxUnitatMesura.SelectedItem = null;
        }

        /*
         * Quan s'activa aquest boto significa que tenim en sel·leccio un ingredient el qual no s'esta utilitzant en cap plat,
         * igualment, comprobo que la seleccio no sigui nula i esborro l'ingredient, al fer aquest pas torno a cridar la funcio
         * que activa o desactiva el btnBaixaIngredient perque en aquest cas el desactivi fins que no es torni a sel·leccionar un 
         * ingredient que es pugui esborrar.
         */
        private void btnBaixaIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (lsbIngredients.SelectedValue != null)
            {
                ingredients.Remove((Ingredient)lsbIngredients.SelectedItem);
            }
            activarDesactivarButtonBaixaIngredients();
        }

        /*
         * Aquesta funcio retorna true si troba l'ingredient en algun dels plats de la llista de plats, aixo ho faig per saber si hi ha un ingredient
         * que s'estigui utilitzant en alguns dels plats.
         * Aquesta funcio la crido a l'hora de voler esborar un ingredient, si retorna true significa que l'ingredient esta dins d'un dels plats de la llista,
         * i per tant no es pot esborrar
         */
        private bool ingredientUtilitzat(Ingredient ingredient)
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

        /*
         * Aquesta funcio retorna true si el ingredient que pasem per parametre ja esta dintre del plat que tenim en sel·leccio, aquesta funcio
         * la faig servir quan vui inserir un ingredient dins d'un plat. El primer que faig abans de inserir-lo es mirar si l'ingredient ja existeix.
         */
        private bool ingredientDinsDelPlatSeleccionat(Ingredient ingredient)
        {
            return plats[lsbPlats.SelectedIndex].Ingredients.ContainsKey(ingredient);
        }

        /*
         * Al escriure en el TextBox del formulari d'alta dels ingredients, comprobo que el formulari sigui correcte per poder activar el boto de Alta Ingredients.
         */
        private void txtNouIngredient_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaIngredients();
        }

        /*
         * Al seleccionar qualsevol unitat de mesura del combo box del formulari d'alta dels ingredients, comprobo que el formulari sigui correcte per poder activar
         * el boto de Alta Ingredients
         */
        private void cbxUnitatMesura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonAltaIngredients();
        }

        /*
         * Quan seleccionem un plat, el primer que faig es verificar que realment el valor sel·leccionar no sigui nul,
         * despres mostro la llista dels ingredients del plat que hem sel·lecionat, i a continuacio,
         * comprobo si puc activar el boto de baixa dels plats, el de afegir ingredients al plat i el de esborrar ingredients del plat.
         */
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

        /*
         * Per poder activar o desactivar el Boto d'afegir Ingredients, comprobo que el formulari d'alta dels ingredients al plat sigui valid
         */
        private void activarDesactivarButtonAfegirIngredient()
        {
            btnAfegirIngredientPlat.IsEnabled = validarFormulariAfegirIngredients();
        }

        /*
         * Per verificar que el formulari d'afegir ingredients al plat sigui valid, comprobo que la quantitat de l'ingredient no estigui en blanc, perque en cas contrari
         * retorno nul pel fer que al fer el Convert.ToInt32 d'un valor buit o null, salta una excepcio, per tant un com tinc comprobar aixo,
         * comprobo que l'ingredient del combobox dels Ingredients del plat no sigui null, paso la quantitat del ingredient a integer i comprobo que com a minim sigui mayor
         * que 0, i per ultim, comprobo que la llista dels plats no sigui nula
         */
        private bool validarFormulariAfegirIngredients()
        {
            if (txtQtatIngredientsPlat.Text != "")
            {
                return cbxIngredientsPlat.SelectedItem != null && Convert.ToInt32(txtQtatIngredientsPlat.Text) > 0 && lsbPlats.SelectedItem != null;
            }
            return false;
        }

        /*
         * Per activar o desactivar el boto de baixa dels plats, orimer comprobo que hi haiguin plats a la llista i que la seleccio dels plats no sigui nula
         */
        private void activarDesactivarButtonBaixaPlats()
        {
            btnBaixaPlat.IsEnabled = plats.Count > 0 && lsbPlats.SelectedValue != null;
        }

        /*
         * Per donar opcio a Activar o que segueixi desactivar el boto d'alta plats, comprobo que s'estigui escribint el codi del plat
         */
        private void txtCodiPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaPlats();
        }

        /*
         * Per donar opcio a Activar o que segueixi desactivar el boto d'alta plats, comprobo que s'estigui escribint el nom del plat
         */
        private void txtNomPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaPlats();
        }

        /*
         * Per donar opcio a Activar o que segueixi desactivar el boto d'alta plats, comprobo que s'estigui escribint la descripcio del plat, encara
         * que aquest pas es opcional i amb els altres dos camps el boto ja queda activat.
         */
        private void txtDescPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            activarDesactivarButtonAltaPlats();
        }

        /*
         * Per activar o desactivar el boto d'alta dels plats, valido el formulari de l'alta
         */
        private void activarDesactivarButtonAltaPlats()
        {
            btnAltaPlat.IsEnabled = validarFormulariAltaPlats();
        }

        /*
         * En la validacio del Alta dels Plats, valido si el codi es correcte, en format AA0000, si el nom es mayor o igual a 5 caracters,
         * i si el plat que volem inserir no existeix
         */
        private bool validarFormulariAltaPlats()
        {
            return Plat.validaCodi(txtCodiPlat.Text) && Plat.validaNom(txtNomPlat.Text) && !plats.Contains(new Plat(txtCodiPlat.Text, txtNomPlat.Text, txtDescPlat.Text));
        }

        /*
         * Un cop que el boto del alta del plat esta activar, creo un plat nou i l'afegeixo a la llista dels plats, despres buido el camps del formulari
         * de l'alta dels plats i deixo la llista dels plats sense cap plat sel·lecionat.
         */
        private void btnAltaPlat_Click(object sender, RoutedEventArgs e)
        {
            Plat nouPlat = new Plat(txtCodiPlat.Text, txtNomPlat.Text, txtDescPlat.Text);
            plats.Add(nouPlat);
            buidarCampsFormulariAltaPlats();
            lsbPlats.SelectedItem = null;
        }

        /*
         * Per buidar els camps del formulari de l'alta dels plats, deixo en blanc en tres textbox del codi, el nom i la descripcio del plat.
         */
        private void buidarCampsFormulariAltaPlats()
        {
            txtCodiPlat.Text = "";
            txtNomPlat.Text = "";
            txtDescPlat.Text = "";
        }

        /*
         * Per que el boto de baixa dels plats estigui activat primer he comprobat que hi haigui algun plat en sel·lecio, despres miro si realment la seleccio no dona
         * un valor nul, llavors esborro el plat de la llista, deixo la llista dels ingredients del plat buida i neteixo l'informe, perque si hem esborrat un plat
         * que te ingredients d'intre, el informe quedaria incoherent, i per ultim desactivi el boto de la baixa dels plats.
         */
        private void btnBaixaPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsbPlats.SelectedValue != null)
            {
                plats.Remove((Plat)lsbPlats.SelectedItem);
                lsbIngredientsPlat.ItemsSource = null;
                netejarInforme();
            }
            activarDesactivarButtonBaixaPlats();
        }

        /*
         * Per activar el boto d'afegir un ingredient dins del plat, comprobo que el combobox dels ingredients tingui algun element sel·leccionat
         */
        private void cbxIngredientsPlat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonAfegirIngredient();
        }

        /*
         * Per asegurarme que el que escriuen al textbox de la quantitat d'ingredients sigui realment un numero el que faix es comprobar si realment el que escriuen es un numero
         * si escriuen algo que no sigui un numero automaticament el text el deixo en blanc perque tornin a començar, ho comprobo d'aquesta manera per no tenir que estar
         * col·locant el cursor al lloc on tocaria, finalment miro si es pot activar el boto d'afegir els ingredients.
         */
        private void txtQtatIngredientsPlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(txtQtatIngredientsPlat.Text, "^[0-9]{1,}$"))
            {
                txtQtatIngredientsPlat.Text = "";
            }
            activarDesactivarButtonAfegirIngredient();
        }

        /*
         * Per activar el boto d'esborrar un ingredient del plat, miro si hi ha un ingredient sel·lecionat.
         */
        private void activarDesactivarButtonEsborrarIngredient()
        {
            btnBaixaIngredientPlat.IsEnabled = lsbIngredientsPlat.SelectedItem != null;
        }

        /*
         * Un cop hem pasat les validacions perque el boto de afegir un ingredient al plat es pugui activar, miro si realment hi ha un plat sel·lecionat,
         * despres comprobo que l'ingredient que volem afegir no estigui ja dins del plat sel·leccionat, si no esta dins del plat l'afegeixo i recarrego la llista dels
         * ingredients, despres netejo el formulari d'alta dels ingredients i per ultim netejo l'informe de compres pel motiu que si afegim un ingredient nou,
         * el formulari de compres quedaria incoherent.
         */
        private void btnAfegirIngredientPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsbPlats.SelectedValue != null)
            {
                if (!ingredientDinsDelPlatSeleccionat((Ingredient)cbxIngredientsPlat.SelectedItem))
                {
                    plats[lsbPlats.SelectedIndex].afegirIngredient((Ingredient)cbxIngredientsPlat.SelectedItem, Convert.ToInt32(txtQtatIngredientsPlat.Text));
                    lsbIngredientsPlat.ItemsSource = plats[lsbPlats.SelectedIndex].getLlistaIngredients();
                }
                netejarFormulariIngredientPlat();
                netejarInforme();
            }
        }

        /*
         * Un cop estigui activat el boto de baixa del ingredients del plat, comprobo que realment tingui un ingredient en sel·leccio,
         * esborro l'ingredient del plat i despres torno a regenerar la llista dels ingredients.
         * per ultim miro si es pot activar el boto d'esborrar els ingredients i netejo l'informe de compres perque no deixi de ser coherent.
         */
        private void btnBaixaIngredientPlat_Click(object sender, RoutedEventArgs e)
        {
            if (lsbIngredientsPlat.SelectedItem != null)
            {
                plats[lsbPlats.SelectedIndex].esborrarIngredient(plats[lsbPlats.SelectedIndex].Ingredients.ElementAt(lsbIngredientsPlat.SelectedIndex).Key);
                lsbIngredientsPlat.ItemsSource = plats[lsbPlats.SelectedIndex].getLlistaIngredients();
            }
            activarDesactivarButtonEsborrarIngredient();
            netejarInforme();
        }

        /*
         * Per netejar el formulari d'alta dels ingredients del plat deixo deseleccionat el combobox dels ingredients del plat i deixo en blanc el textbox de la 
         * quantitat de l'ingredient
         */
        private void netejarFormulariIngredientPlat()
        {
            cbxIngredientsPlat.SelectedItem = null;
            txtQtatIngredientsPlat.Text = "";
        }

        /*
         * El slider el la comanda mira si ha cambiat de valor per actualitzar el textblock del Slider, que es el que mostra el valor que hi ha en sel·leccio
         * en el slider, comprobo que el textblock no sigui nul perque sempre ha de tindre un valor, ja que s'inicialitza a 1, i aixi faig que no peti al principi
         * del programa.
         * Tambe desactivo el boto de mostrar la comanda de compra si el valor del slider el 0, ja que no te sentit mostrar una comanda de compra si no hi han comensals
         */
        private void sdrComanda_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (txbSliderComanda != null)
            {
                txbSliderComanda.Text = sdrComanda.Value.ToString();
                btnComandaCompres.IsEnabled = sdrComanda.Value != 0;
            }
        }

        /*
         * Un cop el boto de comandes de compres estigui activar, creo un diccionari nomes per la comanda,
         * faig mirant els ingredients dels plats i si no el tinc al nou diccionari l'afegeixo, si no, sumo la quantitat que tenia
         * amb la nova quantitat que te a un altre plat.
         * Un cop ja tinc el diccionari de la comanda be, el mostro i segons el valor del slider, multiplico la quantitat del ingredient, per la quantitat
         * de comensals per saber els ingredients que s'han de comprar segons els comensals que siguin i mostro l'informe.
         */
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

        /*
         * Si el listbox dels ingredients del plat entra en sel·leccio activo el boto de Esborrar l'ingredient del plat, per aixo li paso la funcio
         * perque comprobi que realment es pot activar el boto.
         */
        private void lsbIngredientsPlat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarDesactivarButtonEsborrarIngredient();
        }

        /*
         * Netejo l'informe quan s'afegeix o s'esborra qualsevol ingredient del plat i quan s'esborra el plat ja que aquestes tres accions poden
         * fer variar l'informe.
         */
        private void netejarInforme()
        {
            txbComanda.Text = "";
        }
    }
}
