using PA.IPChanger;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PA.OnClickIpChanger.ViewModels
{
    public class IPSettingViewModel : BindableBase
    {
        public event EventHandler Closed;
        #region properties
        public List<string> Devices { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public IPProfile Setting { get; set; }
        public bool IsNew { get; private set; }
        public virtual bool Result { get; set; }
        #endregion
        public IPSettingViewModel(IPProfile instance, bool isNew,List<String> devices)
        {
            Devices = devices;
            SaveCommand = new DelegateCommand(Save, () => true);
            CloseCommand = new DelegateCommand(Close, () => true);
            Setting = instance;
            IsNew = isNew;
        }
        #region public methods
        public void Save()
        {
            Setting.Validate();
            Result = true;
            Close();
        }
        public void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
