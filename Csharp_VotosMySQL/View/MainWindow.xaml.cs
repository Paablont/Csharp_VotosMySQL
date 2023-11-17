using Csharp_VotosMySQL.Model;
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
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

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
                    p.votePartyAux = 0;
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
                MessageBox.Show("10 parties have been added to the database. The simulation will begin now: ");
                tbControlMenu.SelectedIndex = 2;
                tabItem3.IsEnabled = true;
            }

            if (model.parties == null) model.parties = new ObservableCollection<Parties>();

            // Si el registro no existe, procedemos a crearlo
            if (model.parties.Where(x => x.nameParty == model.name).FirstOrDefault() == null)
            {
                // Crear un nuevo objeto Parties utilizando el constructor
                model.parties.Add(new Parties
                {
                    nameParty = model.name,
                    acronymParty = model.acronym,
                    presidentParty = model.presidentName,
                    voteParty = model.votesParty,
                    votePartyAux = model.votesPartyAux,
                    seatCount = model.seatCount
                });

                // Agregar el nuevo partido a la colección y a la base de datos
                
                model.newParty();
            }
            // Si el registro ya existe, debemos actualizarlo
            else
            {
                foreach (Parties r in model.parties)
                {
                    if (r.nameParty.Equals(model.name))
                    {
                        r.nameParty = tbxPartyName.Text;
                        r.acronymParty = tbxAcronym.Text;
                        r.presidentParty = tbxPresidentName.Text;
                        r.voteParty = 0;
                        r.seatCount = 0;
                    }
                }

                // Actualizamos
                model.UpdateParty();
            }
        }



        //Button that delete a new party to the datagrid
        private void btnDeleteParty_Click(object sender, RoutedEventArgs e)
        {
            // Obtén el elemento seleccionado en el DataGrid
            var selectedParty = dvgParties.SelectedItem as Parties;

            if (selectedParty != null)
            {
                // Elimina el elemento seleccionado del modelo y de la base de datos
                model.DeleteParty(selectedParty.nameParty);
            }

        }

        //*************** THIRD TAB FUNCTIONS *****************

        //Start simulation button
        //*************** THIRD TAB FUNCTIONS *****************

        //Start simulation button
        private void startSimulation(object sender, RoutedEventArgs e)
        {

            try
            {
                seatString = tbxSeats.Text;
                seatsNumber = int.Parse(seatString);
                

                if (seatsNumber <= 0)
                {
                    MessageBox.Show("The value of seats can not be less or equals to 0");


                }
                else
                {
                    dvgVotos.ItemsSource = model.parties;
                    dvgVotos.Items.Refresh();
                    model.calculateVotesParty(votesValid, model.parties);
                    //model.calculateStands(model.parties, seatsNumber);

                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The value of seats can not be alphabetic character or 0");
            }
        }




    }
}
