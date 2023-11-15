using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_VotosMySQL.Model
{
    class Parties : INotifyPropertyChanged
    {
        #region VARIABLES
        private String name;
        private String acronym;
        private String presidentName;

        private int votesParty;

        //I use this aux variable to calculate the seats for each party
        public int votesPartyAux;
        private int seat;

        public String nameParty
        {
            get { return name; }
            set { name = value; 
                OnPropertyChange("nameParty");
            }
        }
        public String acronymParty
        {
            get { return acronym; }
            set
            {
                acronym = value;
                OnPropertyChange("acronymParty");
            }
        }
        public String presidentParty
        {
            get { return presidentName; }
            set
            {
                presidentName = value;
                OnPropertyChange("presidentParty");
            }
        }
        public int voteParty
        {
            get { return votesParty; }
            set
            {
                votesParty = value;
                OnPropertyChange("voteParty");
            }
        }
        public int seatCount
        {
            get { return seat; }
            set
            {
                seat = value;
                OnPropertyChange("seatCount");
            }
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
