using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenVdeoV2
{
    public class PresentationViewModel:BaseViewModel
    {
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
            throw new NotImplementedException();
        }

        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                _IsActive = value;
                RaisePropertyChanged("IsActive");
            }
        }


        private double _Height = 1217.6;
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
                RaisePropertyChanged("Height");
            }
        }

        private double _Width = 3065.4;
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
                RaisePropertyChanged("Width");
            }
        }

        private double _Top = 0;
        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                _Top = value;
                RaisePropertyChanged("Top");
            }
        }

        private double _Left = -3120.4;
        public double Left
        {
            get
            {
                return _Left;
            }
            set
            {
                _Left = value;
                RaisePropertyChanged("Left");
            }
        }

        private ListElementViewModel _FrontElement;
        public ListElementViewModel FrontElement
        {
            get { return _FrontElement; }
            set
            {
                _FrontElement = value;
                RaisePropertyChanged("FrontElement");
            }
        }

        private ListElementViewModel _BackElement;
        public ListElementViewModel BackElement
        {
            get { return _BackElement; }
            set
            {
                _BackElement = value;
                RaisePropertyChanged("BackElement");
            }
        }

        private double _OpacitiyFrontElement = 1;
        public double OpacitiyFrontElement
        {
            get { return _OpacitiyFrontElement; }
            set
            {
                _OpacitiyFrontElement = value;
                RaisePropertyChanged("OpacitiyFrontElement");
            }
        }

        private double _OpacitiyBackElement = 0;
        public double OpacitiyBackElement
        {
            get { return _OpacitiyBackElement; }
            set
            {
                _OpacitiyBackElement = value;
                RaisePropertyChanged("OpacitiyBackElement");
            }
        }

        public bool CanExecute { get; private set; }
    }
}
