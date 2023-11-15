using Csharp_VotosMySQL.DDBB;
using Csharp_VotosMySQL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Csharp_VotosMySQL.ModelView
{
    class PartyModelView : INotifyPropertyChanged
    {
        #region VARIABLES

        //Declaro la constante para la conexión a la BDD
        private const String cnstr = "server=localhost;uid=pablo;pwd=pablo;database=votosddbb";
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Parties> _party;
        public String name { get; set; }
        public String acronym { get; set; }
        public String presidentName { get; set; }

        public int votesParty { get; set; }

        //I use this aux variable to calculate the seats for each party
        public int votesPartyAux { get; set; }
        public int seat { get; set; }

        public ObservableCollection<Parties> parties
        {
            get { return _party; }
            set
            {
                _party = value;
                OnPropertyChange("parties");
            }
        }
        #endregion


        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public void newParty()
        {
            String SQL = $"INSERT INTO partido (nombre, acronimo, nombrePresidente, votos, escanios)" +
                $" VALUES ('{name}','{acronym}', '{presidentName}',, '{votesParty}', '{seat}');";
            //usaremos las clases de la librería de MySQL para ejecutar queries
            //Instalar el paquete MySQL.Data
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void UpdateParty()
        {
            String SQL = $"UPDATE partido SET nombre = '{name}', acronimo = '{acronym}',nombreRepresentante = '{presidentName}'" +
                $",votos = '{votesParty}',escanios = '{seat}' WHERE nombre = '{name}';";
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void DeleteParty()
        {
            String SQL = $"DELETE FROM partido WHERE nombre = '{name}';";
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void LoadParties()
        {
            String SQL = $"SELECT nombre, votos, escanios FROM partido;";
            DataTable dt = MySQLDataComponent.LoadData(SQL, cnstr);
            if (dt.Rows.Count > 0)
            {
                if (name == null) parties = new ObservableCollection<Parties>();
                foreach (DataRow i in dt.Rows)
                {
                    parties.Add(new Parties
                    {
                        nameParty = i[0].ToString(),
                        acronymParty = i[0].ToString(),
                        presidentParty = i[0].ToString(),
                        voteParty = int.Parse(i[0].ToString()),
                        seatCount = int.Parse(i[0].ToString())
                    });
                }
            }
            dt.Dispose();
        }
    }
}
