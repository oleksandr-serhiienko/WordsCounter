﻿using System;
using System.ComponentModel;
using System.Diagnostics;

namespace WordsCounter
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(int propertyValue)
        {
            VerifyPropertyName(propertyValue);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyValue.ToString()));
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyName);
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName(int propertyValue)
        {
            if (TypeDescriptor.GetProperties(this)[propertyValue] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyValue.ToString());
        }
    }
}
