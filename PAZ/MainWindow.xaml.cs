using System;
using System.Collections.Generic;
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
using System.ComponentModel;

namespace PAZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    /// 


    public partial class MainWindow : Window
    {
        private List<Zitting> _master;
        public ICollectionView Zittingen{get; private set;}
        public MainWindow()
        {
            InitializeComponent();

            _master = new List<Zitting>
            {
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "13:30",
                    Lokaal = "OB002",
                    Leerlingen = "Piet Jan \nJan Piet",
                    Docenten = "Ger Saris \nKeesjan hogenboom",
                    Deskundige = "Ad Groot 2 \nAad Klein",
                    AantalGasten = 12
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:00",
                    Lokaal = "OB002",
                    Leerlingen = "Ibrahim Boven\nJeroen Schipper",
                    Docenten = "Freek Hogenboom\nSjaak Lauris",
                    Deskundige = "Kees Prof 2 \n Piet Hogensluiter",
                    AantalGasten = 4
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:30",
                    Lokaal = "OB002",
                    Leerlingen = "Freek Netes\nMark Hos",
                    Docenten = "Bruno Marks\nMandy Tregis",
                    Deskundige = "Kelly Bruins\nPatricia Kaai",
                    AantalGasten = 6
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "13:30",
                    Lokaal = "OB002",
                    Leerlingen = "Piet Jan \nJan Piet",
                    Docenten = "Ger Saris \nKeesjan hogenboom",
                    Deskundige = "Ad Groot 2 \nAad Klein",
                    AantalGasten = 10
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:00",
                    Lokaal = "OB002",
                    Leerlingen = "Ibrahim Boven\nJeroen Schipper",
                    Docenten = "Freek Hogenboom\nSjaak Lauris",
                    Deskundige = "Kees Prof 2 \n Piet Hogensluiter",
                    AantalGasten = 11
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:30",
                    Lokaal = "OB002",
                    Leerlingen = "Freek Netes\nMark Hos",
                    Docenten = "Bruno Marks\nMandy Tregis",
                    Deskundige = "Kelly Bruins\nPatricia Kaai",
                    AantalGasten = 8
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "13:30",
                    Lokaal = "OB002",
                    Leerlingen = "Piet Jan \nJan Piet",
                    Docenten = "Ger Saris \nKeesjan hogenboom",
                    Deskundige = "Ad Groot 2 \nAad Klein",
                    AantalGasten = 12
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:00",
                    Lokaal = "OB002",
                    Leerlingen = "Ibrahim Boven\nJeroen Schipper",
                    Docenten = "Freek Hogenboom\nSjaak Lauris",
                    Deskundige = "Kees Prof 2 \n Piet Hogensluiter",
                    AantalGasten = 3
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:30",
                    Lokaal = "OB002",
                    Leerlingen = "Freek Netes\nMark Hos",
                    Docenten = "Bruno Marks\nMandy Tregis",
                    Deskundige = "Kelly Bruins\nPatricia Kaai",
                    AantalGasten = 13
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "13:30",
                    Lokaal = "OB002",
                    Leerlingen = "Piet Jan \nJan Piet",
                    Docenten = "Ger Saris \nKeesjan hogenboom",
                    Deskundige = "Ad Groot 2 \nAad Klein",
                    AantalGasten = 7
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:00",
                    Lokaal = "OB002",
                    Leerlingen = "Ibrahim Boven\nJeroen Schipper",
                    Docenten = "Freek Hogenboom\nSjaak Lauris",
                    Deskundige = "Kees Prof 2 \n Piet Hogensluiter",
                    AantalGasten = 17
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:30",
                    Lokaal = "OB002",
                    Leerlingen = "Freek Netes\nMark Hos",
                    Docenten = "Bruno Marks\nMandy Tregis",
                    Deskundige = "Kelly Bruins\nPatricia Kaai",
                    AantalGasten = 3
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "13:30",
                    Lokaal = "OB002",
                    Leerlingen = "Piet Jan \nJan Piet",
                    Docenten = "Ger Saris \nKeesjan hogenboom",
                    Deskundige = "Ad Groot 2 \nAad Klein",
                    AantalGasten = 9
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:00",
                    Lokaal = "OB002",
                    Leerlingen = "Ibrahim Boven\nJeroen Schipper",
                    Docenten = "Freek Hogenboom\nSjaak Lauris",
                    Deskundige = "Kees Prof 2 \n Piet Hogensluiter",
                    AantalGasten = 8
                },
                new Zitting
                {
                    Datum = "10-5-2011",
                    Tijd = "14:30",
                    Lokaal = "OB002",
                    Leerlingen = "Freek Netes\nMark Hos",
                    Docenten = "Bruno Marks\nMandy Tregis",
                    Deskundige = "Kelly Bruins\nPatricia Kaai",
                    AantalGasten = 8
                }

            };

            Zittingen = CollectionViewSource.GetDefaultView(_master);
            bitch.ItemsSource = Zittingen;
        }

        private void buttonExportPDF_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u het rooster wilt omzetten naar een pdf bestand?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Het PDF-bestand is gemaakt.", "PDF-bestand gemaakt");
            }
        }

        private void buttonVerwijderGebruikers_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u alle gebruikers wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Alle gebruikers zijn verwijderd.", "Gebruikers verwijderd");
            }
        }

        private void buttonVerwijderLokalen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Weet u zeker dat u alle lokalen wilt verwijderen? \n\nLet op: deze actie kan niet ongedaan worden.", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Alle lokalen zijn verwijderd.", "Lokalen verwijderd");
            }
        }

        private void comboBoxSelecteerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSelecteerType.SelectedIndex > 0)
            {
                verbergAlleToevoegGroupBoxs();

                switch (comboBoxSelecteerType.SelectedIndex)
                {
                    case 1: groupBoxLokaalGegevens.Visibility = Visibility.Visible; break;
                    case 2: groupBoxGebruikerGegevens.Visibility = Visibility.Visible; break;
                    case 3: groupBoxBegeleiderGegevens.Visibility = Visibility.Visible; break;
                    case 4: groupBoxExternGegevens.Visibility = Visibility.Visible; break;
                    case 5: groupBoxLeraarGegevens.Visibility = Visibility.Visible; break;
                }
            }
        }

        private void verbergAlleToevoegGroupBoxs()
        {
            groupBoxLokaalGegevens.Visibility = Visibility.Hidden;
            groupBoxGebruikerGegevens.Visibility = Visibility.Hidden;
            groupBoxBegeleiderGegevens.Visibility = Visibility.Hidden;
            groupBoxExternGegevens.Visibility = Visibility.Hidden;
            groupBoxLeraarGegevens.Visibility = Visibility.Hidden;
        }

        private void Agree(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Aanmelding goedkeuren?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol", "Succesvol");
            }
        }

        private void Decline(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Aanmelding afkeuren?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Aanmelding is afgekeurt.", "Succesvol");
            }
        }

        private void buttonEmailVersturen_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Emails versturen?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Emails zijn verzonden.", "Succesvol");
            }
        }

        private void buttonBriefPrinten_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Brief uitprinten?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Succesvol. Brief is uitgeprint.", "Succesvol");
            }
        }

        private void bitch_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (MessageBox.Show("Wilt u de wijzigingen opslaan?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen zijn opgeslagen.", "Succesvol");
            }
        }






    }
}
