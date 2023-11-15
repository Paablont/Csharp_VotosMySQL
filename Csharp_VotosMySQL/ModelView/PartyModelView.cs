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
using System.Xml.Linq;

namespace Csharp_VotosMySQL.ModelView
{
    class PartyModelView : INotifyPropertyChanged
    {
        #region VARIABLES

        //Declaro la constante para la conexión a la BDD
        private const String cnstr = "server=localhost;uid=pablo;pwd=pablo;database=votosddbb";
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Parties> _party;
        private String _name;
        private String _acronym;
        private String _presidentName;

        private int _votesParty;

        //I use this aux variable to calculate the seats for each party
        public int votesPartyAux;
        private int _seat;

        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChange("name");
            }
        }
        public String acronym
        {
            get { return _acronym; }
            set
            {
                _acronym = value;
                OnPropertyChange("acronym");
            }
        }
        public String presidentName
        {
            get { return _presidentName; }
            set
            {
                _presidentName = value;
                OnPropertyChange("presidentName");
            }
        }
        public int votesParty
        {
            get { return _votesParty; }
            set
            {
                _votesParty = value;
                OnPropertyChange("votesParty");
            }
        }
        public int seat
        {
            get { return _seat; }
            set
            {
                _seat = value;
                OnPropertyChange("seat");
            }
        }

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
                $" VALUES ('{name}','{acronym}', '{presidentName}', '{votesParty}', '{seat}');";
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
            String SQL = $"SELECT nombre, acronimo,nombrePresidente,votos,escanios FROM partido;";
            DataTable dt = MySQLDataComponent.LoadData(SQL, cnstr);
            if (dt.Rows.Count > 0)
            {
                if (name == null) parties = new ObservableCollection<Parties>();
                foreach (DataRow i in dt.Rows)
                {
                    parties.Add(new Parties
                    {
                        nameParty = i[0].ToString(),
                        acronymParty = i[1].ToString(),
                        presidentParty = i[2].ToString(),
                        voteParty = int.Parse(i[3].ToString()),
                        seatCount = int.Parse(i[4].ToString())
                    });
                }
            }
            dt.Dispose();
        }
    }
}
