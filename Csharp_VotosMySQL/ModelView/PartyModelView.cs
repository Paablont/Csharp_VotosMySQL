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
                        NameParty = i[0].ToString(),
                        AcronymParty = i[1].ToString(),
                        PresidentParty = i[2].ToString(),
                        VoteParty = int.Parse(i[3].ToString()),
                        SeatCount = int.Parse(i[4].ToString())
                    });
                }
            }
            dt.Dispose();
        }

        public void deleteParties(Parties p)
        {
            parties.Remove(p);
        }


        //Calculate the votes to each party
        public void calculateVotesParty(int votesValid, ObservableCollection<Parties> partyList)
        {
            double[] percentages = { 35.25, 24.75, 15.75, 14.25, 3.75, 3.25, 1.5, 0.5, 0.25, 0.25 };

            for (int i = 0; i < partyList.Count; i++)
            {
                partyList[i].VoteParty = (int)Math.Round(votesValid * (percentages[i] / 100));
                partyList[i].VotePartyAux = (int)Math.Round(votesValid * (percentages[i] / 100));

            }


        }

        public void calculateStands(ObservableCollection<Parties> partyList, int seatsNumber)
        {
            int posMaxValue, maxVotes;

            for (int i = 0; i < seatsNumber; i++)
            {
                maxVotes = partyList.Max(x => x.VotePartyAux);
                posMaxValue = partyList.IndexOf(partyList.FirstOrDefault(x => x.VotePartyAux == maxVotes));
                partyList[posMaxValue].SeatCount += 1;
                partyList[posMaxValue].VotePartyAux = partyList[posMaxValue].VoteParty / (partyList[posMaxValue].SeatCount + 1);
            }
        }


    }
}
