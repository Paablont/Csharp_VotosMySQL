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
        private string _name;
        private string _acronym;
        private string _presidentName;

        private int _votesParty;
        

        // Utilizo esta variable auxiliar para calcular los escaños para cada partido
        private int _votesPartyAux;
        private int _seat;

        public string NameParty
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChange(nameof(NameParty));
            }
        }
        public string AcronymParty
        {
            get { return _acronym; }
            set
            {
                _acronym = value;
                OnPropertyChange(nameof(AcronymParty));
            }
        }
        public string PresidentParty
        {
            get { return _presidentName; }
            set
            {
                _presidentName = value;
                OnPropertyChange(nameof(PresidentParty));
            }
        }
        public int VoteParty
        {
            get { return _votesParty; }
            set
            {
                _votesParty = value;
                OnPropertyChange(nameof(VoteParty));
            }
        }

        public int VotePartyAux
        {
            get { return _votesPartyAux; }
            set
            {
                _votesPartyAux = value;
                OnPropertyChange(nameof(VotePartyAux));
            }
        }
        public int SeatCount
        {
            get { return _seat; }
            set
            {
                _seat = value;
                OnPropertyChange(nameof(SeatCount));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        // Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
