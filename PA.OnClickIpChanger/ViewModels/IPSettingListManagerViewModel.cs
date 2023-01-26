using PA.IPChanger;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace PA.OnClickIpChanger.ViewModels
{
    public class IPSettingListManagerViewModel : BindableBase
    {
        private string db = ".\\setting.txt";
        IPSettingWindow settingWindow = null;
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }
        public DelegateCommand ApplyIPCommand { get; private set; }
        public DelegateCommand ApplyDNSCommand { get; private set; }
        public DelegateCommand ApplyAllCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public ObservableCollection<IPProfile> Data { get; private set; }
        public IPProfile SelectedProfile { get; set; }
        public bool Changed { get; private set; }
        public IPSettingListManagerViewModel()
        {
            Data = new ObservableCollection<IPProfile>();
            AddCommand = new DelegateCommand(Add, () => true);
            EditCommand = new DelegateCommand(Edit, () => true);
            RemoveCommand = new DelegateCommand(Remove, () => true);
            SaveCommand = new DelegateCommand(SaveToFile, () => true);
            ApplyIPCommand = new DelegateCommand(ApplyIP, () => true);
            ApplyDNSCommand = new DelegateCommand(ApplyDNS, () => true);
            ApplyAllCommand = new DelegateCommand(ApplyAll, () => true);
            InitilizeData();
        }

        private void ApplyIP()
        {
            if (SelectedProfile == null)
                return;
            if (SelectedProfile.DHCPEnabled)
                IPManager.SetDHCP(SelectedProfile.NIC);
            IPManager.SetIP(SelectedProfile.IP, SelectedProfile.Subnet,SelectedProfile.Getway,SelectedProfile.NIC);
        }

        private void ApplyDNS()
        {
            if (SelectedProfile == null)
                return;
            IPManager.SetDNS(SelectedProfile.NIC, string.Join(",",new string[] { SelectedProfile.Dns1, SelectedProfile.Dns2 }));
        }

        private void ApplyAll()
        {
            ApplyIP();
            ApplyDNS();
        }

        private void SaveToFile()
        {
            if (Data.Count == 0)
                return;
            string[] lines = new string[Data.Count];
            int i = 0;
            foreach(IPProfile ip in Data)
            {
                string line = ip.Serialize();
                lines[i++] = line;
            }
            File.WriteAllLines(db, lines);
        }

        private void InitilizeData()
        {
            if (!File.Exists(db))
                File.Create(db);
            else
            {
                string[] lines = File.ReadAllLines(db);
                List<IPProfile> ips = IPManager.Parse(lines);
                foreach (IPProfile ip in ips)
                    Data.Add(ip);
            }
        }
        public void Add()
        {
            IPSettingViewModel model = new IPSettingViewModel(new IPProfile(), true,IPManager.GetNetAdapterCaptions());
            model.Closed += Model_Closed;
            settingWindow = new IPSettingWindow()
            {
                WindowState = System.Windows.WindowState.Normal,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                WindowStyle = System.Windows.WindowStyle.SingleBorderWindow,
                DataContext = model,
                Title = "Create New IP Setting"
            };
            settingWindow.ShowDialog();
        }

        private void Model_Closed(object sender, EventArgs e)
        {
            if (settingWindow != null)
            {
                IPSettingViewModel model = sender as IPSettingViewModel;
                if(model.Result)
                {
                    if (model.IsNew)
                    {
                        SaveAsNew(model.Setting);
                    }
                    else
                    {
                        UpdateSelected(model.Setting);
                    }
                }
                settingWindow.Close();
            }
        }

        private void UpdateSelected(IPProfile setting)
        {
            Data.Remove(SelectedProfile);
            Data.Add(setting);
            RaisePropertyChanged("Data");

        }

        private void SaveAsNew(IPProfile setting)
        {
            Data.Add(setting);
            SelectedProfile = Data.Last();
        }

        public void Edit()
        {
            if (SelectedProfile == null)
                return;
            IPProfile editingModel = (IPProfile)SelectedProfile.Clone();
            IPSettingViewModel model = new IPSettingViewModel(editingModel, false, IPManager.GetNetAdapterCaptions());
            model.Closed += Model_Closed;
            settingWindow = new IPSettingWindow()
            {
                WindowState = System.Windows.WindowState.Normal,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                WindowStyle = System.Windows.WindowStyle.SingleBorderWindow,
                DataContext = model,
                Title = "Edit IP Setting : " + model.Setting.Name 
            };
            settingWindow.ShowDialog();
        }
        public void Remove() 
        {
            if (SelectedProfile == null)
                return;
            Data.Remove(SelectedProfile);
        }
    }
}
