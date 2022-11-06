using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public double TotalCapacity { get; set; } = 200;

        public double CurrPeppersQuantity { get; set; }
        public double CurrTomatoesQuantity { get; set; }
        public double CurrCarrotsQuantity { get; set; }
        public double CurrSaltQuantity { get; set; }
        public double CurrSugarQuantity { get; set; }
        public double CurrEggplantQuantity { get; set; }
        public double CurrGarlicQuantity { get; set; }
        public double CurrOnionQuantity { get; set; }

        public double TotalPeppersQuantity { get; set; }
        public double TotalTomatoesQuantity { get; set; }
        public double TotalCarrotsQuantity { get; set; }
        public double TotalSaltQuantity { get; set; }
        public double TotalSugarQuantity { get; set; }
        public double TotalEggplantQuantity { get; set; }
        public double TotalGarlicQuantity { get; set; }
        public double TotalOnionQuantity { get; set; }

        public double TotalJarsOfLiutenitsa { get; set; }
        public double CurrJarsOfLiutenitsa { get; set; }
        public double TotalJarsOfKiopolu { get; set; }
        public double CurrJarsOfKiopolu { get; set; }
        public double TotalJarsLutenitsaAndKiopolu { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadLastSession();
        }

        private void LoadLastSession()
        {
            var exists = File.Exists(@"..\..\..\productsAndJars.json");

            if (exists)
            {
                var lastSessionString = File.ReadAllText(@"..\..\..\productsAndJars.json");
                var lastSession = JsonConvert.DeserializeObject<ProductsAndJars>(lastSessionString);

                JarsOfLiutenitsa.Text = lastSession?.JarsOfLutenitsa.ToString();
                JarsOfKiopolu.Text = lastSession?.JarsOfKiopolu.ToString();
                TotalJars.Text = (lastSession?.JarsOfLutenitsa + lastSession?.JarsOfKiopolu).ToString();

                PeppersQuantity.Text = lastSession?.Peppers.ToString();
                TomatoesQuantity.Text = lastSession?.Tomatoes.ToString();
                CarrotsQuantity.Text = lastSession?.Carrots.ToString();
                SaltQuantity.Text = lastSession?.Salt.ToString();
                SugarQuantity.Text = lastSession?.Suggar.ToString();
                EggplantQuantity.Text = lastSession?.Eggplant.ToString();
                GarlicQuantity.Text = lastSession?.Garlic.ToString();
                OnionQuantity.Text = lastSession?.Onion.ToString();
            }
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
                onionTextBox.Text == String.Empty ||
                peppersTextBox.Text == "∞" ||
                tomatoesTextBox.Text == "∞" ||
                carrotsTextBox.Text == "∞" ||
                saltTextBox.Text == "∞" ||
                sugarTextBox.Text == "∞" ||
                eggplantTextBox.Text == "∞" ||
                garlicTextBox.Text == "∞" ||
                onionTextBox.Text == "∞")
            {
                MessageBox.Show("Въведените данни са невалидни. Необходимо е да въведете количество " +
                    "за всеки продукт. Ако нямате налично, моля въведете 0");
                return;
            }
            else
            {
                try
                {
                    CurrPeppersQuantity = double.Parse(peppersTextBox.Text);
                    CurrTomatoesQuantity = double.Parse(tomatoesTextBox.Text);
                    CurrCarrotsQuantity = double.Parse(carrotsTextBox.Text);
                    CurrSaltQuantity = double.Parse(saltTextBox.Text);
                    CurrSugarQuantity = double.Parse(sugarTextBox.Text);
                    CurrEggplantQuantity = double.Parse(eggplantTextBox.Text);
                    CurrGarlicQuantity = double.Parse(garlicTextBox.Text);
                    CurrOnionQuantity = double.Parse(onionTextBox.Text);

                    TotalPeppersQuantity += CurrPeppersQuantity;
                    TotalTomatoesQuantity += CurrTomatoesQuantity;
                    TotalCarrotsQuantity += CurrCarrotsQuantity;
                    TotalSaltQuantity += CurrSaltQuantity;
                    TotalSugarQuantity += CurrSugarQuantity;
                    TotalEggplantQuantity += CurrEggplantQuantity;
                    TotalGarlicQuantity += CurrGarlicQuantity;
                    TotalOnionQuantity += CurrOnionQuantity;
                }
                catch
                {
                    MessageBox.Show("Въведените данни са невалидни. Необходимо е да въведете количество " +
                    "за всеки продукт. Ако нямате налично, моля въведете 0");
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
                jarOfLutenitsa.Tomatoes, jarOfLutenitsa.Carrots, jarOfLutenitsa.Salt, jarOfLutenitsa.Suggar, TotalPeppersQuantity,
                TotalTomatoesQuantity, TotalCarrotsQuantity, TotalSaltQuantity, TotalSugarQuantity);

            Dictionary<string, double> limitingFactorForKiopolu = LimitingFactorForKiopolu(jarOfKiopolu.Peppers,
                jarOfKiopolu.Tomatoes, jarOfKiopolu.Eggplant, jarOfKiopolu.Salt, jarOfKiopolu.Onion, jarOfKiopolu.Garlic,
                TotalPeppersQuantity, TotalTomatoesQuantity, TotalEggplantQuantity, TotalGarlicQuantity, TotalOnionQuantity, TotalSaltQuantity);

            Dictionary<string, double> commonLimitingFactor = CommonLimitingFactor(jarOfLutenitsa.Peppers,
                jarOfLutenitsa.Tomatoes, jarOfLutenitsa.Carrots, jarOfLutenitsa.Salt, jarOfLutenitsa.Suggar,
                jarOfKiopolu.Peppers, jarOfKiopolu.Tomatoes, jarOfKiopolu.Eggplant, jarOfKiopolu.Salt, jarOfKiopolu.Onion,
                jarOfKiopolu.Garlic, TotalPeppersQuantity, TotalTomatoesQuantity, TotalCarrotsQuantity, TotalEggplantQuantity, TotalGarlicQuantity,
                TotalOnionQuantity, TotalSaltQuantity, TotalSugarQuantity);


            double minJarsOfLiutenitsa = Math.Floor(limitingFactorForLiutenitsa.Values.Min());
            double minJarsOfKiopolu = Math.Floor(limitingFactorForKiopolu.Values.Min());

            double[] getNumberOfJars = GetNumberOfJars(jarOfLutenitsa.Peppers, jarOfLutenitsa.Tomatoes,
                jarOfLutenitsa.Salt, jarOfKiopolu.Peppers, jarOfKiopolu.Tomatoes, jarOfKiopolu.Salt,
                TotalPeppersQuantity, TotalTomatoesQuantity, TotalSaltQuantity, TotalJarsOfLiutenitsa, CurrJarsOfLiutenitsa,
                TotalJarsOfKiopolu, CurrJarsOfKiopolu, TotalJarsLutenitsaAndKiopolu, commonLimitingFactor,
                minJarsOfLiutenitsa, minJarsOfKiopolu);

            CurrJarsOfLiutenitsa = getNumberOfJars[0];
            TotalJarsOfLiutenitsa = getNumberOfJars[1];
            CurrJarsOfKiopolu = getNumberOfJars[2];
            TotalJarsOfKiopolu = getNumberOfJars[3];
            TotalJarsLutenitsaAndKiopolu = getNumberOfJars[4];

            if (CurrJarsOfLiutenitsa == 0 && CurrJarsOfKiopolu == 0)
            {
                MessageBox.Show($"Въведените количества от продуктите са недостатъчни. Следващите количества," +
                    $" които добавите ще бъдат прибавени към настоящите.");               
            }

            if (TotalJarsLutenitsaAndKiopolu > TotalCapacity)
            {
                double overCapacity = TotalJarsLutenitsaAndKiopolu - TotalCapacity;

                TotalPeppersQuantity -= CurrPeppersQuantity;
                TotalTomatoesQuantity -= CurrTomatoesQuantity;
                TotalCarrotsQuantity -= CurrCarrotsQuantity;
                TotalSaltQuantity -= CurrSaltQuantity;
                TotalSugarQuantity -= CurrSugarQuantity;
                TotalEggplantQuantity -= CurrEggplantQuantity;
                TotalGarlicQuantity -= CurrGarlicQuantity;
                TotalOnionQuantity -= CurrOnionQuantity;

                TotalJarsOfLiutenitsa -= CurrJarsOfLiutenitsa;
                CurrJarsOfLiutenitsa = 0;
                TotalJarsOfKiopolu -= CurrJarsOfKiopolu;
                CurrJarsOfKiopolu = 0;
                TotalJarsLutenitsaAndKiopolu = TotalJarsOfLiutenitsa + TotalJarsOfKiopolu;

                MessageBox.Show($"Максималният капацитет на мазето е {TotalCapacity} бр. буркани по 800 гр. " +
                    $"Надминавате капацитета с {overCapacity} бр. Моля въведете по-малки количества.");
            }
            else
            {
                TotalPeppersQuantity -= (CurrJarsOfLiutenitsa * jarOfLutenitsa.Peppers) +
                    (CurrJarsOfKiopolu * jarOfKiopolu.Peppers);
                TotalTomatoesQuantity -= (CurrJarsOfLiutenitsa * jarOfLutenitsa.Tomatoes) +
                    (CurrJarsOfKiopolu * jarOfKiopolu.Tomatoes);
                TotalCarrotsQuantity -= CurrJarsOfLiutenitsa * jarOfLutenitsa.Carrots;
                TotalSaltQuantity -= (CurrJarsOfLiutenitsa * jarOfLutenitsa.Salt) + (CurrJarsOfKiopolu * jarOfKiopolu.Salt);
                TotalSugarQuantity -= CurrJarsOfLiutenitsa * jarOfLutenitsa.Suggar;
                TotalEggplantQuantity -= CurrJarsOfKiopolu * jarOfKiopolu.Eggplant;
                TotalGarlicQuantity -= CurrJarsOfKiopolu * jarOfKiopolu.Garlic;
                TotalOnionQuantity -= CurrJarsOfKiopolu * jarOfKiopolu.Onion;

                JarsOfLiutenitsa.Text = TotalJarsOfLiutenitsa.ToString();
                JarsOfKiopolu.Text = TotalJarsOfKiopolu.ToString();
                TotalJars.Text = TotalJarsLutenitsaAndKiopolu.ToString();

                PeppersQuantity.Text = TotalPeppersQuantity.ToString();
                TomatoesQuantity.Text = TotalTomatoesQuantity.ToString();
                CarrotsQuantity.Text = TotalCarrotsQuantity.ToString();
                SaltQuantity.Text = TotalSaltQuantity.ToString();
                SugarQuantity.Text = TotalSugarQuantity.ToString();
                EggplantQuantity.Text = TotalEggplantQuantity.ToString();
                GarlicQuantity.Text = TotalGarlicQuantity.ToString();
                OnionQuantity.Text = TotalOnionQuantity.ToString();
            }

            var productsAndJars = new ProductsAndJars()
            {
                Peppers = TotalPeppersQuantity,
                Tomatoes = TotalTomatoesQuantity,
                Carrots = TotalCarrotsQuantity,
                Salt = TotalSaltQuantity,
                Suggar = TotalSugarQuantity,
                Eggplant = TotalEggplantQuantity,
                Garlic = TotalGarlicQuantity,
                Onion = TotalOnionQuantity,
                JarsOfLutenitsa = TotalJarsOfLiutenitsa,
                JarsOfKiopolu = TotalJarsOfKiopolu,
            };

            var productsAndJarsString = JsonConvert.SerializeObject(productsAndJars, Formatting.Indented);

            File.WriteAllText(@"..\..\..\productsAndJars.json", productsAndJarsString);
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

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            var exists = File.Exists(@"..\..\..\productsAndJars.json");

            if (exists)
            {
                File.Delete(@"..\..\..\productsAndJars.json");

                JarsOfLiutenitsa.Text = "";
                JarsOfKiopolu.Text = "";
                TotalJars.Text = "";

                PeppersQuantity.Text = "";
                TomatoesQuantity.Text = "";
                CarrotsQuantity.Text = "";
                SaltQuantity.Text = "";
                SugarQuantity.Text = "";
                EggplantQuantity.Text = "";
                GarlicQuantity.Text = "";
                OnionQuantity.Text = "";

                TotalJarsOfLiutenitsa = 0;
                TotalJarsOfKiopolu = 0;
                TotalJarsLutenitsaAndKiopolu = 0;

                TotalPeppersQuantity = 0;
                TotalTomatoesQuantity = 0;
                TotalCarrotsQuantity = 0;
                TotalSaltQuantity = 0;
                TotalSugarQuantity = 0;
                TotalEggplantQuantity = 0;
                TotalGarlicQuantity = 0;
                TotalOnionQuantity = 0;
            }
        }
    }
}
