using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ScreenVdeoV2
{
    public enum Filetype
    {
        KeineDef = 0,
        JPEG = 1,
        BMP = 2,
        PNG = 3,
        MP4 = 4,
        AVI = 5,
    }
    public enum FileArt
    {
        KeineDef = 0,
        Bild = 1,
        Video = 2,
    }
    public class ListElementViewModel:BaseViewModel
    {
        private Uri _Filename;
        public Uri Filename
        {
            get { return _Filename; }
            set
            {
                _Filename = value;
                RaisePropertyChanged(nameof(Filename));
            }
        }

        private Uri _FilenameVideo;
        public Uri FilenameVideo
        {
            get { return _FilenameVideo; }
            set
            {
                _FilenameVideo = value;
                RaisePropertyChanged("Filename");
            }
        }

        private Uri _FilenameBild;
        public Uri FilenameBild
        {
            get { return _FilenameBild; }
            set
            {
                _FilenameBild = value;
                RaisePropertyChanged("Filename");
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private Filetype _Typ;
        public Filetype Typ 
        {
            get
            {
                return _Typ;
            }
            set
            {
                _Typ = value;
                RaisePropertyChanged("Typ");
            }
 
        }

        private ICommand _KeyRightCommand;
        public ICommand KeyRightCommand
        {
            get
            {
                return _KeyRightCommand ?? (_KeyRightCommand = new CommandHandler(() => OnNext(), () => CanExecute));
            }
        }

        private void OnNext()
        {
        }


        private Brush _Background;
        public Brush Background
        {
            get
            {
                return _Background;
            }
            set
            {
                _Background = value;
                RaisePropertyChanged("Background");
            }
        }

        private FileArt _Art;
        public FileArt Art
        {
            get
            {
                return _Art;
            }
            set
            {
                if(value == FileArt.Bild)
                {
                    FilenameBild = Filename;
                    FilenameVideo = null;
                }
                else
                {
                    FilenameVideo = Filename;
                    FilenameBild = null;
                }
                _Art = value;
                RaisePropertyChanged("Art");
            }

        }

        private bool _Loop;
        public bool Loop
        {
            get
            {
                return _Loop;
            }
            set
            {
                _Loop = value;
                RaisePropertyChanged("Loop");
            }
        }

        private double _ScrollOffset;
        public double ScrollOffset
        {
            get
            {
                return _ScrollOffset;
            }
            set
            {
                _ScrollOffset = value;
                RaisePropertyChanged("ScrollOffset");
            }
        }

        private bool _Spiegeln;
        public bool Spiegeln
        {
            get
            {
                return _Spiegeln;
            }
            set
            {
                _Spiegeln = value;
                RaisePropertyChanged("Spiegeln");
            }
        }

        //Height of the Element
        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                RaisePropertyChanged("Height");
            }
        }

        //Width of the Element
        private double _Widht;
        public double Widht
        {
            get { return _Widht; }
            set
            {
                _Widht = value;
                RaisePropertyChanged("Widht");
            }
        }

        //ISActive erst ab 100 Dekkraft
        private bool _IsActive = false;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (value)
                {
                    Background = new SolidColorBrush() { Color = Colors.DimGray }; 
                }
                else
                {
                    Background = null;
                }
                _IsActive = value;
                RaisePropertyChanged("IsActive");
            }
        }


        private bool _IsVisible = true; //Wird nur für Scrolling genutzt
        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                _IsVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        private ICommand _ListElementDoubleClick;
        public ICommand ListElementDoubleClick
        {
            get { return _ListElementDoubleClick ?? (_ListElementDoubleClick = new CommandHandler(() => OnsetDoubleClick(), () => CanExecute)); }
        }

        
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }
        private void OnsetDoubleClick()
        {
            DoubleClick = true;
        }
        private bool _DoubleClick = false; //Wird nur für Scrolling genutzt
        public bool DoubleClick
        {
            get { return _DoubleClick; }
            set
            {
                _DoubleClick = value;
                RaisePropertyChanged("DoubleClick");
            }
        }
        
    }
}
