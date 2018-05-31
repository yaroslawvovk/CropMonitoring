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

        //string _imageFile;
        //public string ImageFile
        //{
        //    get
        //    {
        //        return _imageFile;
        //    }
        //    set
        //    {
        //        _imageFile = value;
        //    }
        //}
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
            LandsatImageProcessor imgProc = new LandsatImageProcessor();
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

        public bool CanCalculateNDVIImageCommand(object parameter)
        {
            if (_selectedFileName == null)
                return false;
            return true;
        }

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



    }
}
