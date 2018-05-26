using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CropMonitoring.Model;
using CropMonitoring.Infrastructure;
using System.Windows.Input;
using CropMonitoring.SetelliteImageProcessor;

namespace CropMonitoring.ViewModel
{
    class SetelliteImageViewModel : ViewModelBase
    {

        List<string> _imageFiles;
        public List<string> ImageFiles
        {
            get
            {
                if (_imageFiles == null)
                    return SetelliteImageFiles.GetInputImageFiles();
                return _imageFiles;
            }
        }

        string _imageFile;
        public string ImageFile
        {
            get
            {
                return _imageFile;
            }
            set
            {
                _imageFile = value;
            }
        }

        string _selectedFileName;
        public string SelectedFileName
        {
            get
            {
                if (_selectedFileName != null)
                {
                    ImageFile = SetelliteImageFiles.GetImageFile(_selectedFileName);
                    OnPropertyChanged("ImageFile");
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
            var bands = SetelliteImageFiles.GetBandsPath(_selectedFileName);
            LandsatImageProcessor imgProc = new LandsatImageProcessor();

            if (bands.band3 != null && bands.band4 != null && bands.outImg != null)
            {
                await Task.Factory.StartNew(() =>
                imgProc.CalculateNDVI(bands.band3, bands.band4, bands.outImg)
                );
                NDVIImageList = SetelliteImageFiles.GetOutputImageFiles();
                OnPropertyChanged("NDVIImageList");
            }
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
                    return SetelliteImageFiles.GetOutputImageFiles();
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
                    ImageFile = SetelliteImageFiles.GetNDVIImage(_selectedNDVIImage);              
                    OnPropertyChanged("ImageFile");
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
       
                SetelliteImageFiles.DeleteImage(SelectedNDVIImage);
                OnPropertyChanged("NDVIImageList");
            
            
        }
        public bool CanDeleteImageCommand(object parameter)
        {
            return true;
        }



    }
}
