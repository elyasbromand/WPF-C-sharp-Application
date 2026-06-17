using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFZooApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Models.CSharpMasterclassContext? db;
        Dictionary<int, string>? zoos;
        Dictionary<int, string>? associated_animals;
        Dictionary<int, string>? animals;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            db = new Models.CSharpMasterclassContext();
            zoos = db.Zoos.ToDictionary(
                z => z.Id, //Key
                z => z.Location //Value
            );
            animals = db.Animals.ToDictionary(
                a => a.Id, //Key
                a => a.Name //Value
            );
            showZoos();
            showAnimals();
        }

        private void RefreshData()
        {
            zoos = db!.Zoos.ToDictionary(z => z.Id, z => z.Location);

            animals = db!.Animals.ToDictionary(a => a.Id, a => a.Name);

            showZoos();
            showAnimals();

            if (ListZoos.SelectedValue != null)
            {
                FetchAssocAnimals();
                showAssocAnimals();
            }
        }

        private void FetchAssocAnimals()
        {
            try
            {
                associated_animals = db!
                    .ZooAnimals.Where(za => za.ZooId == (int)ListZoos.SelectedValue)
                    .Join(db.Animals, za => za.AnimalId, a => a.Id, (za, a) => new { a.Id, a.Name })
                    .ToDictionary(x => x.Id, x => x.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showZoos()
        {
            ListZoos.ItemsSource = zoos;
            ListZoos.DisplayMemberPath = "Value";
            ListZoos.SelectedValuePath = "Key";
        }

        private void showAnimals()
        {
            ListAnimals.ItemsSource = animals;
            ListAnimals.DisplayMemberPath = "Value";
            ListAnimals.SelectedValuePath = "Key";
        }

        private void showAssocAnimals()
        {
            ListAssocAnimals.ItemsSource = associated_animals;
            ListAssocAnimals.DisplayMemberPath = "Value";
            ListAssocAnimals.SelectedValuePath = "Key";
        }

        private void ListAssocAnimals_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        ) { }

        private void ListZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListZoos.SelectedValue == null)
                return;

            TxtInput.Text = zoos![(int)ListZoos.SelectedValue];

            FetchAssocAnimals();
            showAssocAnimals();
        }

        private void ListAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListAnimals.SelectedValue == null)
                return;

            TxtInput.Text = animals![(int)ListAnimals.SelectedValue];
        }

        //CRUD Operations
        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtInput.Text))
                    return;

                db!.Zoos.Add(new Models.Zoo() { Location = TxtInput.Text });

                db!.SaveChanges();

                RefreshData();

                TxtInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListZoos.SelectedValue == null)
                    return;

                int zooId = (int)ListZoos.SelectedValue;

                var zoo = db!.Zoos.Find(zooId);

                if (zoo != null)
                    db.Zoos.Remove(zoo);

                db.SaveChanges();

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListZoos.SelectedValue == null)
                    return;

                var zoo = db!.Zoos.FirstOrDefault(z => z.Id == (int)ListZoos.SelectedValue);

                if (zoo == null)
                    return;

                zoo.Location = TxtInput.Text;

                db.SaveChanges();

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtInput.Text))
                    return;

                db!.Animals.Add(new Models.Animal() { Name = TxtInput.Text });

                db.SaveChanges();

                RefreshData();

                TxtInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListAnimals.SelectedValue == null)
                    return;

                int animalId = (int)ListAnimals.SelectedValue;

                var animal = db!.Animals.Find(animalId);

                if (animal != null)
                    db.Animals.Remove(animal);

                db.SaveChanges();

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListAnimals.SelectedValue == null)
                    return;

                var animal = db!.Animals.FirstOrDefault(a =>
                    a.Id == (int)ListAnimals.SelectedValue
                );

                if (animal == null)
                    return;

                animal.Name = TxtInput.Text;

                db.SaveChanges();

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddToAnimalOfTheZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListZoos.SelectedValue == null || ListAnimals.SelectedValue == null)
                    return;

                int zooId = (int)ListZoos.SelectedValue;

                int animalId = (int)ListAnimals.SelectedValue;

                bool exists = db!.ZooAnimals.Any(x => x.ZooId == zooId && x.AnimalId == animalId);

                if (exists)
                {
                    MessageBox.Show("Already added.");
                    return;
                }

                db.ZooAnimals.Add(new Models.ZooAnimal() { ZooId = zooId, AnimalId = animalId });

                db.SaveChanges();

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListZoos.SelectedValue == null || ListAssocAnimals.SelectedValue == null)
                    return;

                int zooId = (int)ListZoos.SelectedValue;

                int animalId = (int)ListAssocAnimals.SelectedValue;

                var relation = db!.ZooAnimals.FirstOrDefault(x =>
                    x.ZooId == zooId && x.AnimalId == animalId
                );

                if (relation != null)
                {
                    db.ZooAnimals.Remove(relation);

                    db.SaveChanges();

                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
