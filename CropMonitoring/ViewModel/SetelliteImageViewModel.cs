using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CropMonitoring.Model;
using CropMonitoring.Infrastructure;
using System.Windows.Input;
using CropMonitoring.SetelliteImageProcessor;
using System.Threading;
using System.Windows.Media.Imaging;
using CropMonitoring.UserNotify;

namespace CropMonitoring.ViewModel
{
    class SetelliteImageViewModel : ViewModelBase
    {
        INotifyUser notify = new NotifyMessage();
        SetelliteImageAccess imageAccess = new SetelliteImageAccess();

        List<string> _imageFiles;
        public List<string> ImageFiles
        {
            get
            {
                if (_imageFiles == null)
                    return imageAccess.GetInputImageFiles();
                return _imageFiles;
            }
            set
            {
                _imageFiles = value;
            }
        }

        BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get
            {
                return _bitmapImage;
            }
            set
            {
                _bitmapImage = value;
            }
        }

        string _selectedFileName;
        public string SelectedFileName
        {
            get
            {
                if (_selectedFileName != null)
                {
                    BitmapImage = imageAccess.GetBitmapImage(_selectedFileName);
                    OnPropertyChanged("BitmapImage");
                }
                return null;
            }
            set
            {
                _selectedFileName = value;
            }
        }

        bool _isButtonBlocked1;
        public bool IsButtonBlocked1
        {
            get
            {
                return _isButtonBlocked1;
            }
            set
            {
                _isButtonBlocked1 = value;
            }
        }

        bool _isButtonBlocked2;
        public bool IsButtonBlocked2
        {
            get
            {
                return _isButtonBlocked2;
            }
            set
            {
                _isButtonBlocked2 = value;
            }
        }

        #region Async handle image
        RelayCommand _calculateNDVI;
        public ICommand CalculateNDVI
        {
            get
            {
                if (_calculateNDVI == null)
                {
                   
                    return new RelayCommand(CalculateNDVIImageCommand, CanCalculateNDVIImageCommand);
                }
                return _calculateNDVI;
            }
        }
        public async void CalculateNDVIImageCommand(object parameter)
        {
            IsButtonBlocked1 = true;
            OnPropertyChanged("IsButtonBlocked1");
            var bands = imageAccess.GetBandsPath(_selectedFileName);
            LandsatImageProcessor imgProc = new LandsatBitmapPaletteWriter();
            imgProc.statusEvent += ImgProc_statusEvent;

            if (bands.band3 != null && bands.band4 != null && bands.outImg != null)
            {
                await Task.Factory.StartNew(() =>
                imgProc.CalculateNDVI(bands.band3, bands.band4, bands.outImg)
                );
                NDVIImageList = imageAccess.GetOutputImageFiles();
                OnPropertyChanged("NDVIImageList");           
            }
            IsButtonBlocked1 = false;
            OnPropertyChanged("IsButtonBlocked1");
            notify.Message("The" + _selectedFileName + "  was processed");
        }

        public bool CanCalculateNDVIImageCommand(object parameter)
        {
            if (_selectedFileName == null||IsButtonBlocked1==true)
                return false;
            return true;
        }

        int _progressValue;
        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                _progressValue = value;
            }
        }

        private void ImgProc_statusEvent(object sender, ThresholdReachedEventArgs e)
        {
            ProgressValue = e.Status;
            OnPropertyChanged("ProgressValue");
        }
        #endregion

        #region Async handle image two
        RelayCommand _calculateNDVI2;
        public ICommand CalculateNDVI2
        {
            get
            {
                if (_calculateNDVI2 == null)
                    return new RelayCommand(CalculateNDVIImageCommand2, CanCalculateNDVIImageCommand2);
                return _calculateNDVI2;
            }
        }
        public async void CalculateNDVIImageCommand2(object parameter)
        {
            IsButtonBlocked2 = true;
            OnPropertyChanged("IsButtonBlocked2");
            var bands = imageAccess.GetBandsPath(_selectedFileName);
            LandsatImageProcessor imgProc2 = new LandsatRasterPaletteWriter();
            imgProc2.statusEvent += ImgProc_statusEvent2;

            if (bands.band3 != null && bands.band4 != null && bands.outImg != null)
            {
                await Task.Factory.StartNew(() =>
                imgProc2.CalculateNDVI(bands.band3, bands.band4, bands.outImg)
                );
                NDVIImageList = imageAccess.GetOutputImageFiles();
                OnPropertyChanged("NDVIImageList");
            }
            IsButtonBlocked2 = false;
            OnPropertyChanged("IsButtonBlocked2");
            notify.Message("The" + _selectedFileName + "  was processed");
        }

        public bool CanCalculateNDVIImageCommand2(object parameter)
        {
            if (_selectedFileName == null||IsButtonBlocked2==true)
                return false;
            return true;
        }

        int _progressValue2;
        public int ProgressValue2
        {
            get
            {
                return _progressValue2;
            }
            set
            {
                _progressValue2 = value;
            }
        }

        private void ImgProc_statusEvent2(object sender, ThresholdReachedEventArgs e)
        {
            ProgressValue2 = e.Status;
            OnPropertyChanged("ProgressValue2");
        }

        #endregion

        List<string> _ndviImageList;
        public List<string> NDVIImageList
        {
            get
            {
                if (_ndviImageList == null)
                    return imageAccess.GetOutputImageFiles();
                return _ndviImageList;
            }
            set
            {
                _ndviImageList = value;
            }
        }
        string _selectedNDVIImage;
        public string SelectedNDVIImage
        {
            get
            {
                if (_selectedNDVIImage != null)
                {
                    BitmapImage = imageAccess.GetBitmapNDVIImage(_selectedNDVIImage);
                    OnPropertyChanged("BitmapImage");
                }
                return null;
            }
            set
            {
                _selectedNDVIImage = value;
            }
        }

        RelayCommand _deleteImage;
        public ICommand DeleteImage
        {
            get
            {
                if (_deleteNDVIImage == null)
                    return new RelayCommand(DeleteImageCommand, CanDeleteImageCommand);
                return _deleteNDVIImage;
            }

        }

        public void DeleteImageCommand(object parameter)
        {

            BitmapImage = null;
            OnPropertyChanged("BitmapImage");
            imageAccess.DeleteInputImage(_selectedFileName);
            OnPropertyChanged("ImageFiles");
        }
        public bool CanDeleteImageCommand(object parameter)
        {
            return true;
        }


        RelayCommand _deleteNDVIImage;
        public ICommand DeleteNDVICommand
        {
            get
            {
                if (_deleteNDVIImage == null)
                    return new RelayCommand(DeleteNDVIImageCommand, CanDeleteNDVIImageCommand);
                return _deleteNDVIImage;
            }

        }

        public void DeleteNDVIImageCommand(object parameter)
        {

            BitmapImage = null;
            OnPropertyChanged("BitmapImage");
            imageAccess.DeleteNDVIImage(_selectedNDVIImage);
            OnPropertyChanged("NDVIImageList");
        }
        public bool CanDeleteNDVIImageCommand(object parameter)
        {
            return true;
        }

        RelayCommand _selectImage;
        public ICommand SelectCommand
        {
            get
            {
                if (_deleteNDVIImage == null)
                    return new RelayCommand(SelectImageCommand, CanSelectImageCommand);
                return _deleteNDVIImage;
            }

        }

        public void SelectImageCommand(object parameter)
        {
            BitmapImage = imageAccess.GetBitmapNDVIImage(_selectedNDVIImage);
            OnPropertyChanged("BitmapImage");
        }
        public bool CanSelectImageCommand(object parameter)
        {
            return true;
        }



        RelayCommand _refreshLists;
        public ICommand RefreshList
        {
            get
            {
                if (_refreshLists == null)
                    return new RelayCommand(RefreshListCommand, CanRefreshList);
                return _refreshLists;
            }
        }

        public void RefreshListCommand(object parameter)
        {
            _ndviImageList = imageAccess.GetOutputImageFiles();
            //_imageFiles = imageAccess.GetInputImageFiles();
            //OnPropertyChanged("ImageFiles");
            OnPropertyChanged("NDVIImageList");
        }
        public bool CanRefreshList(object parameter)
        {
            return true;
        }

        RelayCommand _openImages;
        public ICommand OpenImages
        {
            get
            {
                if (_deleteNDVIImage == null)
                    return new RelayCommand(OpenImagesCommand, CanOpenImagesCommand);
                return _deleteNDVIImage;
            }

        }

        public void OpenImagesCommand(object parameter)
        {
            imageAccess.OpenImages();
            OnPropertyChanged("ImageFiles");
        }
        public bool CanOpenImagesCommand(object parameter)
        {
            return true;
        }


    }
}
