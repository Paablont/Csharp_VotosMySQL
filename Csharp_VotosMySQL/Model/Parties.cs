using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace Csharp_VotosMySQL.Model
{
    class Parties : INotifyPropertyChanged
    {
        #region VARIABLES
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _name;
        private string _acronym;
        private string _presidentName;

        private int _votesParty;
        

        // Utilizo esta variable auxiliar para calcular los escaños para cada partido
        private int _votesPartyAux;
        private int _seat;


        //ESTAS AL BINDING
        public string nameParty
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChange(nameof(nameParty));
            }
        }
        public string acronymParty
        {
            get { return _acronym; }
            set
            {
                _acronym = value;
                OnPropertyChange(nameof(acronymParty));
            }
        }
        public string presidentParty
        {
            get { return _presidentName; }
            set
            {
                _presidentName = value;
                OnPropertyChange(nameof(presidentParty));
            }
        }
        public int voteParty
        {
            get { return _votesParty; }
            set
            {
                _votesParty = value;
                OnPropertyChange(nameof(voteParty));
            }
        }

        public int votePartyAux
        {
            get { return _votesPartyAux; }
            set
            {
                _votesPartyAux = value;
                OnPropertyChange(nameof(votePartyAux));
            }
        }
        public int seatCount
        {
            get { return _seat; }
            set
            {
                _seat = value;
                OnPropertyChange(nameof(seatCount));
            }
        }

        // En la clase Parties
        




        #endregion

        // Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
