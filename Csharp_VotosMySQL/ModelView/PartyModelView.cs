using Csharp_VotosMySQL.DDBB;
using Csharp_VotosMySQL.Model;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Csharp_VotosMySQL.ModelView
{
    class PartyModelView : INotifyPropertyChanged
    {
        #region VARIABLES

        //Declaro la constante para la conexión a la BDD
        private const String cnstr = "server=localhost;uid=pablo;pwd=pablo;database=votossql";
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
        public int seatCount
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

        public PartyModelView()
        {
            _party = new ObservableCollection<Parties>();
        }

        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public void newParty()
        {
            String SQL = $"INSERT INTO partido (nombre, acronimo, nombrePresidente, votos, escanios)" +
                $" VALUES ('{name}','{acronym}', '{presidentName}', '{votesParty}', '{seatCount}');";
            //usaremos las clases de la librería de MySQL para ejecutar queries
            //Instalar el paquete MySQL.Data
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void UpdateParty()
        {
            String SQL = $"UPDATE partido SET nombre = '{name}', acronimo = '{acronym}',nombrePresidente = '{presidentName}'" +
                $",votos = '{votesParty}',escanios = '{seatCount}' WHERE nombre = '{name}';";
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void LoadParties()
        {
            String SQL = $"SELECT id,nombre, acronimo, nombrePresidente, votos, escanios FROM partido;";
            DataTable dt = MySQLDataComponent.LoadData(SQL, cnstr);

            // Limpia la colección actual
            parties.Clear();

            foreach (DataRow i in dt.Rows)
            {
                parties.Add(new Parties
                {
                    nameParty = i[1].ToString(),  // id está en la posición 0, así que empezamos desde la posición 1
                    acronymParty = i[2].ToString(),
                    presidentParty = i[3].ToString(),
                    voteParty = int.Parse(i[4].ToString()),
                    votePartyAux = 0, // Puedes inicializarlo con algún valor si es necesario
                    seatCount = int.Parse(i[5].ToString())
                });




            }

            dt.Dispose();
        }


        public void DeleteParty(string partyName)
        {
            // Realiza la eliminación del partido en la base de datos
            String SQL = $"DELETE FROM partido WHERE nombre = '{partyName}';";
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);

            // Elimina el partido de la colección local
            var partyToDelete = parties.FirstOrDefault(p => p.nameParty == partyName);
            if (partyToDelete != null)
            {
                parties.Remove(partyToDelete);
            }
        }


        //Calculate the votes to each party
        public void calculateVotesParty(int votesValid, ObservableCollection<Parties> partyList)
        {
            double[] percentages = { 35.25, 24.75, 15.75, 14.25, 3.75, 3.25, 1.5, 0.5, 0.25, 0.25 };

            for (int i = 0; i < partyList.Count; i++)
            {
                partyList[i].voteParty = (int)Math.Round(votesValid * (percentages[i] / 100));
                partyList[i].votePartyAux = (int)Math.Round(votesValid * (percentages[i] / 100));

            }


        }

        public void calculateStands(ObservableCollection<Parties> partyList, int seatsNumber)
        {
            int posMaxValue, maxVotes;

            for (int i = 0; i < seatsNumber; i++)
            {
                maxVotes = partyList.Max(x => x.votePartyAux);
                posMaxValue = partyList.IndexOf(partyList.FirstOrDefault(x => x.votePartyAux == maxVotes));
                partyList[posMaxValue].seatCount += 1;
                partyList[posMaxValue].votePartyAux = partyList[posMaxValue].voteParty / (partyList[posMaxValue].seatCount + 1);
            }
        }


    }
}
