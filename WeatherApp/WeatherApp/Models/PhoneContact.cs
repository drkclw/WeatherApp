using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class PhoneContact : BaseModel
    {
        public string _id { get; set; }

        public string LocalName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }

        bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
