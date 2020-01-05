using PresenterV2.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ScreenVdeoV2
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Delay.Tick += Delay_Tick;
            OpacitiyTicker.Tick += OpacitiyTicker_Tick;
            DelayLoadBackground.Tick += DelayLoadBackground_Tick;
            DelayStartScroll.Tick += DelayStartScroll_Tick;
        }


        private void FadeIn_Tick(object sender, EventArgs e)
        {
            if (PresentationViewModelProp.FrontElement.IsActive)
            {
                if (PresentationViewModelProp.OpacitiyFrontElement > 1)
                {
                    PresentationViewModelProp.OpacitiyFrontElement = 1;
                    FadeIn.Stop();
                }
                else if (PresentationViewModelProp.OpacitiyFrontElement < 1) PresentationViewModelProp.OpacitiyFrontElement += 0.020; 
            }
            else
            {
                if (PresentationViewModelProp.OpacitiyBackElement > 1)
                {
                    PresentationViewModelProp.OpacitiyBackElement = 1;
                    FadeIn.Stop();
                }
                else if (PresentationViewModelProp.OpacitiyBackElement < 1) PresentationViewModelProp.OpacitiyBackElement += 0.020; 
            }
            
        }

        private void FadeOut_Tick(object sender, EventArgs e)
        {
            if (PresentationViewModelProp.OpacitiyFrontElement < 0)
            {
                PresentationViewModelProp.OpacitiyFrontElement = 0;
            }
            else if (PresentationViewModelProp.OpacitiyFrontElement > 0) PresentationViewModelProp.OpacitiyFrontElement -= 0.020;
            if (PresentationViewModelProp.OpacitiyBackElement < 0)
            {
                PresentationViewModelProp.OpacitiyBackElement = 0;
            }
            else if (PresentationViewModelProp.OpacitiyBackElement > 0) PresentationViewModelProp.OpacitiyBackElement -= 0.020;
            if(PresentationViewModelProp.OpacitiyBackElement == 0 && PresentationViewModelProp.OpacitiyFrontElement == 0)
            {
                FadeOut.Stop();
            }
        }

        private void DelayStartScroll_Tick(object sender, EventArgs e)
        {
            DelayStartScroll.Stop();
            ChangingElement = false;
        }

        private void ListElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ScrollOffset":
                    ScrollOffsetThisElmt = (sender as ListElementViewModel).ScrollOffset;
                    break;
                case "DoubleClick":
                    OnNextDirect(sender as ListElementViewModel);
                    break;
                default:
                    break;
            }
        }

        private void Scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (PresentationViewModelProp.FrontElement != null)
            {
                if (!ChangingElement && (PresentationViewModelProp.FrontElement.IsActive && (sender as ScrollViewer).Name == "scrollFrontElement" || PresentationViewModelProp.BackElement.IsActive && (sender as ScrollViewer).Name == "scrollBackElement"))
                {
                    if (((sender as ScrollViewer).DataContext as PresentationViewModel).FrontElement != null)
                    {
                        if(PresentationViewModelProp.FrontElement.IsActive)
                        {
                            ((sender as ScrollViewer).DataContext as PresentationViewModel).FrontElement.ScrollOffset = e.VerticalOffset;
                        }
                        else
                        {
                            ((sender as ScrollViewer).DataContext as PresentationViewModel).BackElement.ScrollOffset = e.VerticalOffset;
                        }
                    }
                }  
            }
        }

        private void DelayLoadBackground_Tick(object sender, EventArgs e)
        {
            DelayLoadBackground.Stop();
            ListElementViewModel nextElm = null;

            
            if (PresentationViewModelProp.FrontElement.IsActive)
            {
                if (ListBoxItems[ListBoxItems.Count - 1].Filename != PresentationViewModelProp.FrontElement.Filename)
                {
                    foreach (var item in ListBoxItems)
                    {
                        if (item.IsActive)
                        {
                            nextElm = ListBoxItems[ListBoxItems.IndexOf(item) + 1];
                        }
                    }
                    PresentationViewModelProp.BackElement = nextElm;
                } 
            }
            else
            {
                if (ListBoxItems[ListBoxItems.Count - 1].Filename != PresentationViewModelProp.BackElement.Filename)
                {
                    foreach (var item in ListBoxItems)
                    {
                        if (item.IsActive)
                        {
                            nextElm = ListBoxItems[ListBoxItems.IndexOf(item) + 1];
                            nextElm.IsVisible = false;
                        }
                    }
                    PresentationViewModelProp.FrontElement = nextElm;
                }
            }
        }

        private void OpacitiyTicker_Tick(object sender, EventArgs e)
        {
            if(PresentationViewModelProp.FrontElement.IsActive)
            {
                if(PresentationViewModelProp.OpacitiyFrontElement > 0) PresentationViewModelProp.OpacitiyFrontElement -= 0.020;
                PresentationViewModelProp.OpacitiyBackElement += 0.020;
                if( PresentationViewModelProp.OpacitiyFrontElement <= 0 && PresentationViewModelProp.OpacitiyBackElement > 1)
                {
                    PresentationViewModelProp.OpacitiyBackElement = 1;
                    PresentationViewModelProp.OpacitiyFrontElement = 0;
                    OpacitiyTicker.Stop();
                    ChangingElement = false;
                    PresentationViewModelProp.FrontElement.IsActive = false;
                    PresentationViewModelProp.BackElement.IsActive = true;
                    PresentationViewModelProp.FrontElement.IsVisible = false;
                    DelayLoadBackground.Start();
                }
            }
            else
            {
                PresentationViewModelProp.OpacitiyFrontElement += 0.020;
                if (PresentationViewModelProp.OpacitiyFrontElement > 1)
                {
                    PresentationViewModelProp.OpacitiyBackElement = 0;
                    PresentationViewModelProp.OpacitiyFrontElement = 1;
                    OpacitiyTicker.Stop();
                    ChangingElement = false;
                    PresentationViewModelProp.BackElement.IsActive = false;
                    PresentationViewModelProp.FrontElement.IsActive = true;
                    DelayLoadBackground.Start();
                }
            }
        }

        private void Delay_Tick(object sender, EventArgs e)
        {
            Delay.Stop();
            OpacitiyTicker.Start();
        }

        private bool _ChangingElement = false;
        public bool ChangingElement
        {
            get
            {
                return _ChangingElement;
            }
            set
            {
                _ChangingElement = value;
                RaisePropertyChanged("ChangingElement");
            }
        }
        private PresentationViewModel _PresentationViewModelProp = new PresentationViewModel();
        public PresentationViewModel PresentationViewModelProp
        {
            get
            {
                return _PresentationViewModelProp;
            }
            set
            {
                _PresentationViewModelProp = value;
                RaisePropertyChanged("PresentationViewModelProp");
            }
        }

        private Presentation _PresentationProp;
        public Presentation PresentationProp
        {
            get
            {
                return _PresentationProp;
            }
            set
            {
                _PresentationProp = value;
                RaisePropertyChanged("PresentationProp");
            }
        }

        private ObservableCollection<ListElementViewModel> _ListBoxItems = new ObservableCollection<ListElementViewModel>();
        public ObservableCollection<ListElementViewModel> ListBoxItems
        {
            get
            {
                return _ListBoxItems;
            }
            set
            {
                _ListBoxItems = value;
                RaisePropertyChanged("ListBoxItems");
            }
        }

        private ICommand _ButtonNext;
        public ICommand ButtonNext
        {
            get
            {
                return _ButtonNext ?? (_ButtonNext = new CommandHandler(()=>OnNext(), () => CanExecute));
            }
        }

        private ICommand _ButtonNew;
        public ICommand ButtonNew
        {
            get
            {
                return _ButtonNew ?? (_ButtonNew = new CommandHandler(() => OnNew(), () => CanExecute));
            }
        }

        private ICommand _ButtonDateiLaden;
        public ICommand ButtonDateiLaden
        {
            get
            {
                return _ButtonDateiLaden ?? (_ButtonDateiLaden = new CommandHandler(() => OnLoad(), () => CanExecute));
            }
        }

        private ICommand _ButtonDateiSpeichern;
        public ICommand ButtonDateiSpeichern
        {
            get
            {
                return _ButtonDateiSpeichern ?? (_ButtonDateiSpeichern = new CommandHandler(() => OnSave(), () => CanExecute));
            }
        }

        private ICommand _ButtonStart;
        public ICommand ButtonStart
        {
            get
            {
                return _ButtonStart ?? (_ButtonStart = new CommandHandler(() => OnStart(), () => CanExecute));
            }
        }
        private ICommand _ButtonStop;
        public ICommand ButtonStop
        {
            get
            {
                return _ButtonStop ?? (_ButtonStop = new CommandHandler(() => OnStopp(), () => CanExecute));
            }
        }
        private ICommand _ButtonBreiteAnwenden;
        public ICommand ButtonBreiteAnwenden
        {
            get
            {
                return _ButtonBreiteAnwenden ?? (_ButtonBreiteAnwenden = new CommandHandler(() => OnBreiteAnwenden(), () => CanExecute));
            }
        }

        private ICommand _ButtonHoeheAnwenden;
        public ICommand ButtonHoeheAnwenden
        {
            get
            {
                return _ButtonHoeheAnwenden ?? (_ButtonHoeheAnwenden = new CommandHandler(() => OnHoehAnwenden(), () => CanExecute));
            }
        }
        private ICommand _ButtonBlack;
        public ICommand ButtonBlack
        {
            get
            {
                return _ButtonBlack ?? (_ButtonBlack = new CommandHandler(() => OnBlack(), () => CanExecute));
            }
        }
        private ICommand _ButtonFadeIn;
        public ICommand ButtonFadeIn
        {
            get
            {
                return _ButtonFadeIn ?? (_ButtonFadeIn = new CommandHandler(() => OnFadeIn(), () => CanExecute));
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

        private double _WidthListBox;
        public double WidthListBox
        {
            get { return WidthMainWindow - 35; }
            set 
            { 
                _WidthListBox = value;
                RaisePropertyChanged(nameof(WidthListBox));
            }
        }
        private double _HeightListBox;
        public double HeightListBox
        {
            get { return HeightMainWindow - 135; }
            set
            {
                _HeightListBox = value;
                RaisePropertyChanged(nameof(HeightListBox));
            }
        }

        private double _WidthMainWindow = 1000;
        public double WidthMainWindow
        {
            get { return _WidthMainWindow; }
            set
            {
                WidthListBox = value;
                _WidthMainWindow = value;
                RaisePropertyChanged(nameof(WidthMainWindow));
            }
        }
        private double _HeightMainWindow = 500;
        public double HeightMainWindow
        {
            get { return _HeightMainWindow; }
            set
            {
                HeightListBox = value;
                _HeightMainWindow = value;
                RaisePropertyChanged(nameof(HeightMainWindow));
            }
        }

        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        public double ScrollOffsetNextElmt //Sets the ScrollOffset to the not Active Element
        {
            set
            {
                if(PresentationViewModelProp.FrontElement.IsActive)
                {
                    PresentationProp.scrollBackElement.ScrollToVerticalOffset(value);
                }
                else
                {
                    PresentationProp.scrollFrontElement.ScrollToVerticalOffset(value);
                }
            }
        }

        public double ScrollOffsetThisElmt //Sets the ScrollOffset to the not Active Element
        {
            set
            {
                if (PresentationViewModelProp.FrontElement.IsActive)
                {
                    PresentationProp.scrollFrontElement.ScrollToVerticalOffset(value);
                }
                else
                {
                    PresentationProp.scrollBackElement.ScrollToVerticalOffset(value);
                }
            }
        }

        public const double OpacityTime = 10;
        private DispatcherTimer Delay = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(50) };
        private DispatcherTimer DelayLoadBackground = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
        private DispatcherTimer OpacitiyTicker = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(OpacityTime) };
        private DispatcherTimer DelayStartScroll = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
        private DispatcherTimer FadeOut = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(OpacityTime) };
        private DispatcherTimer FadeIn = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(OpacityTime) };
        public void OnStart(ListElementViewModel listElement = null)
        {
            if (!PresentationViewModelProp.IsActive && ListBoxItems.Count != 0) //Es ist noch kein Fenster offen und es gibt Elemente in der ListBox
            {
                PresentationProp = new Presentation();
                ChangingElement = true;
                PresentationViewModelProp.IsActive = true;
                PresentationProp.scrollFrontElement.ScrollChanged += Scroll_ScrollChanged;
                PresentationProp.scrollBackElement.ScrollChanged += Scroll_ScrollChanged;
                PresentationProp.DataContext = PresentationViewModelProp;
                
                if (listElement == null)
                {
                    PresentationViewModelProp.FrontElement = ListBoxItems[0];
                    ListBoxItems[0].IsActive = true; 
                }
                else
                {
                    PresentationViewModelProp.FrontElement = listElement;
                    listElement.IsActive = true;
                }
                PresentationViewModelProp.OpacitiyFrontElement = 1;
                PresentationViewModelProp.FrontElement.IsVisible = true;

                DelayStartScroll.Start();

                
                PresentationViewModelProp.BackElement = new ListElementViewModel() { IsActive = false };
                ScrollOffsetThisElmt = PresentationViewModelProp.FrontElement.ScrollOffset;
                PresentationProp.Show();
                Debug.WriteLine(PresentationProp.Height.ToString() + PresentationProp.Width.ToString());
            }
            
        }
        public void OnStopp()
        {
            if(PresentationViewModelProp.IsActive)
            {
                PresentationViewModelProp.FrontElement = null;
                PresentationViewModelProp.BackElement = null;
                PresentationViewModelProp.IsActive = false;
                PresentationProp.Hide();
                foreach (var element in ListBoxItems)
                {
                    element.IsActive = false;
                }
            }
        }
        public void OnNext()
        {
            ChangingElement = true;
            foreach (var element in ListBoxItems)
            {
                if (element.IsActive && ListBoxItems.Count != ListBoxItems.IndexOf(element) + 1)
                {
                    if(element.Filename == PresentationViewModelProp.FrontElement.Filename)
                    {
                        PresentationViewModelProp.BackElement = ListBoxItems[ListBoxItems.IndexOf(element) + 1];
                        if (PresentationViewModelProp.BackElement.Art == FileArt.Video) PresentationProp.MediaElementBack.Position = TimeSpan.FromSeconds(0);
                        ScrollOffsetNextElmt = PresentationViewModelProp.BackElement.ScrollOffset;
                        //Deckkraft Frontelement Runter Fahren
                        Delay.Start();
                    }
                    else
                    {
                        PresentationViewModelProp.FrontElement = ListBoxItems[ListBoxItems.IndexOf(element) + 1];
                        if (PresentationViewModelProp.FrontElement.Art == FileArt.Video) PresentationProp.MediElementFront.Position = TimeSpan.FromSeconds(0);
                        ScrollOffsetNextElmt = PresentationViewModelProp.FrontElement.ScrollOffset;
                        PresentationViewModelProp.FrontElement.IsVisible = true;
                        //Deckraft Frontelement hochfahren
                        Delay.Start();
                    }
                }
            }
        }
        public void OnLoad(string filename = "")
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            FileStream fs = null;
            if (filename == "")
            {

                openDialog.Filter = "Textfile (*.txt;|*.txt;)";
                openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (openDialog.ShowDialog() == true)
                {
                    fs = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);

                }
            }
            else
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            }

            if (fs != null)
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() != -1)
                    {
                        var line = sr.ReadLine();
                        string[] elements = line.Split(new char[] { '#' });
                        if (File.Exists(elements[0].Remove(0,8)))
                        {
                            FileInfo fileInfo = new FileInfo(elements[0].Remove(0, 8));
                            FileArt fileart;
                            Filetype filetyp;
                            switch (fileInfo.Extension.ToLower().Trim('.'))
                            {
                                case "jpeg":
                                case "jpg":
                                    fileart = FileArt.Bild;
                                    filetyp = Filetype.JPEG;
                                    break;
                                case "bmp":
                                    fileart = FileArt.Bild;
                                    filetyp = Filetype.BMP;
                                    break;
                                case "png":
                                    fileart = FileArt.Bild;
                                    filetyp = Filetype.PNG;
                                    break;
                                case "mp4":
                                    fileart = FileArt.Video;
                                    filetyp = Filetype.MP4;
                                    break;
                                case "avi":
                                    fileart = FileArt.Video;
                                    filetyp = Filetype.AVI;
                                    break;
                                default:
                                    fileart = FileArt.KeineDef;
                                    filetyp = Filetype.KeineDef;
                                    break;
                            }
                            var item = new ListElementViewModel() { 
                                Filename = new Uri(elements[0].Remove(0, 8)), 
                                Name = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf('.')), 
                                ScrollOffset = Convert.ToDouble(elements[1]), 
                                Art = fileart, 
                                Typ = filetyp,
                                Spiegeln = Convert.ToBoolean(elements[2]),
                                Widht = Convert.ToDouble(elements[3]),
                                Height = Convert.ToDouble(elements[4])};
                            ListBoxItems.Add(item);
                            item.PropertyChanged += ListElementPropertyChanged;
                        }
                    }
                } 
            }
        }
        public void OnSave()
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.Filter = "Textfile (*.txt;|*.txt;)";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var item in ListBoxItems)
                    {
                        sw.Write(
                            item.Filename.ToString() + "#" + 
                            item.ScrollOffset.ToString() + "#" + 
                            item.Spiegeln.ToString() + "#" +
                            item.Widht.ToString() + "#" +
                            item.Height.ToString() +
                            Environment.NewLine);
                    }

                }
            }
        }
        public void OnNew()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Bilder (*.BMP;*.JPG;*.GIF;*PNG;*JPEG)|*.BMP;*.JPG;*.GIF;*PNG;*JPEG|Video (*.mp4; *.avi;*.mpeg)|*.mp4;*.avi;*.mpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    FileArt fileart;
                    Filetype filetyp;
                    switch (fileInfo.Extension.ToLower().Trim('.'))
                    {
                        case "jpeg":
                        case "jpg":
                            fileart = FileArt.Bild;
                            filetyp = Filetype.JPEG;
                            break;
                        case "bmp":
                            fileart = FileArt.Bild;
                            filetyp = Filetype.BMP;
                            break;
                        case "png":
                            fileart = FileArt.Bild;
                            filetyp = Filetype.PNG;
                            break;
                        case "mp4":
                            fileart = FileArt.Video;
                            filetyp = Filetype.MP4;
                            break;
                        case "avi":
                            fileart = FileArt.Video;
                            filetyp = Filetype.AVI;
                            break;
                        default:
                            fileart = FileArt.KeineDef;
                            filetyp = Filetype.KeineDef;
                            break;
                    }
                    if(fileart == FileArt.Bild)
                    {
                        Bitmap Biap = new Bitmap(file);
                        //System.Diagnostics.Debug.WriteLine("File:" + file + " Breite:" + Biap.Width.ToString() + " Höhe:" + Biap.Height + " Verhältniss Bild:"+ (Biap.Width / Biap.Height).ToString() + " Verhätniss Fenster:" + (PresentationViewModelProp.Width/PresentationViewModelProp.Height).ToString());
                    }
                    var item = new ListElementViewModel() { 
                        Filename = new Uri(file), 
                        Name = fileInfo.Name.Substring(0,fileInfo.Name.IndexOf('.')), 
                        ScrollOffset = 0, 
                        Art = fileart, 
                        Typ = filetyp,
                        Height = PresentationViewModelProp.Height,
                        Widht = PresentationViewModelProp.Width};

                    ListBoxItems.Add(item);
                    item.PropertyChanged += ListElementPropertyChanged;
                }
            }
        }
        public void OnNextDirect(ListElementViewModel listElement)
        {
            if (PresentationViewModelProp.IsActive)
            {
                if (PresentationViewModelProp.FrontElement != null && PresentationViewModelProp.BackElement != null)
                {
                    if (PresentationViewModelProp.FrontElement.IsActive)
                    {
                        PresentationViewModelProp.BackElement = ListBoxItems[ListBoxItems.IndexOf(listElement)];
                        ScrollOffsetNextElmt = PresentationViewModelProp.BackElement.ScrollOffset;
                        //Deckkraft Frontelement Runter Fahren
                        Delay.Start();
                    }
                    else
                    {
                        PresentationViewModelProp.FrontElement = ListBoxItems[ListBoxItems.IndexOf(listElement)];
                        ScrollOffsetNextElmt = PresentationViewModelProp.FrontElement.ScrollOffset;
                        PresentationViewModelProp.FrontElement.IsVisible = true;
                        //Deckraft Frontelement hochfahren
                        Delay.Start();
                    } 
                }
            }
            else
            {
                OnStart(listElement);
            }
        }
        private void OnBreiteAnwenden()
        {
            foreach (var item in ListBoxItems)
            {
                item.Widht = PresentationViewModelProp.Width;
            }
        }
        private void OnHoehAnwenden()
        {
            foreach (var item in ListBoxItems)
            {
                item.Height = PresentationViewModelProp.Height;
            }
        }
        private void OnBlack()
        {
            if (PresentationViewModelProp.FrontElement != null && PresentationViewModelProp.BackElement != null)
            {
                FadeOut.Tick += FadeOut_Tick;
                FadeOut.Start(); 
            }
        }
        private void OnFadeIn()
        {
            if (PresentationViewModelProp.FrontElement != null && PresentationViewModelProp.BackElement != null)
            {
                FadeIn.Tick += FadeIn_Tick;
                FadeIn.Start(); 
            }
        }
    }
}
