using System.ComponentModel;

namespace AssetManager.Common
{
    public class DisplaySettings : INotifyPropertyChanged
    {
        private static DisplaySettings instance;
        public static DisplaySettings Get
        {
            get
            {
                if (instance == null)
                    instance = new DisplaySettings();

                return instance;
            }
        }

        private double csysSize = 1;
        public double CSysSize
        {
            get { return csysSize; }
            set
            {
                if (csysSize != value)
                {
                    csysSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CSysSize)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private DisplaySettings() { }
    }
}
