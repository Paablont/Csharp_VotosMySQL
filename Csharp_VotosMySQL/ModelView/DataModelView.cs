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
using System.Windows;

namespace Csharp_VotosMySQL.ModelView
{
    internal class DataModelView
    {
        #region VARIABLES

        public const int TOTALPOPULATION = 6921267;
        private const String cnstr = "server=localhost;uid=pablo;pwd=pablo;database=votosddbb";


        public event PropertyChangedEventHandler? PropertyChanged;


        private int _peopleThatVote, _votesAbst, _votesValid, _votesNull;


        public int peopleThatVote
        {
            get { return _peopleThatVote; }
            set
            {
                _peopleThatVote = value;
                OnPropertyChange(nameof(peopleThatVote));
            }
        }
        public int votesAbst
        {
            get { return _votesAbst; }
            set
            {
                _votesAbst = value;
                OnPropertyChange(nameof(votesAbst));
            }
        }
        public int votesValid
        {
            get { return _votesValid; }
            set
            {
                _votesValid = value;
                OnPropertyChange(nameof(votesValid));
            }
        }
        public int votesNull
        {
            get { return _votesNull; }
            set
            {
                _votesNull = value;
                OnPropertyChange(nameof(votesNull));
            }
        }


        #endregion

        public DataModelView()
        {

        }

        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public void NewDate()
        {
            String SQL = $"INSERT INTO datos (poblacionTotal, votosNulos, votosValidos, abstenciones, personas)" +
                $" VALUES ('{TOTALPOPULATION}','{votesNull}', '{votesValid}', '{votesAbst}','{peopleThatVote}');";
            //usaremos las clases de la librería de MySQL para ejecutar queries
            //Instalar el paquete MySQL.Data
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void UpdateDates()
        {
            String SQL = $"UPDATE datos SET votosNulos = '{votesNull}',abstenciones = '{votesAbst}',votosValidos = '{votesValid}', personas = '{peopleThatVote}' WHERE poblacionTotal = '{TOTALPOPULATION}';";
            MySQLDataComponent.ExecuteNonQuery(SQL, cnstr);
        }

        public void LoadDates()
        {
            String SQL = $"SELECT poblacionTotal,votosNulos, votosValidos, abstenciones, personas FROM datos;";
            DataTable dt = MySQLDataComponent.LoadData(SQL, cnstr);

            Dates datos = new Dates(votesNull, votesValid, votesAbst,peopleThatVote);            

            dt.Dispose();
        }


        //Calculate the peopleThatVote
        public int calculatePeopleThatVote(String absentString)
        {
            int people = 0;

            try
            {
                int absentionVotes = int.Parse(absentString);
                if (absentionVotes >= TOTALPOPULATION)
                {
                    MessageBox.Show("The absetion votes values can not be higher than total population");
                }
                else
                {
                    people = TOTALPOPULATION - absentionVotes;

                }
            }
            catch (FormatException e)
            {
                MessageBox.Show("The value of absent votes can not be alphabetic character");
            }

            return people;

        }
        //Calculate the null votes        
        public int votesNullCalculate(String absentString)
        {
            int people = calculatePeopleThatVote(absentString);
            int nullvotes = (people / 20);

            return nullvotes;
        }

        //Calculate the valid votes
        public int votesValidCalculate(int peopleThatVote, int votesNull)
        {
            return peopleThatVote - votesNull;
        }

        public override string? ToString()
        {
            return "Valid votes: " + peopleThatVote +
                "\n" + "Abstent votes: " + votesAbst +
                "\n" + "Null votes: " + votesNull;
        }
    }
}
