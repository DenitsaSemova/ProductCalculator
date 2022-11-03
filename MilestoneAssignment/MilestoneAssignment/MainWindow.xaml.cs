using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ProductCalculator
{

    public partial class MainWindow : Window
    {

        JarOfLutenitsa jarOfLutenitsa = new JarOfLutenitsa();
        JarOfKiopolu jarOfKiopolu = new JarOfKiopolu();

        double totalCapacity = 200;

        double currPeppersQuantity = 0;
        double currTomatoesQuantity = 0;
        double currCarrotsQuantity = 0;
        double currSaltQuantity = 0;
        double currSugarQuantity = 0;
        double currEggplantQuantity = 0;
        double currGarlicQuantity = 0;
        double currOnionQuantity = 0;

        double peppersQuantity = 0;
        double tomatoesQuantity = 0;
        double carrotsQuantity = 0;
        double eggplantQuantity = 0;
        double garlicQuantity = 0;
        double onionQuantity = 0;
        double saltQuantity = 0;
        double sugarQuantity = 0;

        double totalJarsOfLiutenitsa = 0;
        double currJarsOfLiutenitsa = 0;
        double totalJarsOfKiopolu = 0;
        double currJarsOfKiopolu = 0;
        double totalJars = 0;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (peppersTextBox.Text == String.Empty ||
                tomatoesTextBox.Text == String.Empty ||
                carrotsTextBox.Text == String.Empty ||
                saltTextBox.Text == String.Empty ||
                sugarTextBox.Text == String.Empty ||
                eggplantTextBox.Text == String.Empty ||
                garlicTextBox.Text == String.Empty ||
                onionTextBox.Text == String.Empty)
            {
                MessageBox.Show("Необходимо е да въведете количество за всеки продукт. " +
                    "Ако нямате налично, моля въведете 0");

            }
            else
            {
                try
                {
                    currPeppersQuantity = double.Parse(peppersTextBox.Text);
                    currTomatoesQuantity = double.Parse(tomatoesTextBox.Text);
                    currCarrotsQuantity = double.Parse(carrotsTextBox.Text);
                    currSaltQuantity = double.Parse(saltTextBox.Text);
                    currSugarQuantity = double.Parse(sugarTextBox.Text);
                    currEggplantQuantity = double.Parse(eggplantTextBox.Text);
                    currGarlicQuantity = double.Parse(garlicTextBox.Text);
                    currOnionQuantity = double.Parse(onionTextBox.Text);

                    peppersQuantity += currPeppersQuantity;
                    tomatoesQuantity += currTomatoesQuantity;
                    carrotsQuantity += currCarrotsQuantity;
                    saltQuantity += currSaltQuantity;
                    sugarQuantity += currSugarQuantity;
                    eggplantQuantity += currEggplantQuantity;
                    garlicQuantity += currGarlicQuantity;
                    onionQuantity += currOnionQuantity;
                }
                catch
                {

                    MessageBox.Show("Необходимо е да въведете количество за всеки продукт. " +
                        "Ако нямате налично, моля въведете 0");

                }
            }


            peppersTextBox.Clear();
            tomatoesTextBox.Clear();
            carrotsTextBox.Clear();
            saltTextBox.Clear();
            sugarTextBox.Clear();
            eggplantTextBox.Clear();
            garlicTextBox.Clear();
            onionTextBox.Clear();


            Dictionary<string, double> limitingFactorForLiutenitsa = LimitingFactorForLiutenitsa(jarOfLutenitsa.Peppers,
                jarOfLutenitsa.Tomatoes, jarOfLutenitsa.Carrots, jarOfLutenitsa.Salt, jarOfLutenitsa.Suggar, peppersQuantity,
                tomatoesQuantity, carrotsQuantity, saltQuantity, sugarQuantity);

            Dictionary<string, double> limitingFactorForKiopolu = LimitingFactorForKiopolu(jarOfKiopolu.Peppers,
                jarOfKiopolu.Tomatoes, jarOfKiopolu.Eggplant, jarOfKiopolu.Salt, jarOfKiopolu.Onion, jarOfKiopolu.Garlic,
                peppersQuantity, tomatoesQuantity, eggplantQuantity, garlicQuantity, onionQuantity, saltQuantity);

            Dictionary<string, double> commonLimitingFactor = CommonLimitingFactor(jarOfLutenitsa.Peppers,
                jarOfLutenitsa.Tomatoes, jarOfLutenitsa.Carrots, jarOfLutenitsa.Salt, jarOfLutenitsa.Suggar,
                jarOfKiopolu.Peppers, jarOfKiopolu.Tomatoes, jarOfKiopolu.Eggplant, jarOfKiopolu.Salt, jarOfKiopolu.Onion,
                jarOfKiopolu.Garlic, peppersQuantity, tomatoesQuantity, carrotsQuantity, eggplantQuantity, garlicQuantity,
                onionQuantity, saltQuantity, sugarQuantity);


            double minJarsOfLiutenitsa = Math.Floor(limitingFactorForLiutenitsa.Values.Min());
            double minJarsOfKiopolu = Math.Floor(limitingFactorForKiopolu.Values.Min());


            double[] getNumberOfJars = GetNumberOfJars(jarOfLutenitsa.Peppers, jarOfLutenitsa.Tomatoes,
                jarOfLutenitsa.Salt, jarOfKiopolu.Peppers, jarOfKiopolu.Tomatoes, jarOfKiopolu.Salt,
                peppersQuantity, tomatoesQuantity, saltQuantity, totalJarsOfLiutenitsa, currJarsOfLiutenitsa,
                totalJarsOfKiopolu, currJarsOfKiopolu, totalJars, commonLimitingFactor,
                minJarsOfLiutenitsa, minJarsOfKiopolu);

            currJarsOfLiutenitsa = getNumberOfJars[0];
            totalJarsOfLiutenitsa = getNumberOfJars[1];
            currJarsOfKiopolu = getNumberOfJars[2];
            totalJarsOfKiopolu = getNumberOfJars[3];
            totalJars = getNumberOfJars[4];

            if (currJarsOfLiutenitsa == 0 && currJarsOfKiopolu == 0 )
            {
                MessageBox.Show($"Въведените количества от продуктите са недостатъчни. Следващите количества," +
                    $" които добавите ще бъдат прибавени към настоящите.");
            }

            if (totalJars > totalCapacity)
            {
                double overCapacity = totalJars - totalCapacity; 

                peppersQuantity -= currPeppersQuantity; 
                tomatoesQuantity -= currTomatoesQuantity;
                carrotsQuantity -= currCarrotsQuantity;
                saltQuantity -= currSaltQuantity;
                sugarQuantity -= currSugarQuantity;
                eggplantQuantity -= currEggplantQuantity;
                garlicQuantity -= currGarlicQuantity;
                onionQuantity -= currOnionQuantity;

                totalJarsOfLiutenitsa -= currJarsOfLiutenitsa;
                currJarsOfLiutenitsa = 0;
                totalJarsOfKiopolu -= currJarsOfKiopolu;
                currJarsOfKiopolu = 0;
                totalJars = totalJarsOfLiutenitsa + totalJarsOfKiopolu;

                                
                MessageBox.Show($"Максималният капацитет на мазето е {totalCapacity} бр. буркани по 800 гр. " +
                    $"Надминавате капацитета с {overCapacity} бр. Моля въведете по-малки количества.");

            }
            else
            {
                peppersQuantity -= (currJarsOfLiutenitsa * jarOfLutenitsa.Peppers) + 
                    (currJarsOfKiopolu * jarOfKiopolu.Peppers); 
                tomatoesQuantity -= (currJarsOfLiutenitsa * jarOfLutenitsa.Tomatoes) + 
                    (currJarsOfKiopolu * jarOfKiopolu.Tomatoes);
                carrotsQuantity -= currJarsOfLiutenitsa * jarOfLutenitsa.Carrots;
                saltQuantity -= (currJarsOfLiutenitsa * jarOfLutenitsa.Salt) + (currJarsOfKiopolu * jarOfKiopolu.Salt);
                sugarQuantity -= currJarsOfLiutenitsa * jarOfLutenitsa.Suggar;
                eggplantQuantity -= currJarsOfKiopolu * jarOfKiopolu.Eggplant;
                garlicQuantity -= currJarsOfKiopolu * jarOfKiopolu.Garlic;
                onionQuantity -= currJarsOfKiopolu * jarOfKiopolu.Onion;

                JarsOfLiutenitsa.Text = totalJarsOfLiutenitsa.ToString();
                JarsOfKiopolu.Text = totalJarsOfKiopolu.ToString();
                TotalJars.Text = totalJars.ToString();

                PeppersQuantity.Text = peppersQuantity.ToString();
                TomatoesQuantity.Text = tomatoesQuantity.ToString();
                CarrotsQuantity.Text = carrotsQuantity.ToString();
                SaltQuantity.Text = saltQuantity.ToString();
                SugarQuantity.Text = sugarQuantity.ToString();
                EggplantQuantity.Text = eggplantQuantity.ToString();
                GarlicQuantity.Text = garlicQuantity.ToString();
                OnionQuantity.Text = onionQuantity.ToString();
            }

        }


        public static double[] GetNumberOfJars(double pepersForLiutenitsa, double tomatoesForLiutenitsa,
            double saltForLiutenitsa, double pepersForKiopolu, double tomatoesForKiopolu, double saltForKiopolu,
            double peppersQuantity, double tomatoesQuantity, double saltQuantity, double totalJarsOfLiutenitsa,
            double currJarsOfLiutenitsa, double totalJarsOfKiopolu, double currJarsOfKiopolu, double totalJars,
            Dictionary<string, double> commonLimitingFactor, double minJarsOfLiutenitsa, double minJarsOfKiopolu)
        {
            double usedQuantity = 0;
            double remainingQuantity = 0;


            if (commonLimitingFactor.ContainsKey("jarsFromPeppers"))
            {
                usedQuantity = Math.Floor(pepersForKiopolu * minJarsOfKiopolu);
                remainingQuantity = Math.Floor(peppersQuantity - usedQuantity);
                currJarsOfLiutenitsa = Math.Floor(remainingQuantity / pepersForLiutenitsa);
                totalJarsOfLiutenitsa += currJarsOfLiutenitsa;
                currJarsOfKiopolu = minJarsOfKiopolu;
                totalJarsOfKiopolu += currJarsOfKiopolu;
                totalJars += Math.Floor(currJarsOfLiutenitsa + currJarsOfKiopolu);

            }
            else if (commonLimitingFactor.ContainsKey("jarsFromTomatoes"))
            {
                usedQuantity = Math.Floor(tomatoesForKiopolu * minJarsOfKiopolu);
                remainingQuantity = Math.Floor(tomatoesQuantity - usedQuantity);
                currJarsOfLiutenitsa = Math.Floor(remainingQuantity / tomatoesForLiutenitsa);
                totalJarsOfLiutenitsa += currJarsOfLiutenitsa;
                currJarsOfKiopolu = minJarsOfKiopolu;
                totalJarsOfKiopolu += currJarsOfKiopolu;
                totalJars += Math.Floor(currJarsOfLiutenitsa + currJarsOfKiopolu);
            }
            else if (commonLimitingFactor.ContainsKey("jarsFromSalt"))
            {
                usedQuantity = Math.Floor(saltForKiopolu * minJarsOfKiopolu);
                remainingQuantity = Math.Floor(saltQuantity - usedQuantity);
                currJarsOfLiutenitsa = Math.Floor(remainingQuantity / saltForLiutenitsa);
                totalJarsOfLiutenitsa += currJarsOfLiutenitsa;
                currJarsOfKiopolu = minJarsOfKiopolu;
                totalJarsOfKiopolu += currJarsOfKiopolu;
                totalJars += Math.Floor(currJarsOfLiutenitsa + currJarsOfKiopolu);
            }
            else
            {

                currJarsOfLiutenitsa = Math.Floor(minJarsOfLiutenitsa);
                totalJarsOfLiutenitsa += currJarsOfLiutenitsa;
                currJarsOfKiopolu = Math.Floor(minJarsOfKiopolu);
                totalJarsOfKiopolu += currJarsOfKiopolu;
                totalJars += Math.Floor(currJarsOfLiutenitsa + currJarsOfKiopolu);

            }

            double[] numberOfJars = new double[] { currJarsOfLiutenitsa, totalJarsOfLiutenitsa, 
                currJarsOfKiopolu, totalJarsOfKiopolu, totalJars };

            return numberOfJars;
        }



        public static Dictionary<string, double> CommonLimitingFactor(double pepersForLiutenitsa,
            double tomatoesForLiutenitsa, double carrotsForLiutenitsa, double saltForLiutenitsa,
            double sugarForLiutenitsa, double pepersForKiopolu, double tomatoesForKiopolu, double eggplantForKiopolu,
            double saltForKiopolu, double onionForKiopolu, double garlicForKiopolu, double peppersQuantity,
            double tomatoesQuantity, double carrotsQuantity, double eggplantQuantity, double garlicQuantity,
            double onionQuantity, double saltQuantity, double sugarQuantity)
        {
            double jarsFromPeppers = peppersQuantity / (pepersForLiutenitsa + pepersForKiopolu);
            double jarsFromTomatoes = tomatoesQuantity / (tomatoesForLiutenitsa + tomatoesForKiopolu);
            double jarsFromCarrots = carrotsQuantity / carrotsForLiutenitsa;
            double jarsFromSalt = saltQuantity / (saltForLiutenitsa + saltForKiopolu);
            double jarsFromSugar = sugarQuantity / sugarForLiutenitsa;
            double jarsFromEggplants = eggplantQuantity / eggplantForKiopolu;
            double jarsFromGarlic = garlicQuantity / garlicForKiopolu;
            double jarsFromOnion = onionQuantity / onionForKiopolu;

            Dictionary<string, double> minJarCount = new Dictionary<string, double>();

            minJarCount.Add("jarsFromPeppers", jarsFromPeppers);
            minJarCount.Add("jarsFromTomatoes", jarsFromTomatoes);
            minJarCount.Add("jarsFromCarrots", jarsFromCarrots);
            minJarCount.Add("jarsFromSalt", jarsFromSalt);
            minJarCount.Add("jarsFromSugar", jarsFromSugar);
            minJarCount.Add("jarsFromEggplants", jarsFromEggplants);
            minJarCount.Add("jarsFromGarlic", jarsFromGarlic);
            minJarCount.Add("jarsFromOnion", jarsFromOnion);

            string minCountProduct = minJarCount.Reverse().Aggregate((l, r) => l.Value < r.Value ? l : r).Key; 
            double minCount = minJarCount.Values.Min();

            Dictionary<string, double> minProduct = new Dictionary<string, double>();
            minProduct.Add(minCountProduct, minCount);

            return minProduct;

        }

        public static Dictionary<string, double> LimitingFactorForKiopolu(double pepersForKiopolu,
            double tomatoesForKiopolu, double eggplantForKiopolu, double saltForKiopolu, double onionForKiopolu,
            double garlicForKiopolu, double peppersQuantity, double tomatoesQuantity, double eggplantQuantity,
            double garlicQuantity, double onionQuantity, double saltQuantity)
        {
            double jarsFromPeppers = peppersQuantity / pepersForKiopolu;
            double jarsFromTomatoes = tomatoesQuantity / tomatoesForKiopolu;
            double jarsFromSalt = saltQuantity / saltForKiopolu;
            double jarsFromEggplants = eggplantQuantity / eggplantForKiopolu;
            double jarsFromGarlic = garlicQuantity / garlicForKiopolu;
            double jarsFromOnion = onionQuantity / onionForKiopolu;

            Dictionary<string, double> minJarCount = new Dictionary<string, double>();

            minJarCount.Add("jarsFromPeppers", jarsFromPeppers);
            minJarCount.Add("jarsFromTomatoes", jarsFromTomatoes);
            minJarCount.Add("jarsFromSalt", jarsFromSalt);
            minJarCount.Add("jarsFromEggplants", jarsFromEggplants);
            minJarCount.Add("jarsFromGarlic", jarsFromGarlic);
            minJarCount.Add("jarsFromOnion", jarsFromOnion);


            string minCountProduct = minJarCount.Reverse().Aggregate((l, r) => l.Value < r.Value ? l : r).Key; ;
            double minCount = minJarCount.Values.Min();

            Dictionary<string, double> minProduct = new Dictionary<string, double>();
            minProduct.Add(minCountProduct, minCount);

            return minProduct;
        }

        public static Dictionary<string, double> LimitingFactorForLiutenitsa(double pepersForLiutenitsa,
            double tomatoesForLiutenitsa, double carrotsForLiutenitsa, double saltForLiutenitsa,
            double sugarForLiutenitsa, double peppersQuantity, double tomatoesQuantity,
            double carrotsQuantity, double saltQuantity, double sugarQuantity)
        {
            double jarsFromPeppers = peppersQuantity / pepersForLiutenitsa;
            double jarsFromTomatoes = tomatoesQuantity / tomatoesForLiutenitsa;
            double jarsFromCarrots = carrotsQuantity / carrotsForLiutenitsa;
            double jarsFromSalt = saltQuantity / saltForLiutenitsa;
            double jarsFromSugar = sugarQuantity / sugarForLiutenitsa;

            Dictionary<string, double> minJarCount = new Dictionary<string, double>();

            minJarCount.Add("jarsFromPeppers", jarsFromPeppers);
            minJarCount.Add("jarsFromTomatoes", jarsFromTomatoes);
            minJarCount.Add("jarsFromCarrots", jarsFromCarrots);
            minJarCount.Add("jarsFromSalt", jarsFromSalt);
            minJarCount.Add("jarsFromSugar", jarsFromSugar);

            string minCountProduct = minJarCount.Reverse().Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            double minCount = minJarCount.Values.Min();

            Dictionary<string, double> minProduct = new Dictionary<string, double>();
            minProduct.Add(minCountProduct, minCount);

            return minProduct;
        }
                

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
