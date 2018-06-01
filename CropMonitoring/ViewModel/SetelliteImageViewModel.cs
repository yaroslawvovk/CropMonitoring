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

namespace CropMonitoring.ViewModel
{
    class SetelliteImageViewModel : ViewModelBase
    {
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

        #region Async handle image
        RelayCommand _calculateNDVI;
        public ICommand CalculateNDVI
        {
            get
            {
                if (_calculateNDVI == null)
                    return new RelayCommand(CalculateNDVIImageCommand, CanCalculateNDVIImageCommand);
                return _calculateNDVI;
            }
        }
        public async void CalculateNDVIImageCommand(object parameter)
        {

            var bands = imageAccess.GetBandsPath(_selectedFileName);
            LandsatImageProcessor1 imgProc = new LandsatBitmapPaletteWriter();
            imgProc.statusEvent += ImgProc_statusEvent;

            if (bands.band3 != null && bands.band4 != null && bands.outImg != null)
            {
                await Task.Factory.StartNew(() =>
                imgProc.CalculateNDVI(bands.band3, bands.band4, bands.outImg)
                );
                NDVIImageList = imageAccess.GetOutputImageFiles();
                OnPropertyChanged("NDVIImageList");
            }


        }

        public bool CanCalculateNDVIImageCommand(object parameter)
        {
            if (_selectedFileName == null)
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

            var bands = imageAccess.GetBandsPath(_selectedFileName);
            LandsatImageProcessor1 imgProc2 = new LandsatRasterPaletteWriter();
            imgProc2.statusEvent += ImgProc_statusEvent2;

            if (bands.band3 != null && bands.band4 != null && bands.outImg != null)
            {
                await Task.Factory.StartNew(() =>
                imgProc2.CalculateNDVI(bands.band3, bands.band4, bands.outImg)
                );
                NDVIImageList = imageAccess.GetOutputImageFiles();
                OnPropertyChanged("NDVIImageList");
            }


        }

        public bool CanCalculateNDVIImageCommand2(object parameter)
        {
            if (_selectedFileName == null)
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
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteImage == null)
                    return new RelayCommand(DeleteImageCommand, CanDeleteImageCommand);
                return _deleteImage;
            }

        }

        public void DeleteImageCommand(object parameter)
        {

            BitmapImage = null;
            OnPropertyChanged("BitmapImage");
            imageAccess.DeleteImage(_selectedNDVIImage);
            OnPropertyChanged("NDVIImageList");
        }
        public bool CanDeleteImageCommand(object parameter)
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
            OnPropertyChanged("NDVIImageList");
        }
        public bool CanRefreshList(object parameter)
        {
            return true;
        }


    }
}
