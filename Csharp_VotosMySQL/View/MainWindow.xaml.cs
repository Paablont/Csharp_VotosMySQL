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

namespace Csharp_VotosMySQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Invocamos el modelo y lo asignamos a DataContext
        private PartyModelView model = new PartyModelView();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
            //Cargamos los datos existentes en la BDD
            model.LoadParties();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (model.parties == null) model.parties = new ObservableCollection<Party>();
            //Si el registro no existe, procedemos a crearlo
            if (model.parties.Where(x => x.name == model.name).FirstOrDefault() == null)
            {
                model.parties.Add(new Party
                {
                    name = model.name,
                    acronym = model.acronym,
                    presidentName = model.presidentName,
                    votesParty = model.votesParty,
                    seat = model.seat
                }) ;
                //una vez agregado el registro al modelo, lo agregamos a la BDD
                model.newParty();
            }
            //Si el registro ya existe, debemos actualizarlo
            else
            {
                foreach (Party r in model.parties)
                {
                    if (r.name.Equals(model.name))
                    {
                        r.acronym = model.acronym;
                        r.presidentName = model.presidentName;
                        r.votesParty = model.votesParty;
                        r.seat = model.seat;
                        break;
                    }
                }

                //Actualizamos
                model.UpdateParty();
            }
        }
    }
}
