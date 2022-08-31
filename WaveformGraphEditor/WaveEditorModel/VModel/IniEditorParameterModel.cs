using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfBase;
using WpfControlBase.Commands;
using WpfControlBase.Interfaces;

namespace WaveEditorModel.VModel
{
    public class IniEditorParameterModel : ModelBase, ICommandFun
    {
        #region Filde
        private double _sampleRate;
        private double _sampleTime;
        private bool _srvisbilty = false;
        Dictionary<string, Action<string>> funkeyValues = new();
        private string _samples;
        private bool _STVisiblity=false;
        private string _SampleRateUnit;
        private string _SampleTimeUnit;
        #endregion
        #region Properties

        public List<string> TimesName { get; set; } = new List<string>() {"s","ns" ,"us","ms"};

        public List<string> SampleRateName { get; set; } = new List<string>() { "μsa/s","msa/s","sa/s","Ksa/s","Msa/s","Gsa/s"};
            /// <summary>
        /// 采样率
        /// </summary>
        public double SampleRate
        {
            get { return _sampleRate; }
            set
            {
                SRVisibilty = true;
                _sampleRate = value;
                OnPropertyChanged(nameof(SampleRate));
            }
        }

        public string SampleRateUnit
        {
            get { return _SampleRateUnit; }
            set { _SampleRateUnit = value; OnPropertyChanged(nameof(SampleRateUnit)); }
        }

        public string SampleTimeUnit
        {
            get
            {
                return _SampleTimeUnit;

            }
            set
            { _SampleTimeUnit = value; OnPropertyChanged(nameof(SampleTimeUnit)); }
        }
        /// <summary>
        /// 采样时间
        /// </summary>
        public double SampleTime
        {
            get
            {
                return _sampleTime; ;
            }
            set
            {
                STVisiblity = true;
                _sampleTime = value;
                OnPropertyChanged(nameof(SampleTime));
            }
        }
        /// <summary>
        /// 参数单位选择框可见性
        /// </summary>
        public bool SRVisibilty
        {
            get
            {
                return _srvisbilty;
            }
            set
            {
                _srvisbilty = value;
                OnPropertyChanged(nameof(SRVisibilty));
            }
        }

        public bool STVisiblity
        {
            get
            {
                return _STVisiblity;
            }
            set
            {
                _STVisiblity = value;
                OnPropertyChanged(nameof(STVisiblity));
            }
        }

        public string Samples {
            get
            {
                return _samples;
            } set
            {
                _samples = value;
                OnPropertyChanged(nameof(Samples));
            }
        }
        #endregion

        public EventHandler<string> ErrorEventHandler;

        #region Private Methods

        private void setSampleRate( string str)
        {
            if (SampleRate == 0)
            {
                return;
            }
            if (SampleTime == 0)
                return;
            SampleRateUnit = str;

           
        }

        private void setSamples()
        {
            var s = GetRate() * GetTime();

            if(s<10)
            {
                Samples = "请大于10sa";
            }
            if(s>1000000000)
            {
                Samples = "请小于1Gsa";
            }

        }

        private double GetRate()
        {
            switch (SampleRateUnit)
            {
                case "μsa/s": return 0.000001 ;
                case "msa/s": return 0.001;
                case "sa/s":return 1;
                case "Ksa/s":return 1000;
                case "Msa/s": return 1000000;
                case "Gsa/s": return 1000000000;
            }

            return 1;
        }

        private double GetTime()
        {
            if (SampleTimeUnit == "ns")
                return 0.000000001;
            if (SampleTimeUnit == "us")
                return 0.000001;
            if (SampleTimeUnit == "ms")
                return 0.001;
            return 1;
            
        }
        #endregion

        public IniEditorParameterModel()
        {

        }
        public ICommand ClickAction => new RelayCommand<object>(UpdateNameExecute, CanUpdateNameExecute1);

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand EnterClick => new RelayCommand<object>(Checkpar, CanUpdateNameExecute12);

        private bool CanUpdateNameExecute12()
        {
            return true;
        }

        private void Checkpar(object obj)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdateNameExecute1()
        {
            return true;
        }

        public void UpdateNameExecute(object sender)
        {
            try
            {
                if (sender is Object[] value)
                {
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                    funkeyValues[value[0].ToString()].Invoke(value[1].ToString());
#pragma warning restore CS8604 // 引用类型参数可能为 null。

                }
            }
            catch(Exception)
            {
                STVisiblity = false;
                SRVisibilty = false;
            }
        }
    }
}
