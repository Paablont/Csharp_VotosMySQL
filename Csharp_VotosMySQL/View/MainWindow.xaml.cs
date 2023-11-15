﻿using Csharp_VotosMySQL.Model;
using Csharp_VotosMySQL.ModelView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Csharp_VotosMySQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Invocamos el modelo y lo asignamos a DataContext
        private PartyModelView model = new PartyModelView();
        Dates datesPre { get; set;  }
        Parties party;
        int peopleThatVote, votesAbst, votesNull, seatsNumber, votesValid;
        string absentString, nullString, seatString;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
            //Cargamos los datos existentes en la BDD
            model.LoadParties();
            
            datesPre = new Dates();
           
            dvgParties.ItemsSource = model.parties;
            
            Loaded += totalPopulationChange;

            //When the tbxAbsent  changes, tbxNull refresh with update null vote count
            tbxAbsent.TextChanged += nullVoteChange;

            //Disable delete button from the 2nd tab
            btnDeleteParty.Visibility = Visibility.Hidden;
        }
        //*************** TAB CONTROL FUNCTIONS *************** // 

        //When press tab 1 or tab 2, clear the data from the datagrid on tab 3
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (tbControlMenu.SelectedIndex == 0 || tbControlMenu.SelectedIndex == 1)
            {
                dvgVotos.ItemsSource = null;
                dvgVotos.Items.Refresh();
                tabItem3.IsEnabled = false;
                foreach (Parties p in model.parties)
                {
                    p.seatCount = 0;
                    p.votesPartyAux = 0;
                    p.voteParty = 0;
                }
            }
        }

        //*************** FIRST TAB FUNCTIONS *****************//

        //When click on Button saves data
        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            absentString = tbxAbsent.Text;
            nullString = tbxNull.Text;
            peopleThatVote = datesPre.calculatePeopleThatVote(absentString);
            votesAbst = int.Parse(tbxAbsent.Text);
            votesNull = datesPre.votesNullCalculate(absentString);
            votesValid = datesPre.votesValidCalculate(peopleThatVote, votesNull);
            datesPre.PeopleThatVote = peopleThatVote;
            datesPre.VotesAbst = votesAbst;
            datesPre.VotesNull = votesNull;

            if (datesPre.VotesAbst == 0)
            {
                MessageBox.Show("The absent votes can not be 0");
            }
            else
            {
                //When  you press the button change to the second tab
                MessageBox.Show("Data save properly");
                //MessageBox.Show(peopleThatVote.ToString());
                tbControlMenu.SelectedIndex = 1;
                tabItem2.IsEnabled = true;
            }


        }

        //Change the field in total Population 
        private void totalPopulationChange(object sender, RoutedEventArgs e)
        {
            tbxPopulation.Text = Dates.TOTALPOPULATION.ToString();
        }
        //Change the field in null votes
        private void nullVoteChange(object sender, RoutedEventArgs e)
        {
            absentString = tbxAbsent.Text;
            tbxNull.Text = datesPre.votesNullCalculate(absentString).ToString();
        }

        //*************** SECOND TAB FUNCTIONS *****************//

        //Select one field in the Datagrid
        private void dgvPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDeleteParty.Visibility = Visibility.Visible;

            if (dvgParties.SelectedItem == null)
            {
                btnDeleteParty.Visibility = Visibility.Hidden;

            }

        }



        //Button that add a new party to the datagrid
        private void btnSaveParty_Click(object sender, RoutedEventArgs e)
        {
            if (model.parties.Count == 10)
            {

            }
            if (model.parties == null) model.parties = new ObservableCollection<Parties>();
            //Si el registro no existe, procedemos a crearlo
            if (model.parties.Where(x => x.nameParty == model.name).FirstOrDefault() == null)
            {
                model.parties.Add(new Parties
                {
                    nameParty = tbxPartyName.Text,
                    acronymParty = tbxAcronym.Text,
                    presidentParty = tbxPresidentName.Text,
                    voteParty = 0,
                    seatCount = 0
                });
                //una vez agregado el registro al modelo, lo agregamos a la BDD
                model.newParty();
            }
            //Si el registro ya existe, debemos actualizarlo
            else
            {
                foreach (Parties r in model.parties)
                {
                    if (r.nameParty.Equals(model.name))
                    {
                        r.acronymParty = model.acronym;
                        r.presidentParty = model.presidentName;
                        r.voteParty = model.votesParty;
                        r.seatCount = model.seat;
                        break;
                    }
                }

                //Actualizamos
                model.UpdateParty();
            }

        }

        //Button that delete a new party to the datagrid
        private void btnDeleteParty_Click(object sender, RoutedEventArgs e)
        {
            

        }


       
    }
}
