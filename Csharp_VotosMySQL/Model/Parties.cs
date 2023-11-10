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
        public String name { get; set; }
        public String acronym { get; set; }
        public String presidentName { get; set; }

        public int votesParty { get; set; }

        //I use this aux variable to calculate the seats for each party
        public int votesPartyAux { get; set; }
        public int seat { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
