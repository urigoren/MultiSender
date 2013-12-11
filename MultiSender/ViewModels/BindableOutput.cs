using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSender.ViewModels
{
    public class BoundedVars : INotifyPropertyChanged
    {
        //members:
        private String _headline = "";
        public ObservableCollection<ActionItem> _actionitems = new ObservableCollection<ActionItem>();
        public ObservableCollection<Models.Recipient> _recipients;

        public BoundedVars()
        {
        }

        #region Binded Properties
        public String Headline
        {
            get
            {
                return _headline;
            }
            set
            {
                _headline = value;
                RaisePropertyChanged("Headline");
            }
        }
        public ObservableCollection<ActionItem> ActionItems
        {
            get
            {
                return _actionitems;
            }
            set
            {
                _actionitems = value;
                RaisePropertyChanged("ActionItems");
            }
        }
        public ObservableCollection<Models.Recipient> Recipients
        {
            get
            {
                return _recipients;
            }
            set
            {
                _recipients = value;
                RaisePropertyChanged("Recipients");
            }
        }
        #endregion
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


    }

}
