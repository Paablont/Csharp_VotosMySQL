using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Csharp_VotosMySQL.Model
{
    public class Dates
    {
        public const int TOTALPOPULATION = 6921267;

        public event PropertyChangedEventHandler? PropertyChanged;

        private int _peopleThatVote, _votesAbst, _votesValid, _votesNull;

        public Dates(int peopleThatVote, int votesAbst, int votesValid, int votesNull)
        {
            _peopleThatVote = peopleThatVote;
            _votesAbst = votesAbst;
            _votesValid = votesValid;
            _votesNull = votesNull;

        }

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

        // Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
